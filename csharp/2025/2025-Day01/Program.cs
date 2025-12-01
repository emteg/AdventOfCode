using Common;

namespace _2025_Day01;

public static class Program
{
    public static void Main()
    {
        string sample1 = """
            L68
            L30
            R48
            L5
            R60
            L55
            L1
            L99
            R14
            L82
            """;
        Dail dail = new();
        int count = sample1
            .Lines()
            .Select(line => dail.Rotate(line))
            .Count(line => line.IsAtZero);
        Console.WriteLine($"Part 1 sample: {count}");

        dail = new Dail();
        count = FileReader
            .ReadLines("input.txt")
            .Select(line => dail.Rotate(line))
            .Count(line => line.IsAtZero);
        Console.WriteLine($"Part 1: {count}");

        dail = new Dail();
        foreach (string line in sample1.Lines()) 
            dail.Rotate2(line);
        Console.WriteLine($"Part 2 sample 1: {dail.NumberOfTimesAtZero}"); // 6
        
        dail = new Dail().Rotate2("R1000");
        Console.WriteLine($"Part 2 sample 2: {dail.NumberOfTimesAtZero}"); // 10
        
        dail = new Dail();
        foreach (string line in FileReader.ReadLines("input.txt")) 
            dail.Rotate2(line);
        Console.WriteLine($"Part 2: {dail.NumberOfTimesAtZero}");
    }
}

internal struct Dail
{
    public byte Position { get; private set; } = 50;
    public int NumberOfTimesAtZero { get; private set; } = 0;
    public bool IsAtZero => Position == 0;
    
    public Dail() {}

    public Dail Rotate(string instruction)
    {
        (int direction, int distance) = ParseInstruction(instruction);
        distance %= NumberOfSteps;
        
        if (distance == 0)
            return this;
        
        int newPosition = Position + direction * distance;
        
        if (newPosition > MaxValue)
            newPosition = newPosition - MaxValue - 1;
        else if (newPosition < MinValue)
            newPosition = MaxValue + newPosition + 1;
        
        Position = (byte)newPosition;
        
        return this;
    }

    public Dail Rotate2(string instruction)
    {
        (int direction, int distance) = ParseInstruction(instruction);

        int newPosition = Position;
        while (distance > 0)
        {
            newPosition += direction;

            newPosition = newPosition switch
            {
                > MaxValue => MinValue,
                < MinValue => MaxValue,
                _ => newPosition
            };

            if (newPosition == MinValue)
                NumberOfTimesAtZero++;
            
            distance--;
        }
        
        Position = (byte)newPosition;

        return this;
    }

    private static (int direction, int distance) ParseInstruction(string instruction)
    {
        char dir = instruction[0];
        if (dir != 'R' && dir != 'L')
            throw new InvalidOperationException($"Invalid instruction: {instruction}");

        int directionMultiplicator = dir == 'R' ? 1 : -1;
        
        int distance = int.Parse(instruction[1..]);
        if (distance < 0)
            throw new InvalidOperationException($"Invalid distance: {distance}");
        
        return (directionMultiplicator, distance);
    }
    
    private const int MaxValue = 99;
    private const int MinValue = 0;
    private const int NumberOfSteps = 100;
}