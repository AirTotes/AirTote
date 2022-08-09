namespace AirTote.Components.FlyoutMenu;

public partial class FlyoutMenuView : ContentView
{
	public event EventHandler? PageChangeRequested;

	public FlyoutMenuView()
	{
		InitializeComponent();

		PageListView.SetBinding(CollectionView.ItemsSourceProperty, new Binding()
		{
			Source = Shell.Current,
			Path = "Items",
			Mode = BindingMode.OneWay,
		});

		PageListView.SelectionChanged += async (_, e) =>
		{
			if (e.CurrentSelection.Count <= 0 || e.CurrentSelection[0] is not ShellItem item)
				return;

			await (Shell.Current as IShellController).OnFlyoutItemSelectedAsync(item);
			PageChangeRequested?.Invoke(this, new());
		};
	}
}
