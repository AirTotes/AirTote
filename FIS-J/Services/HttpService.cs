namespace FIS_J.Services;

public class HttpService
{
	static public HttpClient HttpClient { get; } = new()
	{
		Timeout = new(0, 0, 2),
	};
}

