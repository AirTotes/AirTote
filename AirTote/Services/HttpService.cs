using System.Reflection;

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
}

