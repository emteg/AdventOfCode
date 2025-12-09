using Common;

namespace _2025_Day06;

internal static class Program
{
    private static void Main()
    {
        string sample = """
            123 328  51 64 
             45 64  387 23 
              6 98  215 314
            *   +   *   +  
            """;
        ulong result = Part1New(sample.Lines());
        Console.WriteLine($"Part 1 sample: {result}");
        
        result = Part1New(GhettoAocApi.ReadLinesFromCachedPuzzleInput(2025, 6));
        Console.WriteLine($"Part 1: {result}");
        
        result = Part2(sample.Lines());
        Console.WriteLine($"Part 2 sample: {result}");
        
        result = Part2(GhettoAocApi.ReadLinesFromCachedPuzzleInput(2025, 6));
        Console.WriteLine($"Part 2: {result}");
    }

    private static ulong Part2(IEnumerable<string> input)
    {
        return ParseInput(input, false)
            .Select(problem => problem.Solve())
            .Sum();
    }

    private static ulong Part1New(IEnumerable<string> input)
    {
        return ParseInput(input, true)
            .Select(problem => problem.Solve())
            .Sum();
    }

    private static IEnumerable<Problem> ParseInput(IEnumerable<string> input, bool horizontalNumbers)
    {
        string[] lines = input.ToArray();
        int columnIndex = 0;
        int maxColumnIndex = lines.Select(line => line.Length).Max() - 1;
        while (columnIndex <= maxColumnIndex)
        {
            char operationSymbol = lines[^1][columnIndex];
            int nextOperationIndex = lines[^1].AsSpan(columnIndex + 1).IndexOfAny(['*', '+']);
            int nextColumnIndex = nextOperationIndex > 0
                ? columnIndex + nextOperationIndex
                : lines[0].Length;
            List<string> column = [];
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i];
                column.Add(line[columnIndex..nextColumnIndex]);
            }

            yield return new Problem(column.ToArray(), operationSymbol, horizontalNumbers);
            columnIndex = nextColumnIndex + 1;
        }
    }

    private static ulong Part1(IEnumerable<string> lines)
    {
        ulong grandTotal = 0;
        string[][] problems = lines
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToArray();

        for (int columnIndex = 0; columnIndex < problems[0].Length; columnIndex++)
        {
            (Func<ulong, ulong, ulong> operation, ulong problemResult) = InitializeProblemSolver(problems, columnIndex);

            for (int lineIndex = 0; lineIndex < problems.Length - 1; lineIndex++)
            {
                problemResult = operation(problemResult, ulong.Parse(problems[lineIndex][columnIndex]));
            }
            
            grandTotal += problemResult;
        }

        return grandTotal;
    }

    private static (Func<ulong, ulong, ulong> operation, ulong problemResult) InitializeProblemSolver(
        string[][] problems, int columnIndex)
    {
        string operationSymbol = problems[^1][columnIndex];
        (Func<ulong, ulong, ulong> operation, ulong problemResult) result = operationSymbol == "*"
            ? (Multiply, 1)
            : (Add, 0);
        return result;
    }

    private static ulong Multiply(ulong soFar, ulong next) => soFar * next;
    private static ulong Add(ulong soFar, ulong next) => soFar + next;
}

internal readonly struct Problem
{
    public readonly string[] RawLines;
    public readonly char Operation;
    public readonly bool UseHorizontalNumbers;
    
    public Problem(string[] rawLines, char operation, bool useHorizontalNumbers)
    {
        RawLines = rawLines;
        Operation = operation;
        UseHorizontalNumbers = useHorizontalNumbers;
    }

    public ulong Solve()
    {
        return UseHorizontalNumbers
            ? SolveWithHorizontalNumbers()
            : SolveWithVerticalNumbers();
    }

    private ulong SolveWithHorizontalNumbers()
    {
        return Solve(RawLines
            .Select(line => line.Trim())
            .Select(ulong.Parse)
        );
    }

    private ulong SolveWithVerticalNumbers()
    {
        List<ulong> numbers = [];
        int width = RawLines[0].Length - 1;
        for (int i = width; i >= 0; i--)
        {
            string numberStr = new(RawLines.Select(rawLine => rawLine[i]).ToArray());
            numbers.Add(ulong.Parse(numberStr));
        }

        return Solve(numbers);
    }

    private ulong Solve(IEnumerable<ulong> numbers)
    {
        (Func<ulong, ulong, ulong> operation, ulong result) = InitializeSolver();

        foreach (ulong number in numbers)
            result = operation(result, number);
        
        return result;
    }

    private (Func<ulong, ulong, ulong> operation, ulong initialValue) InitializeSolver()
    {
        return Operation == '+'
            ? (Add, 0)
            : (Multiply, 1);
    }
    
    private static ulong Multiply(ulong soFar, ulong next) => soFar * next;
    private static ulong Add(ulong soFar, ulong next) => soFar + next;
}