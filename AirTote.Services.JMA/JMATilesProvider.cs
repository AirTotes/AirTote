using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AirTote.Services.JMA.Models;

using BruTile.Predefined;
using BruTile.Web;

using Mapsui.Tiling.Layers;

namespace AirTote.Services.JMA;

public enum NowC_Types
{
	HRPNs,
	THNs,
	TRNs,
	LIDEN,
}

public class JMATilesProvider
{
	// hrpns valid history list: https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N1.json
	// hrpns forecast list: https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N2.json
	// others list: https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N3.json

	public TargetTimes? HRPNs_Latest { get; }

	public IReadOnlyList<TargetTimes> HRPNsTimeList { get; }
	public IReadOnlyList<TargetTimes> THNsTimeList { get; }
	public IReadOnlyList<TargetTimes> TRNsTimeList { get; }
	public IReadOnlyList<TargetTimes> LIDENTimeList { get; }

	private JMATilesProvider(
		TargetTimes? hrpns_latest,
		IReadOnlyList<TargetTimes> hrpns,
		IReadOnlyList<TargetTimes> thns,
		IReadOnlyList<TargetTimes> trns,
		IReadOnlyList<TargetTimes> liden)
	{
		HRPNs_Latest = hrpns_latest;

		HRPNsTimeList = hrpns;
		THNsTimeList = thns;
		TRNsTimeList = trns;
		LIDENTimeList = liden;
	}

	public static async Task<JMATilesProvider> Init()
	{
		TargetTimes? latest = null;
		List<TargetTimes> hrpns = new();
		List<TargetTimes> thns = new();
		List<TargetTimes> trns = new();
		List<TargetTimes> liden = new();

		// HRPNs history
		using (var stream = await HttpService.HttpClient.GetStreamAsync("https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N1.json"))
		{
			var result = await JsonSerializer.DeserializeAsync<TargetTimes[]>(stream);

			if (result is not null)
			{
				hrpns = result
					.Where(v => v.elements.Contains(TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS))
					.ToList();

				latest = hrpns.MaxBy(v => v.validtime);
			}
		}

		using (var stream = await HttpService.HttpClient.GetStreamAsync("https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N2.json"))
		{
			var result = await JsonSerializer.DeserializeAsync<TargetTimes[]>(stream);

			if (result is not null)
			{
				hrpns.AddRange(result.Where(v => v.elements.Contains(TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS)));
			}
		}

		hrpns.Sort((a, b) => DateTime.Compare(a.validtime, b.validtime));

		using (var stream = await HttpService.HttpClient.GetStreamAsync("https://www.jma.go.jp/bosai/jmatile/data/nowc/targetTimes_N3.json"))
		{
			var result = await JsonSerializer.DeserializeAsync<TargetTimes[]>(stream);

			if (result is not null)
			{
				thns = result.Where(v => v.elements.Contains(TargetTimes.TYPE_THUNDER_NOWCASTS)).ToList();
				trns = result.Where(v => v.elements.Contains(TargetTimes.TYPE_TORNADO_NOWCASTS)).ToList();
				liden = result.Where(v => v.elements.Contains(TargetTimes.TYPE_LIGHTNING_DETECTION_NETWORK_SYSTEM)).ToList();
			}
		}

		return new(latest, hrpns, thns, trns, liden);
	}

	public TileLayer GetLayer(TargetTimes targetTime, NowC_Types type)
	{
		string type_name = type switch
		{
			NowC_Types.HRPNs => "hrpns",
			NowC_Types.THNs => "thns",
			NowC_Types.TRNs => "trns",
			_ => throw new ArgumentException($"The type `{type}` is not supported", nameof(type))
		};

		string name = type switch
		{
			NowC_Types.HRPNs => "雨雲の動き",
			NowC_Types.THNs => "雷活動度",
			NowC_Types.TRNs => "竜巻発生確度",
			_ => "不明なデータ"
		};

		TileLayer layer = new(new HttpTileSource(
				new GlobalSphericalMercator(4, 10),
				@$"https://www.jma.go.jp/bosai/jmatile/data/nowc/{targetTime.basetime:yyyyMMddhhmmss}/none/{targetTime.validtime:yyyyMMddhhmmss}/surf/{type_name}/" + "{z}/{x}/{y}.png",
				name: name,
				tileFetcher: HttpService.GetByteArrayAsync,
				userAgent: HttpService.USER_AGENT,
				attribution: new("出典: 気象庁 ナウキャスト", "https://www.jma.go.jp/bosai/nowc/")
			))
		{
			Name = name,
		};

		layer.Attribution.Enabled = false;
		layer.IsMapInfoLayer = false;

		return layer;
	}
}
