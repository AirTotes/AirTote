using AirTote.Services;

namespace AirTote;

public static class AISJapanUtils
{
	public const string SEC_STORAGE_KEY_USER = "AIS_JAPAN_USERNAME";
	public const string SEC_STORAGE_KEY_PASS = "AIS_JAPAN_PASSWORD";

	static public Task<AISJapan> FromSecureStorageAsync(ISecureStorage secureStorage)
		=> FromSecureStorageIfNeededAsync(secureStorage, null);

	static public async Task<AISJapan> FromSecureStorageIfNeededAsync(ISecureStorage secureStorage, AISJapan? aisJapan)
	{
		string? id = await secureStorage.GetAsync(SEC_STORAGE_KEY_USER);
		string? pass = await secureStorage.GetAsync(SEC_STORAGE_KEY_PASS);

		if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(pass))
			throw new InvalidOperationException("ID or Password was not in the provided SecureStorage");

		if (aisJapan?.IsReLoginNeeded(id, pass) == false)
			return aisJapan;

		return new(id, pass);
	}

	public static Task<AISJapan> FromSecureStorageIfNeededAsync(this AISJapan aisJapan, ISecureStorage secureStorage)
		=> FromSecureStorageIfNeededAsync(secureStorage, aisJapan);
}
