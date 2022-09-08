using System;

using AirTote.SketchPad.Controls;

using Foundation;

using PencilKit;

namespace AirTote.SketchPad.NativeControls;

public class DrawingData : IDrawingData
{
	readonly PKCanvasView _canvasView;

	public DrawingData(PKCanvasView canvas)
	{
		_canvasView = canvas;
	}

	public byte[] GetBytes()
		=> _canvasView.Drawing.DataRepresentation.ToArray();

	public void FromBytes(byte[] bytes)
		=> FromData(NSData.FromArray(bytes));

	public void SaveToFile(string path)
		=> _canvasView.Drawing.DataRepresentation.Save(path, false);
	public void FromFile(string path)
		=> FromData(NSData.FromFile(path));

	void FromData(NSData data)
	{
		PKDrawing drawing = new(data, out NSError? err);
		if (err is null)
			_canvasView.Drawing = drawing;
		else
			throw new DrawingLoadFailedException(err.LocalizedFailureReason, err.LocalizedDescription);
	}
}
