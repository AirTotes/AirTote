using System.Windows.Input;

namespace AirTote.Services;

internal class OpenBrowserCommand : ICommand
{
	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		return parameter is not null;
	}
	public async void Execute(object? parameter)
	{
		if (parameter is string s)
		{
			try
			{
				await Browser.Default.OpenAsync(s, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("エラー", $"ブラウザを開けませんでした {ex.Message}", "OK");
			}
		}
	}
}
