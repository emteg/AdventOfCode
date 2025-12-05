namespace _2025_Day02;

public static class Program
{
    public static void Main()
    {
        string sample1 = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

        List<Range> ranges = [];
        ReadOnlySpan<char> span = sample1.AsSpan();
        foreach (Range range in span.Split(','))
        {
            
        }
        while (span.Length > 0)
        {
            span = ReadRanges(span, ranges);
        }
    }

    private static ReadOnlySpan<char> ReadRanges(ReadOnlySpan<char> span, List<Range> ranges)
    {
        bool containsMultipleRanges = span.Contains(',');
        
        ReadOnlySpan<char> idRange = containsMultipleRanges ? span[..span.IndexOf(',')] : span;
        
        int firstId = int.Parse(idRange[..idRange.IndexOf('-')].ToString());
        int lastId = int.Parse(idRange[(idRange.IndexOf('-') + 1)..].ToString());
        Range range = new(firstId, lastId);
        
        ranges.Add(range);
        
        return containsMultipleRanges ? span[(span.IndexOf(',') + 1)..] : ReadOnlySpan<char>.Empty;
    }
}
