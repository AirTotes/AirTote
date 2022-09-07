using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using AirTote.Services.Types;

namespace AirTote.Services;

public class AirRouteProvider
{
	public static async Task<LowerATSRoute?> GetLowerATSRouteAsync(DateOnly PublicationDate, DateOnly EffectiveDate)
	{
		var httpResponse = await HttpService.HttpClient.GetAsync($"https://d.airtote.jp/AISJapan/{PublicationDate:yyyyMMdd}/{EffectiveDate:yyyyMMdd}/LowerATSRoute.json");

		if (httpResponse.StatusCode != HttpStatusCode.OK)
			return null;

		return JsonSerializer.Deserialize<LowerATSRoute>(await httpResponse.Content.ReadAsStreamAsync());
	}

	public static LowerATSRoute? ParseLowerATSRouteJson(string s)
	{
		if (string.IsNullOrWhiteSpace(s))
			return null;

		return JsonSerializer.Deserialize<LowerATSRoute>(s);
	}
}
