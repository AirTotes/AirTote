using System;

namespace AirTote.Services.Tests;

public class AirRouteProviderTests
{
	[OneTimeSetUp]
	public void Init()
	{
		HttpService.HttpClient.Timeout = new(0, 0, 10);
	}

	[Test]
	public void SimpleJsonTest_NoContent()
	{
		var result = AirRouteProvider.ParseLowerATSRouteJson("");

		Assert.That(result, Is.Null);
	}

	[Test]
	public void SimpleJsonTest_PointListNull()
	{
		ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() => AirRouteProvider.ParseLowerATSRouteJson("{\"PointList\": null,\"RouteDict\": {}}"));

		Assert.That(exception, Is.Not.Null);

		Assert.That(exception.ParamName, Is.EqualTo("PointList"));
	}

	[Test]
	public void SimpleJsonTest_RouteDictNull()
	{
		ArgumentNullException? exception = Assert.Throws<ArgumentNullException>(() => AirRouteProvider.ParseLowerATSRouteJson("{\"PointList\": [],\"RouteDict\": null}"));

		Assert.That(exception, Is.Not.Null);

		Assert.That(exception.ParamName, Is.EqualTo("RouteDict"));
	}

	[Test]
	public void SimpleJsonTest_EmptyElem()
	{
		var result = AirRouteProvider.ParseLowerATSRouteJson("{\"PointList\": [],\"RouteDict\": {}}");

		Assert.That(result, Is.Not.Null);

		Assert.Multiple(() =>
		{
			Assert.That(result.PointDict, Has.Count.Zero);
			Assert.That(result.PointList, Has.Length.Zero);
			Assert.That(result.RouteDict, Has.Count.Zero);
		});
	}

	[Test]
	public async Task LoadFromRemoteTest()
	{
		var result = await AirRouteProvider.GetLowerATSRouteAsync(new(2022, 9, 8), new(2022, 9, 8));

		Assert.That(result, Is.Not.Null);

		Assert.Multiple(() =>
		{
			Assert.That(result.PointList, Is.Not.Null);
			Assert.That(result.PointDict, Is.Not.Null);
			Assert.That(result.RouteDict, Is.Not.Null);
		});

		Assert.Multiple(() =>
		{
			Assert.That(result.PointList, Has.Length.GreaterThan(0));
			Assert.That(result.PointDict, Has.Count.GreaterThan(0));
			Assert.That(result.PointDict.Count == result.PointList.Length, Is.True);
			Assert.That(result.RouteDict, Has.Count.GreaterThan(0));
		});
	}
}
