using Microsoft.VisualBasic.FileIO;

namespace FIS_J.Services;

public class GetRemoteCsv
{
	public IReadOnlyDictionary<string, string[]> Data => _Data;
	Dictionary<string, string[]> _Data { get; } = new();

	public string Src { get; }

	public GetRemoteCsv(string srcUrl)
	{
		Src = srcUrl;
	}

	public async Task<bool> ReLoad()
	{
		var res = await HttpService.HttpClient.GetAsync(Src);
		if (!res.IsSuccessStatusCode)
			return false;

		using (var csvParser = new TextFieldParser(await res.Content.ReadAsStreamAsync()))
		{
			csvParser.SetDelimiters(",");

			while (!csvParser.EndOfData)
			{
				var fields = csvParser.ReadFields();
				if (csvParser.LineNumber == 0 || fields.Length < 2)
					continue;

				_Data[fields[0]] = fields[1..].ToArray();
			}
		}

		return true;
	}
}
