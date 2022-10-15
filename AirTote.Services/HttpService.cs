using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace AirTote.Services;

public class HttpService
{
	static public HttpClient HttpClient { get; }

	static HttpService()
	{
		HttpClient = new()
		{
			Timeout = new(0, 0, 2),
		};

		HttpClient.DefaultRequestHeaders.UserAgent.Add(new(
			Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName),
			Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0"
			));
	}

	static public string USER_AGENT => HttpService.HttpClient.DefaultRequestHeaders.UserAgent.ToString();

	static public async Task<byte[]> GetByteArrayAsync(Uri uri)
	{
		try
		{
			using HttpResponseMessage res = await HttpService.HttpClient.GetAsync(uri);

			return res.IsSuccessStatusCode ? await res.Content.ReadAsByteArrayAsync() : Array.Empty<byte>();
		}
		catch (TaskCanceledException)
		{
			// When Timeout
			return Array.Empty<byte>();
		}
	}
}

