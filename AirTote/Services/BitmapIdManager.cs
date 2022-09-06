using Mapsui.Styles;
using Mapsui.Utilities;

using SkiaSharp;

using System.Collections.Concurrent;
using System.Threading;

namespace AirTote.Services;

public static class BitmapIdManager
{
	static ConcurrentDictionary<string, Task<int>> LockObj { get; } = new();

	static public async Task<int> GetSvgFromMauiAssetAsync(string filePath)
	{
		if (LockObj.TryGetValue(filePath, out Task<int>? otherTask) && otherTask is not null)
			return await otherTask;

		if (BitmapRegistry.Instance.TryGetBitmapId(filePath, out int value))
			return value;

		LockObj[filePath] = GenerateSvgBitmapIdFromMauiAssetAsync(filePath);

		Task<int> task = LockObj[filePath];
		int id = await task;
		_ = LockObj.TryRemove(filePath, out var _);

		return id;
	}

	static public async Task<int> GenerateSvgBitmapIdFromMauiAssetAsync(string filePath)
	{
		using Stream stream = await FileSystem.OpenAppPackageFileAsync(filePath);

		SKPicture pict = SvgHelper.LoadSvgPicture(stream);

		return BitmapRegistry.Instance.Register(pict, filePath);
	}
}
