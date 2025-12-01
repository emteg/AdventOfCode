using Common;

namespace _2016_Day06;

public static class Program
{
    public static void Main()
    {
        // Part 1 sample
        string sample = """
            eedadn
            drvtee
            eandsr
            raavrd
            atevrs
            tsrnev
            sdttsa
            rasrtv
            nssdts
            ntnada
            svetve
            tesnvt
            vntsnd
            vrdear
            dvrsen
            enarar
            """;
        Console.WriteLine(CharCountsPerColumn(sample)
            .Select(MostCommonCharPerColumn)
            .AsString());
        
        // Part 1
        string input = File.ReadAllText("input.txt");
        Console.WriteLine(CharCountsPerColumn(input)
            .Select(MostCommonCharPerColumn)
            .AsString());
        
        // Part 2 sample
        Console.WriteLine(CharCountsPerColumn(sample)
            .Select(LeastCommonCharPerColumn)
            .AsString());
        
        // Part 2
        Console.WriteLine(CharCountsPerColumn(input)
            .Select(LeastCommonCharPerColumn)
            .AsString());
    }

    private static Dictionary<int, Dictionary<char, int>> CharCountsPerColumn(string s)
    {
        Dictionary<int, Dictionary<char, int>> result = [];
        
        foreach (ReadOnlySpan<char> line in s.EnumerateLines())
        {
            for (int columnIndex = 0; columnIndex < line.Length; columnIndex++)
            {
                if (!result.TryGetValue(columnIndex, out Dictionary<char, int>? columnDict))
                {
                    columnDict = new Dictionary<char, int>();
                    result.Add(columnIndex, columnDict);
                }

                char c = line[columnIndex];

                if (columnDict.TryGetValue(c, out int count))
                    columnDict[c] = count + 1;
                else
                    columnDict.Add(c, 1);
            }
        }

        return result;
    }

    private static char MostCommonCharPerColumn(KeyValuePair<int, Dictionary<char, int>> columnDict) 
        => columnDict.Value.OrderByDescending(pair => pair.Value).First().Key;
    
    private static char LeastCommonCharPerColumn(KeyValuePair<int, Dictionary<char, int>> columnDict)
        => columnDict.Value.OrderBy(pair => pair.Value).First().Key;
}