using Common;

namespace _2016_Day02;

public static class Program
{
    public static void Main()
    {
        // Part 1 sample input
        string[] sampleInput = """
            ULL
            RRDDD
            LURDL
            UUUUD
            """
            .Lines()
            .ToArray();
        string code = Part1(sampleInput);
        Console.WriteLine($"Part 1: {code}");
        
        // Part 1
        code = Part1(FileReader.ReadLines("part1.txt"));
        Console.WriteLine($"Part 1: {code}");
        
        // Part 2 sample input
        code = Part2(sampleInput);
        Console.WriteLine($"Part 2: {code}");
        
        // Part 2
        code = Part2(FileReader.ReadLines("part1.txt"));
        Console.WriteLine($"Part 2: {code}");
    }

    private static string Part1(IEnumerable<string> lines)
    {
        Keypad keypad = Keypad.WithNineButtons();
        string code = lines
            .Select(FromLine)
            .Select(directions => keypad.ApplyMoves(directions).CurrentKey)
            .AsString();
        return code;
    }
    
    private static string Part2(IEnumerable<string> lines)
    {
        Keypad keypad = Keypad.WithTwelveButtons();
        string code = lines
            .Select(FromLine)
            .Select(directions => keypad.ApplyMoves(directions).CurrentKey)
            .AsString();
        return code;
    }

    private static IEnumerable<Direction> FromLine(string line) => line.Select(FromChar);

    private static Direction FromChar(char c)
    {
        return c switch
        {
            'U' => Direction.Up,
            'R' => Direction.Right,
            'D' => Direction.Down,
            'L' => Direction.Left,
            _ => throw new ArgumentException($"Unknown character '{c}'")
        };
    }
}

internal struct Keypad
{
    public char CurrentKey => layout[keyIndex.y, keyIndex.x]!.Value;

    public static Keypad WithNineButtons() => new(NineButtonLayout, 3, 3, 1, 1);
    public static Keypad WithTwelveButtons() => new(TwelveButtonLayout, 5, 5, 0, 2);
    
    private Keypad(char?[,] layout, int width, int height, int startX, int startY)
    {
        this.layout = layout;
        this.width = width;
        this.height = height;
        this.keyIndex = (startX, startY);
    }

    public Keypad ApplyMoves(IEnumerable<Direction> directions)
    {
        foreach (Direction direction in directions) 
            ApplyMove(direction);

        return this;
    }

    private void ApplyMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                MoveUpIfPossible();
                break;
            case Direction.Down:
                MoveDownIfPossible();
                break;
            case Direction.Left:
                MoveLeftIfPossible();
                break;
            case Direction.Right:
                MoveRightIfPossible();
                break;
            default:
                throw new Exception($"Unknown direction {direction}");
        }
    }

    private void MoveUpIfPossible()
    {
        if (keyIndex.y == 0)
            return;

        if (keyIndex.y - 1 < 0)
            return;

        if (KeyAt(keyIndex.x, keyIndex.y - 1) is null)
            return;
        
        keyIndex.y--;
    }
    
    private void MoveDownIfPossible()
    {
        if (keyIndex.y == height - 1)
            return;

        if (keyIndex.y + 1 >= height)
            return;

        if (KeyAt(keyIndex.x, keyIndex.y + 1) is null)
            return;
        
        keyIndex.y++;
    }
    
    private void MoveLeftIfPossible()
    {
        if (keyIndex.x == 0)
            return;

        if (keyIndex.x - 1 < 0)
            return;

        if (KeyAt(keyIndex.x - 1, keyIndex.y) is null)
            return;
        
        keyIndex.x--;
    }
    
    private void MoveRightIfPossible()
    {
        if (keyIndex.x == width - 1)
            return;

        if (keyIndex.x + 1 >= width)
            return;

        if (KeyAt(keyIndex.x + 1, keyIndex.y) is null)
            return;
        
        keyIndex.x++;
    }

    private char? KeyAt(int x, int y) => layout[y, x];

    private (int x, int y) keyIndex = (0, 0);
    
    private readonly char?[,] layout;
    private readonly int width;
    private readonly int height;
    
    private static readonly char?[,] NineButtonLayout =
    {
        {'1', '2', '3'},
        {'4', '5', '6'},
        {'7', '8', '9'}
    };
    
    private static readonly char?[,] TwelveButtonLayout =
    {
        {null, null, '1', null, null},
        {null, '2' , '3', '4' , null},
        {'5' , '6' , '7', '8' , '9'},
        {null, 'A' , 'B', 'C' , null},
        {null, null, 'D', null, null},
    };
}