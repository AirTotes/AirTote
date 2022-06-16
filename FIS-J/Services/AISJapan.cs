using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIS_J.Services
{
	internal class AISJapan : IDisposable
	{
		// AngleSharp login ref : https://neue.cc/2021/12/04.html

		IBrowsingContext Ctx { get; } = BrowsingContext.New(Configuration.Default.WithDefaultLoader().WithDefaultCookies());
		Task<IDocument> WhatsNew { get; }

		public AISJapan(in string id, in string password)
		{
			WhatsNew = Ctx.OpenAsync(
				DocumentRequest.PostAsUrlencoded(
					new Url("https://aisjapan.mlit.go.jp/LoginAction.do"),
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
			Ctx.Dispose();
		}

		public async Task<IHtmlTableRowElement> GetWhatsNewAsync()
		{
			var whatsnew = await WhatsNew;
			return whatsnew.QuerySelector<IHtmlTableRowElement>("body > table > tbody > tr:nth-child(3)");
		}

		public async Task<IDocument> GetAIPAsync()
		{
			var whatsnew = await WhatsNew;

			var menu_aip_list = whatsnew.GetElementsByName("menu-aip");
			if (menu_aip_list is null || menu_aip_list.Length < 1)
				throw new Exception("cannot find menu-aip");

			return await (menu_aip_list[0] as IHtmlImageElement).GetAncestor<IHtmlAnchorElement>().NavigateAsync();
		}

		public async Task<string> GetPage(string url)
		{
			var result = await Ctx.OpenAsync(url);
			return result.ToHtml();
		}
	}
}
