using AirTote.AISJapanParser.EAIP;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace AirTote.AISJapanParser.Tests.EAIP;

public class InfoDatesTest
{
	static readonly IReadOnlyList<AIPDateInfo> ExpectedAIPDateList = new List<AIPDateInfo>()
	{
		new(new(2022, 9, 8), new(2022, 9, 8), false),
		new(new(2022, 9, 8), new(2022, 10, 1), false),
		new(new(2022, 9, 8), new(2022, 10, 6), true),
		new(new(2022, 9, 8), new(2022, 10, 30), false),
		new(new(2022, 8, 11), new(2022, 8, 11), false),
		new(new(2022, 8, 11), new(2022, 9, 8), true),
		new(new(2022, 8, 11), new(2022, 10, 1), false),
		new(new(2022, 8, 11), new(2022, 10, 6), true),
	};

	[Test]
	public async Task InitTest()
	{
		string html = await Utils.ReadAllTextFromTestCaseFilesAsync("DomesticAIP.html");
		InfoDates d = await InfoDates.GetAsync(html);

		Assert.That(d.AIPDateList, Has.Count.EqualTo(8));
		Assert.That(d.AIPDateList, Is.SubsetOf(ExpectedAIPDateList));
	}

	[Test]
	public async Task CurrentPropTest_1()
	{
		string html = await Utils.ReadAllTextFromTestCaseFilesAsync("DomesticAIP.html");
		InfoDates d = new(await new HtmlParser().ParseDocumentAsync(html), new(2022, 9, 10));

		Assert.That(d.Current, Is.EqualTo(new AIPDateInfo(new(2022, 9, 8), new(2022, 9, 8), false)));
	}

	[Test]
	public async Task CurrentPropTest_2()
	{
		string html = await Utils.ReadAllTextFromTestCaseFilesAsync("DomesticAIP.html");
		InfoDates d = new(await new HtmlParser().ParseDocumentAsync(html), new(2022, 10, 10));

		Assert.That(d.Current, Is.EqualTo(new AIPDateInfo(new(2022, 9, 8), new(2022, 10, 6), true)));
	}
}
