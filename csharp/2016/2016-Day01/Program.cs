using Common;

namespace _2016_Day01;

public static class Program
{
    public static void Main()
    {
        // Part 1 sample inputs
        _ = new Position().Apply("R2, L3").Expect(2, 3, 5);
        _ = new Position().Apply("R2, R2, R2").Expect(0, -2, 2);
        _ = new Position().Apply("R5, L5, R5, R3").Expect(12);

        // Part 1
        Position part1 = new Position().Apply(FileReader.ReadLines("part1.txt").First());
        Console.WriteLine($"Part 1: {part1.DistanceFromStart}");
        
        // Part 2 sample inputs
        MovementInterceptor movementInterceptor = new();
        _ = new Position(movementInterceptor).Apply("R8, R4, R4, R8");
        movementInterceptor.Expect(4, 0);
        
        // Part 2
        movementInterceptor = new MovementInterceptor();
        _ = new Position(movementInterceptor).Apply(FileReader.ReadLines("part1.txt").First());
        Console.WriteLine($"Part 2: {movementInterceptor.DistanceFromStartOfFirstLocationVisitedTwice}");
    }

    extension(Position pos)
    {
        private Position Apply(string s) => pos.Apply(s.Split(", ").Select(Instruction.FromString));
        
        private Position Expect(int x, int y, int distance)
        {
            return pos.X != x || pos.Y != y || pos.DistanceFromStart != distance
                ? throw new Exception($"Expected to be at ({x},  {y}) with a distance of {pos.DistanceFromStart}, but is at ({pos.X}, {pos.Y}) with a distance of {distance}")
                : pos;
        }

        private Position Expect(int distance)
        {
            return pos.DistanceFromStart != distance 
                ? throw new Exception($"Expected to be at a distance of {distance}, but has an actual distance of {pos.DistanceFromStart}") 
                : pos;
        }
    }
}

internal class MovementInterceptor
{
    public int? DistanceFromStartOfFirstLocationVisitedTwice
    {
        get
        {
            if (firstLocationVisitedTwice is null)
                return null;
            
            return Math.Abs(firstLocationVisitedTwice.Value.X) +  Math.Abs(firstLocationVisitedTwice.Value.Y);
        }
    }

    public void Intercept(Position pos, int xDir, int yDir, int distance)
    {
        int x = pos.X;
        int y = pos.Y;
        while (distance > 0)
        {
            x += xDir;
            y += yDir;
            distance--;
            
            if (!visitedPositions.Add((x, y)) && firstLocationVisitedTwice is null)
                firstLocationVisitedTwice = (x, y);
        }
    }

    public void Expect(int x, int y)
    {
        if (firstLocationVisitedTwice is null)
            throw new Exception("No location was visited twice!");

        if (firstLocationVisitedTwice.Value.X != x || firstLocationVisitedTwice.Value.Y != y)
            throw new Exception(
                $"Expected first location visited twice to be at ({x},  {y}), but is at ({firstLocationVisitedTwice.Value.X}, {firstLocationVisitedTwice.Value.Y})");
    }

    private (int X, int Y)? firstLocationVisitedTwice;
    private readonly HashSet<(int, int)> visitedPositions = [(0, 0)];
}

internal readonly struct Instruction
{
    public readonly Turn Turn;
    public readonly int Distance;

    public static Instruction FromString(string s)
    {
        if (s.Length < 2)
            throw new ArgumentException($"Expected the string be at least 2 characters long (actual: {s.Length})");

        Turn turn = s[0] switch
        {
            'L' => Turn.Left,
            'R' => Turn.Right,
            _ => throw new ArgumentException($"Expected the string to start with either 'R' or 'L' (actual: {s[0]})")
        };

        if (!int.TryParse(s[1..], out int distance))
            throw new ArgumentException(
                $"Couldn't parse the distance as an int after the first character: '{s[1..]}')");

        return new Instruction(turn, distance);
    }

    public override string ToString() => $"{Turn}: {Distance}";

    private Instruction(Turn turn, int distance)
    {
        Turn = turn;
        Distance = distance;
    }
}

internal enum Turn
{
    Left = -1,
    Right = 1
}

internal struct Position
{
    public Direction Direction { get; private set; } = Direction.North;
    public int X { get; private set; } = 0;
    public int Y { get; private set; } = 0;
    public int DistanceFromStart => Math.Abs(X) + Math.Abs(Y);

    public Position(MovementInterceptor? interceptor = null)
    {
        this.interceptor = interceptor;
    }
    
    public Position Apply(IEnumerable<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
            Apply(instruction);

        return this;
    }

    private void Apply(Instruction instruction)
    {
        Direction = (Direction, instruction.Turn) switch
        {
            (Direction.North, Turn.Left) => Direction.West,
            (Direction.West, Turn.Right) => Direction.North,
            (_, _) => (Direction)((int)Direction + (int)instruction.Turn),
        };

        int xMultiplyer = Direction switch
        {
            Direction.East => 1,
            Direction.West => -1,
            _ => 0
        };
        int yMultiplyer = Direction switch
        {
            Direction.North => 1,
            Direction.South => -1,
            _ => 0
        };
        
        interceptor?.Intercept(this, xMultiplyer, yMultiplyer, instruction.Distance);
        
        X += instruction.Distance * xMultiplyer;
        Y += instruction.Distance * yMultiplyer;
    }

    private readonly MovementInterceptor? interceptor = null;
}

internal enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}