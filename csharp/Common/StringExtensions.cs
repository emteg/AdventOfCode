namespace Common;

public static class StringExtensions
{
    public static IEnumerable<char> CharsAtEvenIndices(this string s) => s.Where((_, i) => i % 2 == 0);
    public static IEnumerable<char> CharsAddOddIndices(this string s) => s.Where((_, i) => i % 2 != 0);
    public static string AsString(this IEnumerable<char> chars) => new(chars.ToArray());

    public static IEnumerable<string> Lines(this string s)
    {
        TextReader reader = new StringReader(s);
        while (true)
        {
            string? line = reader.ReadLine();
            if (line is null)
                yield break;
            yield return line;
        }
    }
}