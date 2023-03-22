using System.Buffers.Text;
using System.Text.RegularExpressions;

namespace AirTote.Utils;

public record PdfDataUri(string FileName, byte[] data);

public static partial class PdfDataUriParser
{
	[GeneratedRegex(@"^data:application/pdf;filename=.{1,256}\.pdf;base64,[\w\+/=]+$")]
	static private partial Regex pdfDataUriCheckRegex();

	static public PdfDataUri Parse(string s)
	{
		if (s.Length is < 128 or >= (50 * 1000 * 1000))
			throw new ArgumentOutOfRangeException(nameof(s), "Length of `datauri` must be less than 50M");


		if (!pdfDataUriCheckRegex().IsMatch(s[0..64]))
			throw new ArgumentOutOfRangeException(nameof(s), "Invalid DataURI format");

		int fname_len = 0;
		ReadOnlySpan<char> fname_span = s.AsSpan()[30..];
		while (fname_span[fname_len] != ';')
			fname_len++;
		string fname = fname_span[..fname_len].ToString();

		int offset = 38 + fname_len;
		ReadOnlySpan<char> base64Span = s.AsSpan()[(38 + fname_len)..];
		byte[] result = Convert.FromBase64CharArray(base64Span.ToArray(), 0, base64Span.Length);

		return new PdfDataUri(fname, result);
	}
}
