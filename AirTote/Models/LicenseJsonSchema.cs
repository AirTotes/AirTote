namespace AirTote.Models;

public interface ILicenseJsonSchema
{
	string id { get; set; }
	string? version { get; set; }
	string resolvedVersion { get; set; }
	string? license { get; set; }
	string? licenseDataType { get; set; }
	string? licenseUrl { get; set; }
	string? author { get; set; }
	string? projectUrl { get; set; }
	string? copyrightText { get; set; }
}

public class LicenseJsonSchema : ILicenseJsonSchema
{
	public string id { get; set; } = "";
	public string? version { get; set; }
	public string resolvedVersion { get; set; } = "";
	public string? license { get; set; }
	public string? licenseDataType { get; set; }
	public string? licenseUrl { get; set; }
	public string? author { get; set; }
	public string? projectUrl { get; set; }
	public string? copyrightText { get; set; }
}
