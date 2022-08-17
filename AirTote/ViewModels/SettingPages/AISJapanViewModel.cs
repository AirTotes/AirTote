using AirTote.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirTote.ViewModels.SettingPages;

public partial class AISJapanViewModel : ObservableObject
{
	[ObservableProperty]
	private bool _IsEnabled = false;

	[ObservableProperty]
	private string _Email = "";

	[ObservableProperty]
	private string _Password = "";
}

