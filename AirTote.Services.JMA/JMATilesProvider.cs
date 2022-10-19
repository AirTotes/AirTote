using System;
using System.Collections.Generic;
using System.IO;
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

	static async Task<TargetTimes[]> GetTargetTimes(string fileName)
	{
		try
		{
			using Stream stream = await HttpService.HttpClient.GetStreamAsync("https://www.jma.go.jp/bosai/jmatile/data/nowc/" + fileName);

			if (stream.CanRead)
				return await JsonSerializer.DeserializeAsync<TargetTimes[]>(stream) ?? Array.Empty<TargetTimes>();
		}
		catch (TaskCanceledException)
		{
			System.Diagnostics.Debug.WriteLine($"{nameof(JMATilesProvider)}.{nameof(GetTargetTimes)}({fileName}): Http Request Timeout");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}

		return Array.Empty<TargetTimes>();
	}

	public static async Task<JMATilesProvider> Init()
	{
		TargetTimes? latest = null;
		List<TargetTimes> hrpns = new();
		List<TargetTimes> thns = new();
		List<TargetTimes> trns = new();
		List<TargetTimes> liden = new();

		// HRPNs history
		TargetTimes[] targetTimes_N1 = await GetTargetTimes("targetTimes_N1.json");

		if (targetTimes_N1 is not null)
		{
			hrpns = targetTimes_N1
				.Where(v => v.elements.Contains(TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS))
				.ToList();

			latest = hrpns.MaxBy(v => v.validtime);
		}

		TargetTimes[] targetTimes_N2 = await GetTargetTimes("targetTimes_N2.json");

		if (targetTimes_N2 is not null)
		{
			hrpns.AddRange(targetTimes_N2.Where(v => v.elements.Contains(TargetTimes.TYPE_HIGH_RESOLUTION_PRECIPITATION_NOWCASTS)));
		}

		// [0]が最新(Max)、[Last]が過去(Min)にする
		hrpns.Sort((a, b) => string.Compare(b.validtime, a.validtime));

		TargetTimes[] targetTimes_N3 = await GetTargetTimes("targetTimes_N3.json");

		if (targetTimes_N3 is not null)
		{
			thns = targetTimes_N3.Where(v => v.elements.Contains(TargetTimes.TYPE_THUNDER_NOWCASTS)).ToList();
			trns = targetTimes_N3.Where(v => v.elements.Contains(TargetTimes.TYPE_TORNADO_NOWCASTS)).ToList();
			liden = targetTimes_N3.Where(v => v.elements.Contains(TargetTimes.TYPE_LIGHTNING_DETECTION_NETWORK_SYSTEM)).ToList();
		}

		return new(latest, hrpns, thns, trns, liden);
	}

	public static TileLayer GetLayer(TargetTimes targetTime, NowC_Types type)
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
				@$"https://www.jma.go.jp/bosai/jmatile/data/nowc/{targetTime.basetime}/none/{targetTime.validtime}/surf/{type_name}/" + "{z}/{x}/{y}.png",
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
