namespace AirTote.SketchPad.Controls;

public interface IDrawingData
{
	byte[] GetBytes();
	void SaveToFile(string path);

	void FromBytes(byte[] bytes);
	void FromFile(string path);
}
