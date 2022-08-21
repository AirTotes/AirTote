using AirTote.Services;
using AirTote.ViewModels.SettingPages;

namespace AirTote.Pages.Settings;

public partial class AISJapan : ContentPage
{
	public const string SEC_STORAGE_KEY_USER = AirTote.Services.AISJapan.SEC_STORAGE_KEY_USER;
	public const string SEC_STORAGE_KEY_PASS = AirTote.Services.AISJapan.SEC_STORAGE_KEY_PASS;
	public const string AIS_JAPAN_CREATE_ACCOUNT = "https://aisjapan.mlit.go.jp/ShowAccount.do";

	AISJapanViewModel ViewModel { get; } = new();
	ISecureStorage SecStorage { get; }

	public AISJapan() : this(SecureStorage.Default) { }

	public AISJapan(ISecureStorage secureStorage)
	{
		InitializeComponent();

		Content.BindingContext = ViewModel;

		SecStorage = secureStorage;
		Task.Run(LoadAccountAsync);
	}

	Task SetIsEnabledPropAsync(bool isEnabled)
		=> MainThread.InvokeOnMainThreadAsync(() => ViewModel.IsEnabled = isEnabled);

	async void CreateAccount_Tapped(object sender, EventArgs e)
		=> await Browser.OpenAsync(AIS_JAPAN_CREATE_ACCOUNT);

	async Task LoadAccountAsync()
	{
		await SetIsEnabledPropAsync(false);

		try
		{
			ViewModel.IsBusy = true;

			ViewModel.Username = await SecStorage.GetAsync(SEC_STORAGE_KEY_USER) ?? "";

			// パスワードに関しては、取得ができるかどうかのみをチェックする
			// ViewModelのPassowrdがEmptyのとき、Passwordが変更されていないことを表す。
			_ = await SecStorage.GetAsync(SEC_STORAGE_KEY_PASS);
			ViewModel.Password = "";

			await SetIsEnabledPropAsync(true);

			return;
		}
		catch (FeatureNotSupportedException ex)
		{
			MsgBox.DisplayAlert("Cannot use this Feature", "この端末では、アカウント情報の保存に対応していません。\n" + ex.Message, "OK");
		}
		catch (Exception ex)
		{
			MsgBox.DisplayAlert("Unknown Error", "アカウント情報の取得でエラーが発生しました。\n" + ex.Message, "OK");
		}
		finally
		{
			ViewModel.IsBusy = false;
		}
	}

	void RemoveAccount()
	{
		ViewModel.Username = "";
		ViewModel.Password = "";

		try
		{
			SecStorage.Remove(SEC_STORAGE_KEY_PASS);
			SecStorage.Remove(SEC_STORAGE_KEY_USER);
		}
		catch (Exception ex)
		{
			MsgBox.DisplayAlert("Unknown error", "不明なエラーが発生しました。データの消去に失敗している可能性があります。\n" + ex.Message, "OK");
		}
	}

	async Task SaveAccountAsync()
	{
		try
		{
			await SecStorage.SetAsync(SEC_STORAGE_KEY_USER, ViewModel.Username);

			if (!string.IsNullOrWhiteSpace(ViewModel.Password))
				await SecStorage.SetAsync(SEC_STORAGE_KEY_PASS, ViewModel.Password);
		}
		catch (Exception ex)
		{
			MsgBox.DisplayAlert("Unknown Error", "アカウント情報の保存でエラーが発生しました。\n" + ex.Message, "OK");
		}
	}

	static async Task<string?> TrySignInAsync(string username, string pass)
	{
		using AirTote.Services.AISJapan aisJapan = new(username, pass);

		return await aisJapan.GetSignInError();
	}

	async void LogInButtonClicked(object sender, EventArgs e)
	{
		ViewModel.Message = "";

		if (string.IsNullOrWhiteSpace(ViewModel.Username))
		{
			ViewModel.Message = "⚠️ ユーザ名を入力してください。";
			return;
		}

		string pass = ViewModel.Password;
		try
		{
			if (string.IsNullOrWhiteSpace(pass))
				pass = await SecStorage.GetAsync(SEC_STORAGE_KEY_PASS);
		}
		catch (Exception ex)
		{
			MsgBox.DisplayAlert("Password Restore Error", "パスワードの復元に失敗しました。\n" + ex.Message, "OK");
			return;
		}

		if (string.IsNullOrWhiteSpace(pass))
		{
			ViewModel.Message = "⚠️ パスワードを入力してください。";
			return;
		}

		try
		{
			ViewModel.IsBusy = true;
			await SetIsEnabledPropAsync(false);

			var result = await TrySignInAsync(ViewModel.Username, pass);

			if (string.IsNullOrWhiteSpace(result))
				ViewModel.Message = "✅ ログインに成功しました";
			else
			{
				ViewModel.Message = "⚠️ ログインに失敗しました\n" + result;
				return;
			}
		}
		finally
		{
			ViewModel.IsBusy = false;
			await SetIsEnabledPropAsync(true);
		}

		await SaveAccountAsync();
	}

	async void RemoveAccountButtonClicked(object sender, EventArgs e)
	{
		ViewModel.Message = "";

		bool isAccepted = await MsgBox.DisplayAlertAsync(
			"Remove Account from App?",
			"アカウントを除去すると、再度設定するまでAIS Japanを使用する機能を使用できなくなります。",
			"Continue Remove",
			"Cancel");

		if (!isAccepted)
			return;

		try
		{
			await SetIsEnabledPropAsync(false);
			ViewModel.IsBusy = true;
			ViewModel.Message = "";

			RemoveAccount();

			ViewModel.Message = "✅ 削除処理終了";
		}
		finally
		{
			await SetIsEnabledPropAsync(true);
			ViewModel.IsBusy = false;
		}
	}

	async void ResetButtonClicked(object sender, EventArgs e)
	{
		ViewModel.Password = "";

		try
		{
			ViewModel.Username = await SecStorage.GetAsync(SEC_STORAGE_KEY_USER) ?? "";

			await SetIsEnabledPropAsync(true);
		}
		catch (Exception ex)
		{
			MsgBox.DisplayAlert("Username restore failed", "ユーザ名の復元に失敗しました。\n" + ex.Message, "OK");
			return;
		}
	}
}
