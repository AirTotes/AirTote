namespace AirTote.Services;

static public class MsgBox
{
	static private Page Host => Shell.Current.CurrentPage ?? Shell.Current;

	static public void DisplayAlert(string title, string msg, string cancelBtnText)
		=> MainThread.BeginInvokeOnMainThread(() =>
			Host.DisplayAlert(title, msg, cancelBtnText)
		);

	static public void DisplayAlert(string title, string msg, string acceptBtnText, string cancelBtnText, Action<bool> action, bool actionOnMainThread = false)
		=> MainThread.BeginInvokeOnMainThread(async () =>
			{
				var result = await Host.DisplayAlert(title, msg, acceptBtnText, cancelBtnText);

				if (action is null)
					return;

				if (actionOnMainThread)
					action.Invoke(result);
				else
					_ = Task.Run(() => action.Invoke(result));
			}
		);

	static public Task DisplayAlertAsync(string title, string msg, string cancelBtnText)
		=> MainThread.InvokeOnMainThreadAsync(() =>
			Host.DisplayAlert(title, msg, cancelBtnText)
		);

	static public Task<bool> DisplayAlertAsync(string title, string msg, string acceptBtnText, string cancelBtnText)
		=> MainThread.InvokeOnMainThreadAsync(() =>
			Host.DisplayAlert(title, msg, acceptBtnText, cancelBtnText)
		);
}

