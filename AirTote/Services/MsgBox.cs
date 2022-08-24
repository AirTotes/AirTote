namespace AirTote.Services;

static public class MsgBox
{
	static public void DisplayAlert(string title, string msg, string cancelBtnText)
		=> MainThread.BeginInvokeOnMainThread(() =>
			Shell.Current.CurrentPage.DisplayAlert(title, msg, cancelBtnText)
		);

	static public void DisplayAlert(string title, string msg, string acceptBtnText, string cancelBtnText, Action<bool> action, bool actionOnMainThread = false)
		=> MainThread.BeginInvokeOnMainThread(async () =>
			{
				var result = await Shell.Current.CurrentPage.DisplayAlert(title, msg, acceptBtnText, cancelBtnText);

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
			Shell.Current.CurrentPage.DisplayAlert(title, msg, cancelBtnText)
		);

	static public Task<bool> DisplayAlertAsync(string title, string msg, string acceptBtnText, string cancelBtnText)
		=> MainThread.InvokeOnMainThreadAsync(() =>
			Shell.Current.CurrentPage.DisplayAlert(title, msg, acceptBtnText, cancelBtnText)
		);
}

