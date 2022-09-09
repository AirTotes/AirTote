using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AirTote.Services;

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace AirTote.AISJapanParser.EAIP;

public record AIPDateInfo(DateOnly PublicationDate, DateOnly EffectuveDate, bool IsAIRAC = false)
{
	public string GetUrl(string fname)
		=> $"https://aisjapan.mlit.go.jp/html/AIP/html/{PublicationDate:yyyyMMdd}/eAIP/{PublicationDate:yyyyMMdd}/{fname}";
}

public class InfoDates
{
	public IReadOnlyList<AIPDateInfo> AIPDateList { get; }
	public AIPDateInfo? Current { get; }

	public static async Task<InfoDates> GetAsync(AISJapan ais)
		=> new(await ais.GetPageDocument("https://aisjapan.mlit.go.jp/html/AIP/html/DomesticAIP.do"));
	public static async Task<InfoDates> GetAsync(string page)
		=> new(await new HtmlParser().ParseDocumentAsync(page));

	public InfoDates(IDocument doc) : this(doc, DateOnly.FromDateTime(DateTime.Today)) { }
	public InfoDates(IDocument doc, DateOnly Today)
	{
		List<AIPDateInfo> _AIPDates = new();
		AIPDateList = _AIPDates;


		IHtmlTableElement? root_table = doc.QuerySelector<IHtmlTableElement>("body > table");

		if (root_table is null)
			return;

		var each_pub_date_table = root_table.QuerySelectorAll<IHtmlTableElement>("* > table");

		foreach (var pubs in each_pub_date_table)
		{
			foreach (var row in pubs.Rows)
			{
				if (!DateOnly.TryParse(row.Cells[1]?.Text(), out var EffectiveDate))
					continue;
				if (!DateOnly.TryParse(row.Cells[2]?.Text(), out var PublicationDate))
					continue;

				bool IsAIRAC = row.Cells[3].Text() == "AIRAC";

				_AIPDates.Add(new(PublicationDate, EffectiveDate, IsAIRAC));
			}
		}

		Current = AIPDateList[0];
		foreach (var info in AIPDateList)
		{
			if (Today < info.PublicationDate)
				continue;
			if (info.EffectuveDate <= Today && Current.EffectuveDate < info.EffectuveDate)
				Current = info;
		}
	}
}
