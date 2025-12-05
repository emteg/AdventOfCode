using Common;

namespace _2025_Day04;

public static class Program
{
    public static void Main()
    {
        string sample1 = """
                         ..@@.@@@@.
                         @@@.@.@.@@
                         @@@@@.@.@@
                         @.@@@@..@.
                         @@.@@@@.@@
                         .@@@@@@@.@
                         .@.@.@.@@@
                         @.@@@.@@@@
                         .@@@@@@@@.
                         @.@.@@@.@.
                         """;
        //Console.WriteLine($"Part 1 sample: {Part1(sample1.Lines())}");
        
        //Console.WriteLine($"Part 1: {Part1(FileReader.ReadLines("input.txt"))}");
        
        //Console.WriteLine($"Part 2 sample: {Part2(sample1.Lines())}");
        
        Console.WriteLine($"Part 2 : {Part2(FileReader.ReadLines("input.txt"))}");
    }

    private static int Part1(IEnumerable<string> lines)
    {
        List<List<RollCell2D>> cells = BuildGrid(lines);
        int count = cells.SelectMany(list => list).Count(it => it.AccessibleByForklift);
        return count;
    }

    private static int Part2(IEnumerable<string> lines)
    {
        int numberOfRollsRemoved = 0;
        
        List<List<RollCell2D>> cells = BuildGrid(lines);
        int i = 0;
        
        while (true)
        {
            int rollsRemovedThisRound = 0;
            RollCell2D[] accessibleCells = cells
                .SelectMany(list => list)
                .Where(cell => cell.AccessibleByForklift)
                .ToArray();
            foreach (RollCell2D cell in accessibleCells)
            {
                cell.RollRemoved();
                numberOfRollsRemoved++;
                rollsRemovedThisRound++;
            }

            Console.WriteLine($"Round {i}: {rollsRemovedThisRound} rolls removed, {numberOfRollsRemoved} total");

            i++;
            if (rollsRemovedThisRound == 0)
                break;
        }
        
        return numberOfRollsRemoved;
    }

    private static List<List<RollCell2D>> BuildGrid(IEnumerable<string> lines)
    {
        List<List<RollCell2D>> cells = [];
        int y = 0;
        foreach (string line in lines)
        {
            int x = 0;
            int width = line.Length;
            cells.Add([]);

            foreach (char c in line)
            {
                RollCell2D cell = new(x, y, c);
                cells[y].Add(cell);
                if (x > 0)
                    cell.ConnectLeft(cells[y][x - 1]);
                if (y > 0)
                    cell.ConnectUp(cells[y - 1][x]);
                if (x > 0 && y > 0)
                    cell.ConnectUpLeft(cells[y - 1][x - 1]);
                if (y > 0 && x < width - 1)
                    cell.ConnectUpRight(cells[y - 1][x + 1]);
                x++;
            }
            y++;
        }

        return cells;
    }
}

internal sealed class RollCell2D : Cell2D
{
    public bool HasRoll { get; private set; }

    public bool AccessibleByForklift => HasRoll && AdjacentRollsOfPaper < 4;
    
    public int AdjacentRollsOfPaper => HasRoll 
        ? Neighbors
            .Cast<RollCell2D>()
            .Count(rollCell => rollCell.HasRoll)
        : 0;

    public RollCell2D(int x, int y, char c)
        : base(x, y)
    {
        HasRoll = c == '@';
    }

    public void RollRemoved()
    {
        HasRoll = false;
    }
}