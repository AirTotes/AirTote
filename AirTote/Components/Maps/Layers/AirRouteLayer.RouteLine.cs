using AirTote.Services.Types;

using Mapsui.Nts;
using Mapsui.Styles;

using NetTopologySuite.Geometries;

using SkiaSharp;

using Topten.RichTextKit;

using IStyle = Topten.RichTextKit.IStyle;
using Style = Topten.RichTextKit.Style;

namespace AirTote.Components.Maps.Layers;

public partial class AirRouteLayer
{
	private class RouteLine : GeometryFeature
	{
		public static IStyle DefaultTextStyle { get; } = new Style()
		{
			BackgroundColor = SKColor.Empty,
			FontSize = 14,
			TextColor = SKColors.Gray,
		};

		CalloutStyle CalloutStyle { get; } = new()
		{
			RectRadius = 10,
			ShadowWidth = 4,
			Enabled = false,
			MaxWidth = 400,
		};

		VectorStyle LineStyle { get; } = new()
		{
			Line = new()
			{
				Color = Mapsui.Styles.Color.FromArgb(0xFF, 0x80, 0x50, 0x40),
				Width = 2
			}
		};

		RichString? CalloutText { get; set; }

		public RouteInfo RouteInfo { get; }

		public RouteLine(RouteInfo routeInfo)
		{
			RouteInfo = routeInfo;

			Styles.Add(LineStyle);
			Styles.Add(CalloutStyle);

			Task.Run(SetCalloutTextToCallout);
			Task.Run(SetLine);
		}

		void SetCalloutTextToCallout()
		{
			static string ShowNoneIfNullOrWhiteSpace(string? str)
				=> string.IsNullOrWhiteSpace(str) ? "(None)" : str;

			CalloutText = new();

			CalloutText.Add(
				$"Route Name: {RouteInfo.Name ?? "(Error! Unknown)"}\n",
				fontSize: 18,
				fontWeight: 700,
				textColor: SKColors.Black
			);

			if (!string.IsNullOrWhiteSpace(RouteInfo.PreviousRoute?.RemarksControllingUnitFrequency)
				&& RouteInfo.RemarksControllingUnitFrequency != RouteInfo.PreviousRoute.RemarksControllingUnitFrequency)
			{
				CalloutText.Paragraph().Add(
					"\nPrevious Remarks / Controlling Unit / Frequency:\n"
					+ $"    {ShowNoneIfNullOrWhiteSpace(RouteInfo.PreviousRoute.RemarksControllingUnitFrequency)}");
			}

			CalloutText.Paragraph().Add($"Previous Point: {ShowNoneIfNullOrWhiteSpace(RouteInfo.PreviousPointName)}\n");

			CalloutText.Paragraph().Add($"Tracking Magnetic Course: {ShowNoneIfNullOrWhiteSpace(RouteInfo.TrackingMagneticCourse)}");
			CalloutText.Paragraph().Add($"Geometrical Course: {ShowNoneIfNullOrWhiteSpace(RouteInfo.GeometricalCourse)}");
			CalloutText.Paragraph().Add($"Distance [NM]: {ShowNoneIfNullOrWhiteSpace(RouteInfo.Distance_NM)}");
			CalloutText.Paragraph().Add($"Change Over Point: {ShowNoneIfNullOrWhiteSpace(RouteInfo.ChangeOverPoint).Replace('\n', ' ').Replace("----", "/")}");
			CalloutText.Paragraph().Add($"Minimum Enroute Altitude: {ShowNoneIfNullOrWhiteSpace(RouteInfo.MinimumEnrouteAltitude).Replace('\n', '/')}");

			string lateralLimits = ShowNoneIfNullOrWhiteSpace(RouteInfo.LateralLimits).Replace("\t", "  ").Replace("\n", "\n  ");
			CalloutText.Paragraph().Add($"Lateral Limits:\n  {lateralLimits}");

			CalloutText.Paragraph().Add(
				"Direction of Cruising Levels (Odd / Even):\n"
				+ $"    {ShowNoneIfNullOrWhiteSpace(RouteInfo.DirectionOfCruisingLevels_Odd)} / {ShowNoneIfNullOrWhiteSpace(RouteInfo.DirectionOfCruisingLevels_Even)}");
			CalloutText.Paragraph().Add($"Remarks / Controlling Unit / Frequency:\n    {ShowNoneIfNullOrWhiteSpace(RouteInfo.RemarksControllingUnitFrequency)}");

			CalloutText.Paragraph().Add($"\nNext Point: {ShowNoneIfNullOrWhiteSpace(RouteInfo.NextPointName)}");

			if (!string.IsNullOrWhiteSpace(RouteInfo.NextRoute?.RemarksControllingUnitFrequency)
				&& RouteInfo.RemarksControllingUnitFrequency != RouteInfo.NextRoute.RemarksControllingUnitFrequency)
			{
				CalloutText.Paragraph().Add(
					"Next Remarks / Controlling Unit / Frequency:\n"
					+ $"    {ShowNoneIfNullOrWhiteSpace(RouteInfo.NextRoute.RemarksControllingUnitFrequency)}");
			}

			Utils.SetCalloutText(this.CalloutStyle, this.CalloutText);
		}

		void SetLine()
		{
			if (RouteInfo.PreviousPoint is null || RouteInfo.NextPoint is null)
				return;

			// Previousを基準にNext (あるいはNextRoute.Next) までLineを引く
			// そのため、Previousを参照できないのであればこのLineは引かない
			if (RouteInfo.PreviousPoint.ToCoordinate() is not Coordinate previousPoint)
				return;

			// Nextが存在しない場合 (距離依存の点など) には、Nextの位置が定まるまで繰り返し続ける
			Coordinate? nextPoint = RouteInfo.NextPoint.ToCoordinate();
			RouteInfo? nextRoute = RouteInfo.NextRoute;
			while (nextPoint is null && nextRoute is not null)
			{
				nextPoint = nextRoute.NextPoint?.ToCoordinate();
				nextRoute = nextRoute.NextRoute;
			}

			// 座標が定まったNextが最後まで見つからなかった場合。
			// 通常は終点が明示的に指定されているため、ここまで来ない
			if (nextPoint is null)
				return;

			Geometry = new LineSegment(previousPoint, nextPoint).ToGeometry(GeometryFactory.Default);
		}
	}
}

