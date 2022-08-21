using System;

using CommunityToolkit.Maui.Views;

namespace AirTote.Components.FlyoutMenu;

public class FlyoutMenuPopup : Popup
{
	FlyoutMenuView MenuView { get; }

	public FlyoutMenuPopup()
	{
		MenuView = new();

		Content = MenuView;

		MenuView.PageChangeRequested += (_, _) => this.Close();

		HorizontalOptions = VerticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Start;
	}
}

