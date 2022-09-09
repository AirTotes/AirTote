using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AirTote.Services;

using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace AirTote.AISJapanParser.EAIP;

public record MenuItem(string Title, string FileName, AIP_PartName Category);

public record MenuItem_Aerodrome(string Title, string FileName, string ICAO, string AerodromeName, int ItemNum, string ItemTitle) : MenuItem(Title, FileName, AIP_PartName.AD);

public enum AIP_PartName
{
	Unknown,
	GEN,
	ENR,
	AD
}

public class Menu
{
	const string fname = "JP-menu-en-JP.html";

	public static async Task<Menu> GetAsync(AISJapan ais, AIPDateInfo dateInfo)
		=> new(await ais.GetPageDocument(dateInfo.GetUrl(fname)));

	public IReadOnlyList<MenuItem> ItemList { get; }

	public Menu(IDocument doc)
	{
		var divList = doc.QuerySelectorAll("body > :not(div.tab):not(div.H1)");
		List<MenuItem> itemList = new();
		ItemList = itemList;

		foreach (var div in divList)
		{
			foreach (var anchor in div.GetElementsByTagName("a").Select(v => v as IHtmlAnchorElement))
			{
				if (anchor is null)
					continue;

				string fname = Path.GetFileName(anchor.Href);
				if (fname == "#")
					continue;

				AIP_PartName part = fname switch
				{
					string s when s.StartsWith("JP-GEN") => AIP_PartName.GEN,
					string s when s.StartsWith("JP-ENR") => AIP_PartName.ENR,
					string s when s.StartsWith("JP-AD") => AIP_PartName.AD,
					_ => AIP_PartName.Unknown
				};

				string title = anchor.Text().Replace("\n", " ").Trim();
				if (part != AIP_PartName.AD || !fname.StartsWith("JP-AD-2-"))
					itemList.Add(new(title, fname, part));
				else
					itemList.Add(GetAerodromeItem(title, fname));
			}
		}
	}

	Dictionary<string, string> IcaoAerodromeNameDict { get; } = new();
	MenuItem GetAerodromeItem(string Title, string fname)
	{
		string[] fname_tag = fname.Split('#', StringSplitOptions.RemoveEmptyEntries);

		if (fname_tag.Length < 2 || fname_tag[1] == "AD-2")
			return new(Title, fname, AIP_PartName.AD);

		string[] segs = fname_tag[0].Split('-');
		string icao = segs[3];

		if (fname_tag[1] == $"AD-2.{icao}")
		{
			string aeroaromeName = Title.Remove(0, 5);

			IcaoAerodromeNameDict[icao] = aeroaromeName;

			return new MenuItem_Aerodrome(Title, fname, icao, aeroaromeName, 0, aeroaromeName);
		}

		string itemTitle = Title;
		if (!int.TryParse(fname_tag[1].Remove(0, 10), out int itemNum))
			itemNum = -1;
		else
			itemTitle = Title.Remove(0, $"AD 2.{itemNum} {icao} ".Length);

		return new MenuItem_Aerodrome(Title, fname, icao, IcaoAerodromeNameDict[icao], itemNum, itemTitle);
	}
}
