using Common;

namespace _2016_Day03;

public static class Program
{
    public static void Main()
    {
        // Part 1 samples
        Console.WriteLine($"Part 1 sample: {new MaybeTriangle(5, 10, 25).IsPossible}"); // false
        Console.WriteLine($"Part 1 sample: {new MaybeTriangle(3, 4, 5).IsPossible}"); // true
        
        // Part 1
        int possibleTriangles = FileReader
            .ReadLines("input.txt")
            .Select(MaybeTriangle.FromString)
            .Count(triangle => triangle.IsPossible);
        Console.WriteLine($"Part 1: {possibleTriangles}");
        
        string part2Sample = """
              101  301  501
              102  302  502
              103  303  503
              201  401  601
              202  402  602
              203  403  603
            """;
        RowBuffer buffer = new();
        possibleTriangles = part2Sample
            .Lines()
            .Select(ParseRow)
            .SelectMany(buffer.Append)
            .Count(triangle => triangle.IsPossible);
        Console.WriteLine($"Part 2 sample: {possibleTriangles}");
        
        // Part 2
        buffer = new RowBuffer();
        possibleTriangles = FileReader
            .ReadLines("input.txt")
            .Select(ParseRow)
            .SelectMany(buffer.Append)
            .ToArray()
            .Count(triangle => triangle.IsPossible);
        Console.WriteLine($"Part 2: {possibleTriangles}");
    }

    private static IEnumerable<int> ParseRow(string row)
    {
        return row.Split(' ')
            .Where(str => str.Length > 0)
            .Select(str => str.Trim())
            .Select(int.Parse);
    }
}

internal sealed class RowBuffer
{
    public IEnumerable<MaybeTriangle> Append(IEnumerable<int> rowValues)
    {
        buffer.Add(rowValues.ToArray());

        if (buffer.Count < 3)
            yield break;
        
        yield return new MaybeTriangle(buffer[0][0], buffer[1][0], buffer[2][0]);
        yield return new MaybeTriangle(buffer[0][1], buffer[1][1], buffer[2][1]);
        yield return new MaybeTriangle(buffer[0][2], buffer[1][2], buffer[2][2]);
        
        buffer.Clear();
    }
    
    private readonly List<int[]> buffer = new(3);
}

internal readonly struct MaybeTriangle
{
    public readonly int A;
    public readonly int B;
    public readonly int C;
    
    public bool IsPossible => A + B > C && A + C > B && B + C > A;

    public static MaybeTriangle FromString(string s)
    {
        int[] numbers = s
            .Split(' ')
            .Where(str => str.Length > 0)
            .Select(str => str.Trim())
            .Select(int.Parse)
            .ToArray();
        return new MaybeTriangle(numbers[0], numbers[1], numbers[2]);
    }

    public MaybeTriangle(int a, int b, int c)
    {
        A = a;
        B = b;
        C = c;
    }
}