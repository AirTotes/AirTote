using System;

namespace AirTote.SketchPad.Controls;

public class DrawingLoadFailedException : Exception
{
	public string Reason { get; }
	public string Description { get; }

	public DrawingLoadFailedException(string reason, string description)
	{
		Reason = reason;
		Description = description;
	}

	public override string ToString()
		=> $"{Description} : {Reason}";
}

