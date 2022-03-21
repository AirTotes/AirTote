#nullable disable

using FIS_J.Models;

using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIS_J.Services
{
	public class NoContentException : Exception { }

	internal class AVWX
	{
		string API_TOKEN { get; }

		string BASE_URL { get; }

		public AVWX(in string token, in string base_url = "https://avwx.rest/api/")
		{
			if (token == null)
				throw new ArgumentNullException("token");
			if (base_url == null)
				throw new ArgumentNullException("base_url");
			if (string.IsNullOrEmpty(token))
				throw new ArgumentException("token cannot be empty");
			if (string.IsNullOrEmpty(base_url))
				throw new ArgumentException("base_url cannot be empty");

			API_TOKEN = token;
			BASE_URL = base_url;
		}

		public async Task<string> GetSanitizedMETAR(ICAOCode code) => (await GetMETAR(code)).sanitized;
		public async Task<METAR> GetMETAR(ICAOCode code)
		{
			string endpoint = $"{BASE_URL}metar/{code}";
			using HttpClient client = new();
			client.DefaultRequestHeaders.Authorization = new(API_TOKEN);
			var result = await client.GetAsync(endpoint);

			if (!result.IsSuccessStatusCode)
				return null;
			if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
				throw new NoContentException();

			return JsonConvert.DeserializeObject<METAR>(await result.Content.ReadAsStringAsync());
		}

		public async Task<string> GetSanitizedTAF(ICAOCode code) => (await GetTAF(code)).sanitized;
		public async Task<TAF> GetTAF(ICAOCode code)
		{
			string endpoint = $"{BASE_URL}taf/{code}";
			using HttpClient client = new();
			client.DefaultRequestHeaders.Authorization = new(API_TOKEN);
			var result = await client.GetAsync(endpoint);
			if (!result.IsSuccessStatusCode)
				return null;
			if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
				throw new NoContentException();

			return JsonConvert.DeserializeObject<TAF>(await result.Content.ReadAsStringAsync());
		}

		public async Task<Station> GetStationInformation(ICAOCode code)
		{
			string endpoint = $"{BASE_URL}station/{code}";
			using HttpClient client = new();
			client.DefaultRequestHeaders.Authorization = new(API_TOKEN);
			var result = await client.GetAsync(endpoint);
			if (!result.IsSuccessStatusCode)
				return null;
			if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
				throw new NoContentException();

			return JsonConvert.DeserializeObject<Station>(await result.Content.ReadAsStringAsync());
		}

		public class Meta
		{
			public string timestamp { get; set; }
			public string stations_updated { get; set; }
		}
		public class Units
		{
			public string accumulation { get; set; }
			public string altimeter { get; set; }
			public string altitude { get; set; }
			public string temperture { get; set; }
			public string visibility { get; set; }
			public string wind_speed { get; set; }
		}
		public class Time
		{
			public string repr { get; set; }
			public DateTimeOffset? dt { get; set; }
		}
		public class ReprAndValueAndSpoken
		{
			public string repr { get; set; }
			public int? value { get; set; }
			public string spoken { get; set; }
		}
		public class ReprAndValue
		{
			public string repr { get; set; }
			public int? value { get; set; }
		}
		public class ReprAndSpoken
		{
			public string repr { get; set; }
			public string spoken { get; set; }
		}
		public class Cloud
		{
			public string repr { get; set; }
			public string type { get; set; }
			public int? altitude { get; set; }
			public object modifier { get; set; }
			public object direction { get; set; }
		}
		public class Forecast
		{
			public object altimeter { get; set; }
			public Cloud[] clouds { get; set; }
			public string flight_rules { get; set; }
			public object[] other { get; set; }
			public ReprAndValueAndSpoken visibility { get; set; }
			public ReprAndValueAndSpoken wind_direction { get; set; }
			public ReprAndValueAndSpoken wind_gust { get; set; }
			public ReprAndValueAndSpoken wind_speed { get; set; }
			public ReprAndSpoken[] wx_codes { get; set; }
			public Time end_time { get; set; }
			public object[] icing { get; set; }
			public object probability { get; set; }
			public string raw { get; set; }
			public string sanitized { get; set; }
			public Time start_time { get; set; }
			public Time transition_start { get; set; }
			public object[] turbulence { get; set; }
			public string type { get; set; }
			public object wind_shear { get; set; }
			public string summary { get; set; }
		}
		public class RemarksInfo
		{
			public ReprAndValueAndSpoken maximum_temperature_6 { get; set; }
			public ReprAndValueAndSpoken minimum_temperature_6 { get; set; }
			public ReprAndValueAndSpoken pressure_tendency { get; set; }
			public ReprAndValueAndSpoken precip_36_hours { get; set; }
			public ReprAndValueAndSpoken precip_24_hours { get; set; }
			public ReprAndValueAndSpoken sunshine_minutes { get; set; }
			public ReprAndValue[] codes { get; set; }
			public ReprAndValueAndSpoken dewpoint_decimal { get; set; }
			public ReprAndValueAndSpoken maximum_temperature_24 { get; set; }
			public ReprAndValueAndSpoken minimum_temperature_24 { get; set; }
			public object precip_hourly { get; set; }
			public ReprAndValueAndSpoken sea_level_pressure { get; set; }
			public ReprAndValueAndSpoken snow_depth { get; set; }
			public ReprAndValueAndSpoken temperature_decimal { get; set; }
		}

		public class METAR
		{
			public Meta meta { get; set; }
			public ReprAndValueAndSpoken altimeter { get; set; }
			public Cloud[] clouds { get; set; }
			public string flight_rules { get; set; }
			public object[] other { get; set; }
			public ReprAndValueAndSpoken visibility { get; set; }
			public ReprAndValueAndSpoken wind_direction { get; set; }
			public ReprAndValueAndSpoken wind_gust { get; set; }
			public ReprAndValueAndSpoken wind_speed { get; set; }
			public ReprAndSpoken[] wx_codes { get; set; }
			public string raw { get; set; }
			public string sanitized { get; set; }
			public string station { get; set; }
			public Time time { get; set; }
			public string remarks { get; set; }
			public ReprAndValueAndSpoken dewpoint { get; set; }
			public double relative_humidity { get; set; }
			public RemarksInfo remarks_info { get; set; }
			public object[] runway_visibility { get; set; }
			public ReprAndValueAndSpoken temperature { get; set; }
			public object[] wind_variable_direction { get; set; }
			public int? density_altitude { get; set; }
			public int? pressure_altitude { get; set; }
			public Units units { get; set; }
		}

		public class TAF
		{
			public Meta meta { get; set; }
			public string raw { get; set; }
			public string sanitized { get; set; }
			public string station { get; set; }
			public Time time { get; set; }
			public string remarks { get; set; }
			public Forecast[] forecast { get; set; }
			public object start_time { get; set; }
			public object end_time { get; set; }
			public object max_temp { get; set; }
			public object min_temp { get; set; }
			public object alts { get; set; }
			public object temps { get; set; }
			public object remarks_info { get; set; }
			public Units units { get; set; }
		}

		public class Station
		{
			public string city { get; set; }
			public string country { get; set; }
			public int elevation_ft { get; set; }
			public int elevation_m { get; set; }
			public string iata { get; set; }
			public string icao { get; set; }
			public double latitude { get; set; }
			public double longitude { get; set; }
			public string name { get; set; }
			public string note { get; set; }
			public bool reporting { get; set; }
			public Runway[] runways { get; set; }
			public string state { get; set; }
			public string type { get; set; }
			public string website { get; set; }
			public string wiki { get; set; }
		}

		public class Runway
		{
			public int length_ft { get; set; }
			public int length_m { get; set; }
			public string ident1 { get; set; }
			public string ident2 { get; set; }
		}
	}
}
