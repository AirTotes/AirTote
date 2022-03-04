using FIS_J.Models;
using FIS_J.Services;

using NUnit.Framework;

using System;
using System.Threading.Tasks;

namespace FIS_J.Tests;

public class AVWXTests
{
	static readonly ICAOCode[] codes = ICAOCodes.Codes;
	static string? API_KEY = Environment.GetEnvironmentVariable("AVWX_API_KEY");

	[Parallelizable]
	[TestCaseSource(nameof(codes))]
	public async Task METAR_Test(ICAOCode code)
	{
		AVWX avwx = new(API_KEY);
		string result = await avwx.GetSanitizedMETAR(code);
		Assert.AreEqual(code.ToString(), result.Split(' ')[0]);
	}

	[Parallelizable]
	[TestCaseSource(nameof(codes))]
	public async Task TAF_Test(ICAOCode code)
	{
		AVWX avwx = new(API_KEY);
		string result = await avwx.GetSanitizedTAF(code);
		Assert.NotNull(result);
		Assert.AreEqual(code.ToString(), result.Split(' ')[0]);
	}
}
