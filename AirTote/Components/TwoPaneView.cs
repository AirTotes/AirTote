using DependencyPropertyGenerator;

namespace AirTote.Components;

[DependencyProperty<View>("LeftPaneContent")]
[DependencyProperty<View>("RightPaneContent")]
[DependencyProperty<double>("MinimumTwoPaneWidth", DefaultValue = 300)]
[DependencyProperty<GridLength>("LeftPaneWidth", TypeConverter = typeof(GridLengthTypeConverter))]
[DependencyProperty<GridLength>("RightPaneWidth", TypeConverter = typeof(GridLengthTypeConverter))]
[DependencyProperty<bool>("IsTwoPane", IsReadOnly = true, DefaultValue = true)]
public partial class TwoPaneView : ContentView
{
	Grid BaseGrid { get; } = new();
	ColumnDefinitionCollection BaseGridColumnDefinitions { get; } = new();

	ContentPage RightViewPage { get; } = new();
	ContentView LeftView { get; } = new();
	ContentView RightView { get; } = new();
	Binding BindingToThis(string name)
		=> new()
		{
			Source = this,
			Mode = BindingMode.OneWay,
			Path = name
		};

	public TwoPaneView()
	{
		Content = BaseGrid;

		#region Set Column(Pane) Settings
		ColumnDefinition LeftPaneColumnDefinition = new();
		LeftPaneColumnDefinition.SetBinding(
			ColumnDefinition.WidthProperty,
			BindingToThis(nameof(LeftPaneWidth))
		);
		ColumnDefinition RightPaneColumnDefinition = new();
		RightPaneColumnDefinition.SetBinding(
			ColumnDefinition.WidthProperty,
			BindingToThis(nameof(RightPaneWidth))
		);

		BaseGridColumnDefinitions.Add(LeftPaneColumnDefinition);
		BaseGridColumnDefinitions.Add(RightPaneColumnDefinition);

		BaseGrid.ColumnDefinitions = BaseGridColumnDefinitions;

		Grid.SetColumn(LeftView, 0);
		Grid.SetColumn(RightView, 1);

		SizeChanged += TwoPaneView_SizeChanged;
		#endregion

		#region Content Settings
		LeftView.SetBinding(
			ContentView.ContentProperty,
			BindingToThis(nameof(LeftPaneContent))
		);
		RightView.SetBinding(
			ContentView.ContentProperty,
			BindingToThis(nameof(RightPaneContent))
		);
		RightView.SetBinding(
			ContentView.IsVisibleProperty,
			BindingToThis(nameof(IsTwoPane))
		);

		BaseGrid.Children.Add(LeftView);
		BaseGrid.Children.Add(RightView);
		#endregion
	}

	private void TwoPaneView_SizeChanged(object? sender, EventArgs e)
		=> IsTwoPane = MinimumTwoPaneWidth <= Width;

	partial void OnIsTwoPaneChanged(bool newValue)
	{
		RightView.Content = null;
		RightViewPage.Content = null;
		Grid.SetColumnSpan(LeftView, newValue ? 1 : 2);

		if (newValue)
		{
			RightViewPage.RemoveBinding(ContentPage.ContentProperty);

			RightView.SetBinding(
				ContentView.ContentProperty,
				BindingToThis(nameof(RightPaneContent))
			);
		}
		else
		{
			RightView.RemoveBinding(ContentPage.ContentProperty);

			RightViewPage.SetBinding(
				ContentPage.ContentProperty,
				BindingToThis(nameof(RightPaneContent))
			);
		}
	}

	/// <summary>
	/// <see cref="RightPane"/>の表示内容が変化した際に呼び出す関数です。
	/// OnePaneモード時に呼び出すことにより、RightPaneの内容が含まれたページに移動します。
	/// TwoPaneモード時は何も処理を行いません。
	/// </summary>
	public async Task NotifyRightPaneContentChanged(string pageTitle = "")
	{
		if (IsTwoPane)
			return;

		RightViewPage.Title = pageTitle;

		if (Navigation.NavigationStack.Last() != RightViewPage)
			await Navigation.PushAsync(RightViewPage);
	}
}
