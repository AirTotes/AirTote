using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FIS_J.Models
{
	public static class AirportInfo
	{
		static Dictionary<string, APInfo> AirportInfoDic { get; } = new();

		public static async Task<APInfo?> GetAPInfoAsync(string icao)
		{
			var dic = await getAPInfoDic();

			return dic.TryGetValue(icao, out var value) ? value : null;
		}

		public static async Task<Dictionary<string, APInfo>> getAPInfoDic()
		{
			if (AirportInfoDic.Keys.Count > 0)
				return AirportInfoDic;

			var asm = IntrospectionExtensions.GetTypeInfo(typeof(AirportInfo)).Assembly;

			using var stream = await FileSystem.OpenAppPackageFileAsync("airport_list.json");
			if (stream is null)
				return new();
			using StreamReader reader = new(stream);

			string json = await reader.ReadToEndAsync();
			var json_obj = JsonConvert.DeserializeObject<APInfo[]>(json);
			if (json_obj is null)
				return AirportInfoDic;

			foreach (var d in json_obj)
				AirportInfoDic[d.icao] = d;

			return AirportInfoDic;
		}

		public class APInfo
		{
			public string aid { get; set; } = string.Empty;
			public string icao { get; set; } = string.Empty;
			public string iata { get; set; } = string.Empty;
			public string faa { get; set; } = string.Empty;
			public LatLng coordinates { get; set; } = new();
			public Country country { get; set; } = new();
			public string timezone { get; set; } = string.Empty;
			public string name { get; set; } = string.Empty;
			public string servedCity { get; set; } = string.Empty;
		}

		public class LatLng
		{
			public double latitude { get; set; }
			public double longitude { get; set; }
		}

		public class Country
		{
			public string iso2 { get; set; } = string.Empty;
			public string iso3 { get; set; } = string.Empty;
			public int isoNumeric { get; set; }
			public string name { get; set; } = string.Empty;
			public string officialName { get; set; } = string.Empty;
			public string locationIdentifierName { get; set; } = string.Empty;
		}
	}
}
