using System.Windows.Input;

namespace AirTote.Services;

internal class OpenBrowserCommand : ICommand
{
	public static readonly OpenBrowserCommand Default = new();

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		return parameter is string;
	}
	public async void Execute(object? parameter)
	{
		if (parameter is string s)
		{
			if (!s.StartsWith("http://") && !s.StartsWith("https://"))
			{
				MsgBox.DisplayAlert("エラー", $"内部エラーです。開発者にお知らせください。\n({typeof(OpenBrowserCommand).FullName}: `{nameof(parameter)}` is not a valid url)", "OK");
				return;
			}
			try
			{
				if (!await Browser.Default.OpenAsync(s, BrowserLaunchMode.SystemPreferred))
					MsgBox.DisplayAlert("エラー", $"ブラウザを開けませんでした。\nURL: {s} ", "OK");
			}
			catch (Exception ex)
			{
				MsgBox.DisplayAlert("エラー", $"ブラウザを開けませんでした {ex.Message}", "OK");
			}
		}
		else
			MsgBox.DisplayAlert("エラー", $"内部エラーです。開発者にお知らせください。\n({typeof(OpenBrowserCommand).FullName}: `{nameof(parameter)}` is not string)", "OK");
	}
}
