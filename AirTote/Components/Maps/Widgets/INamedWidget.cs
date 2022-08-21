using Mapsui.Widgets;

namespace AirTote.Components.Maps.Widgets;

public interface INamedWidget : IWidget
{
	string Name { get; }
}

