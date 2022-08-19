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

	async void CreateAccount_Tapped(object sender, EventArgs e)
		=> await Browser.OpenAsync(AIS_JAPAN_CREATE_ACCOUNT);

	async Task LoadAccountAsync()
	{
		ViewModel.IsEnabled = false;

		try
		{
			ViewModel.Username = await SecStorage.GetAsync(SEC_STORAGE_KEY_USER) ?? "";

			// パスワードに関しては、取得ができるかどうかのみをチェックする
			// ViewModelのPassowrdがEmptyのとき、Passwordが変更されていないことを表す。
			_ = await SecStorage.GetAsync(SEC_STORAGE_KEY_PASS);
			ViewModel.Password = "";

			ViewModel.IsEnabled = true;
		}
		catch (FeatureNotSupportedException ex)
		{
			await Shell.Current.CurrentPage.DisplayAlert("Cannot use this Feature", "この端末では、アカウント情報の保存に対応していません。\n" + ex.Message, "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.CurrentPage.DisplayAlert("Unknown Error", "アカウント情報の取得でエラーが発生しました。\n" + ex.Message, "OK");
		}
	}

	async Task RemoveAccountAsync()
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
			await Shell.Current.CurrentPage.DisplayAlert("Unknown error", "不明なエラーが発生しました。データの消去に失敗している可能性があります。\n" + ex.Message, "OK");
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
			await Shell.Current.CurrentPage.DisplayAlert("Unknown Error", "アカウント情報の保存でエラーが発生しました。\n" + ex.Message, "OK");
		}
	}

	static async Task<string?> TrySignInAsync(string username, string pass)
	{
		using AirTote.Services.AISJapan aisJapan = new(username, pass);

		return await aisJapan.GetSignInError();
	}

	async void LogInButtonClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(ViewModel.Username))
		{
			await Shell.Current.CurrentPage.DisplayAlert("Username Empty", "ユーザ名を入力してください。", "OK");
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
			await Shell.Current.CurrentPage.DisplayAlert("Password Restore Error", "パスワードの復元に失敗しました。", "OK");
			return;
		}

		if (string.IsNullOrWhiteSpace(pass))
		{
			await Shell.Current.CurrentPage.DisplayAlert("Password Empty", "パスワードを入力してください。", "OK");
			return;
		}

		try
		{
			ViewModel.IsBusy = true;
			ViewModel.IsEnabled = false;

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
			ViewModel.IsEnabled = true;
		}

		await SaveAccountAsync();
	}

	async void RemoveAccountButtonClicked(object sender, EventArgs e)
	{
		ViewModel.Message = "";

		bool isAccesped = await Shell.Current.CurrentPage.DisplayAlert(
			"Remove Account from App?",
			"アカウントを除去すると、再度設定するまでAIS Japanを使用する機能を使用できなくなります。",
			"Continue Remove",
			"Cancel");

		if (!isAccesped)
			return;

		try
		{
			ViewModel.IsEnabled = false;
			ViewModel.IsBusy = true;
			ViewModel.Message = "";
			await RemoveAccountAsync();
			ViewModel.Message = "✅ 削除処理終了";
		}
		finally
		{
			ViewModel.IsEnabled = true;
			ViewModel.IsBusy = false;
		}
	}

	async void ResetButtonClicked(object sender, EventArgs e)
	{
		ViewModel.Password = "";

		try
		{
			ViewModel.Username = await SecStorage.GetAsync(SEC_STORAGE_KEY_USER) ?? "";
		}
		catch (Exception ex)
		{
			await Shell.Current.CurrentPage.DisplayAlert("Username restore failed", "ユーザ名の復元に失敗しました。\n" + ex.Message, "OK");
			return;
		}
	}
}
