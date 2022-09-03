using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AirTote.Services.Types;

public class LowerATSRoute
{
	public PointInfo[] PointList { get; }
	public Dictionary<string, RouteInfo[]> RouteDict { get; }

	[JsonIgnore]
	public Dictionary<string, PointInfo> PointDict { get; }

	[JsonConstructor]
	public LowerATSRoute(PointInfo[] PointList, Dictionary<string, RouteInfo[]> RouteDict)
	{
		if (PointList is null)
			throw new ArgumentNullException(nameof(PointList));

		if (RouteDict is null)
			throw new ArgumentNullException(nameof(RouteDict));

		this.PointList = PointList;
		this.PointDict = PointList.ToDictionary(v => v.Name);
		this.RouteDict = RouteDict;

		foreach (var lineList in RouteDict)
		{
			foreach (var line in lineList.Value)
			{
				if (!string.IsNullOrWhiteSpace(line.PreviousPointName)
					&& line.PreviousPoint is null
					&& this.PointDict.TryGetValue(line.PreviousPointName, out PointInfo? previousPt) && previousPt is not null)
				{
					line.PreviousPoint = previousPt;

					if (lineList.Value.FirstOrDefault(v => v.NextPointName == line.PreviousPointName) is RouteInfo previousLine)
					{
						previousLine.NextPoint = previousPt;
						previousLine.NextRoute = line;
					}
				}

				if (!string.IsNullOrWhiteSpace(line.NextPointName)
					&& line.PreviousPoint is null
					&& this.PointDict.TryGetValue(line.NextPointName, out PointInfo? nextPt) && nextPt is not null)
				{
					line.NextPoint = nextPt;

					if (lineList.Value.FirstOrDefault(v => v.PreviousPointName == line.NextPointName) is RouteInfo nextLine)
					{
						nextLine.NextPoint = nextPt;
						nextLine.NextRoute = line;
					}
				}

				if (string.IsNullOrWhiteSpace(line.Name))
					line.Name = lineList.Key;
			}
		}
	}
}
