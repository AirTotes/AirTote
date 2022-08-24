using System;
using System.Windows.Input;

using Microsoft.Maui.Controls;

namespace AirTote.TwoPaneView;

public class ChangeRightPaneViewCommand : ICommand
{
	public event EventHandler? CanExecuteChanged;
	public TwoPaneView Target { get; }

	public ChangeRightPaneViewCommand(TwoPaneView target)
	{
		Target = target;
	}

	public bool CanExecute(object? parameter)
		=> parameter switch
		{
			Type t => t.IsSubclassOf(typeof(View)) || t.IsSubclassOf(typeof(ContentPage)),
			ViewProps vp => vp.Content is not null,
			_ => false
		};

	public async void Execute(object? parameter)
	{
		string title = "";
		View? view = null;

		switch (parameter)
		{
			case Type t:
				if (t.IsSubclassOf(typeof(View)))
					view = t.GetConstructor(Array.Empty<Type>())?.Invoke(null) as View;
				else if (t.IsSubclassOf(typeof(ContentPage))
					&& t.GetConstructor(Array.Empty<Type>())?.Invoke(null) is ContentPage p)
				{
					view = p.Content;
					title = p.Title;
				}
				break;

			case ViewProps vp:
				view = vp.Content;
				title = vp.Title;
				break;
		}

		Target.RightPaneContent = view;
		await Target.NotifyRightPaneContentChanged(title);
	}
}
