using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AirTote.Components;

public class TableView : Microsoft.Maui.Controls.TableView
{
	const float Alpha = 0.8f;

	public TableView()
	{
		Root.CollectionChanged += Root_CollectionChanged;
		SetTextColors(Root);
	}

	protected override void OnModelChanged()
	{
		base.OnModelChanged();
		Root.CollectionChanged += Root_CollectionChanged;
		SetTextColors(Root);
	}

	private static void Root_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		=> SetTextColors(e.NewItems);
	private static void TableSection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		=> SetTextColors(e.NewItems);

	static void SetTextColors(System.Collections.IEnumerable? e)
	{
		Console.WriteLine("\t" + nameof(SetTextColors));
		if (e is null)
			return;

		foreach (var v in e)
		{
			switch (v)
			{
				case TableSection s:
					s.SetAppThemeColor(TableSectionBase.TextColorProperty, App.LightSecondaryColor, App.DarkSecondaryColor);
					s.CollectionChanged += TableSection_CollectionChanged;
					SetTextColors(s);
					break;

				case TextCell c:
					c.SetAppThemeColor(
						TextCell.TextColorProperty,
						App.LightSecondaryColor,
						App.DarkSecondaryColor
					);
					c.SetAppThemeColor(
						TextCell.DetailColorProperty,
						App.LightSecondaryColor.WithAlpha(Alpha),
						App.DarkSecondaryColor.WithAlpha(Alpha)
					);
					break;

				case EntryCell c:
					c.SetAppThemeColor(
						EntryCell.LabelColorProperty,
						App.LightSecondaryColor,
						App.DarkSecondaryColor
					);
					break;
			}
		}
	}
}
