namespace AirTote.Services;

public static class PreferenceManager
{
	public enum Keys
	{
		TopPage_EnableLocationService,
		TopPage_EnableLocationFollowAnimation,
		TopPage_LocationRefleshInterval,
	}

	static public void Set<T>(Keys key, T value)
		=> Preferences.Default.Set(key.ToString(), value);

	static public bool TryGet<T>(Keys key, ref T? value)
	{
		if (!Preferences.Default.ContainsKey(key.ToString()))
			return false;

		Preferences.Default.Get(key.ToString(), value);
		return true;
	}

	static public T? Get<T>(Keys key)
		=> Preferences.Default.Get(key.ToString(), default(T));

	static public void Remove(Keys key)
		=> Preferences.Default.Remove(key.ToString());
}
