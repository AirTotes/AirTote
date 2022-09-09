using AirTote.AISJapanParser.EAIP;

using AngleSharp.Html.Parser;

namespace AirTote.AISJapanParser.Tests.EAIP;

public class MenuTests
{
	static string html = "";

	[SetUp]
	public async Task SetUp()
	{
		html = await Utils.ReadAllTextFromTestCaseFilesAsync("JP-menu-en-JP.html");
	}

	[Test]
	public async Task ItemCountTest()
	{
		Menu menu = new(await new HtmlParser().ParseDocumentAsync(html));

		Assert.That(menu.ItemList, Is.Not.Null);
		Assert.That(menu.ItemList, Has.Count.EqualTo(3402));
	}
}
