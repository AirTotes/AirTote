using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirTote.Services
{
	internal class AISJapan : IDisposable
	{
		// AngleSharp login ref : https://neue.cc/2021/12/04.html

		IBrowsingContext Ctx { get; } = BrowsingContext.New(Configuration.Default.WithDefaultLoader().WithDefaultCookies());
		Task<IDocument> WhatsNew { get; }

		static Url LoginPageUrl { get; } = new("https://aisjapan.mlit.go.jp/LoginAction.do");

		public AISJapan(in string id, in string password)
		{
			WhatsNew = Ctx.OpenAsync(
				DocumentRequest.PostAsUrlencoded(
					LoginPageUrl,
					new Dictionary<string, string>
					{
						{ "formName", "ais-web" },
						{ "userID", id },
						{ "password", password },
					}
				)
			);
		}

		public void Dispose()
		{
			if (WhatsNew.IsCompleted)
				WhatsNew.Result.Dispose();
			WhatsNew.Dispose();

			Ctx.Dispose();
		}

		public async Task<string?> GetSignInError()
		{
			var whatsnew = await WhatsNew;
			if (whatsnew.Url != LoginPageUrl.ToString())
				return null;

			List<string> list = new();

			foreach (var v in whatsnew.Scripts)
			{
				if (!string.IsNullOrWhiteSpace(v.Source))
					continue;

				var result = System.Text.RegularExpressions.Regex.Match(v.Text, "alert\\(([^\\(\\)]+)\\)");
				if (!result.Success)
					continue;

				int prefixLen = "alert('".Length;
				list.Add(result.Value.Substring(prefixLen, result.Value.Length - prefixLen - 2).Trim());
			}

			if (list.Count <= 0)
				return null;

			return string.Join('\n', list);
		}

		public async Task<IHtmlTableRowElement?> GetWhatsNewAsync()
		{
			var whatsnew = await WhatsNew;
			return whatsnew.QuerySelector<IHtmlTableRowElement>("body > table > tbody > tr:nth-child(3)");
		}

		public async Task<IDocument?> GetAIPAsync()
		{
			var whatsnew = await WhatsNew;

			var menu_aip_list = whatsnew.GetElementsByName("menu-aip");
			if (menu_aip_list is null || menu_aip_list.Length < 1)
				throw new Exception("cannot find menu-aip");

			var ancestor = (menu_aip_list[0] as IHtmlImageElement)?.GetAncestor<IHtmlAnchorElement>();
			if (ancestor is null)
				return null;

			return await ancestor.NavigateAsync();
		}

		public async Task<string> GetPage(string url)
		{
			var result = await Ctx.OpenAsync(url);
			return result.ToHtml();
		}
	}
}
