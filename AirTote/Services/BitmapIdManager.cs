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
		try
		{
			if (LockObj.TryGetValue(filePath, out Task<int>? otherTask) && otherTask is not null)
				return await otherTask;
		}
		catch (IndexOutOfRangeException)
		{
		}
		catch (KeyNotFoundException)
		{
		}

		if (BitmapRegistry.Instance.TryGetBitmapId(filePath, out int value))
			return value;

		Task<int> task = GenerateSvgBitmapIdFromMauiAssetAsync(filePath);
		LockObj[filePath] = task;
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
