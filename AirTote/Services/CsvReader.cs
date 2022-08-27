using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AirTote.Services;

public class MyCsvReader
{
	public string GroupName { get; set; }
	public string ShortTitle { get; set; }
	public string FullTitle { get; set; }
	public string URL { get; set; }
	public bool IsPublic { get; set; }

	public async Task<string> ReadTextFileAsync(string filePath)
	{
		using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
		using StreamReader reader = new StreamReader(fileStream);

		return await reader.ReadToEndAsync();
	}


	// List<T>
	public async Task<List<MyCsvReader>> ReadCsvFileAsync(string filePath)
	{
		using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
		using StreamReader reader = new StreamReader(fileStream);

		using CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
		IEnumerable<MyCsvReader> records = csv.GetRecords<MyCsvReader>();

		return records.ToList();
	}
}
