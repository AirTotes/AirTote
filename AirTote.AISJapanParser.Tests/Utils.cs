using System;
namespace AirTote.AISJapanParser.Tests;

public static class Utils
{
	public static Task<string> ReadAllTextFromTestCaseFilesAsync(params string[] path_segs)
		=> File.ReadAllTextAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestCaseFiles", Path.Combine(path_segs)));
}

