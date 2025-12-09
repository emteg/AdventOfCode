using Common;

namespace _2025_Day07;

internal static class Program
{
    private static void Main()
    {
        string sample ="""
            .......S.......
            ...............
            .......^.......
            ...............
            ......^.^......
            ...............
            .....^.^.^.....
            ...............
            ....^.^...^....
            ...............
            ...^.^...^.^...
            ...............
            ..^...^.....^..
            ...............
            .^.^.^.^.^...^.
            ...............
            """;
        Console.WriteLine($"Part 1 sample: {Execute(ReadGrid(sample.Lines()))} splits/paths");
        
        Console.WriteLine($"Part 1: {Execute(ReadGrid(GhettoAocApi.ReadLinesFromCachedPuzzleInput(2025, 7)))} splits/paths");
    }

    private static (int splitCount, ulong totalPaths) Execute(List<List<TachyonCell>> grid)
    {
        int splitCount = 0;
        ulong totalPaths = 0;
        foreach (List<TachyonCell> row in grid)
        {
            foreach (TachyonCell cell in row)
            {
                if (!cell.IsStart && !cell.HasBeam)
                    continue;

                if (cell.IsStart)
                {
                    ((TachyonCell)cell.Down!).AddBeam();
                    continue;
                }

                if (cell.HasBeam && cell.Down is not null && ((TachyonCell)cell.Down).IsSplitter)
                {
                    splitCount++;
                    TachyonCell downLeft = (TachyonCell)cell.DownLeft!;
                    
                    if (!downLeft.IsSplitter)
                        downLeft.AddBeam(cell.BeamCount);
                    
                    TachyonCell downRight = (TachyonCell)cell.DownRight!;
                    if (!downRight.IsSplitter)
                        downRight.AddBeam(cell.BeamCount);
                    
                    continue;
                }

                if (cell.HasBeam && cell.Down is not null)
                {
                    ((TachyonCell)cell.Down).AddBeam(cell.BeamCount);
                    continue;
                }
            }
        }
        
        totalPaths = grid[^1].Select(cell => cell.BeamCount).Sum();

        return (splitCount, totalPaths);
    }

    private static void PrintGrid(List<List<TachyonCell>> grid)
    {
        Console.Clear();
        foreach (List<TachyonCell> row in grid)
        {
            foreach (TachyonCell cell in row) 
                Console.Write(cell.ToString());
            
            Console.WriteLine();
        }
    }

    private static List<List<TachyonCell>> ReadGrid(IEnumerable<string> lines)
    {
        List<List<TachyonCell>> grid = [];
        int y = 0;
        
        foreach (string line in lines)
        {
            int x = 0;
            int width = line.Length;
            grid.Add([]);
            
            foreach (char c in line)
            {
                TachyonCell cell = new(c, x, y);
                grid[y].Add(cell);
                
                if (y > 0)
                    cell.ConnectUp(grid[y - 1][x]);
                
                if (x > 0)
                    cell.ConnectLeft(grid[y][x - 1]);
                
                if (x > 0 && y > 0)
                    cell.ConnectUpLeft(grid[y - 1][x - 1]);
                
                if (y > 0 && x < width - 1)
                    cell.ConnectUpRight(grid[y - 1][x + 1]);

                x++;
            }
            y++;
        }

        return grid;
    }
}

internal sealed class TachyonCell : Cell2D
{
    public bool IsStart { get; }
    public bool IsSplitter { get; }
    public bool HasBeam { get; private set; }
    public ulong BeamCount { get; private set; }

    public TachyonCell(char c, int x, int y) : base(x, y)
    {
        IsStart = c == 'S';
        IsSplitter = c == '^';
    }

    public override string ToString()
    {
        if (IsStart)
            return "S";

        if (IsSplitter)
            return "^";

        if (HasBeam)
            return BeamCount.ToString();

        return ".";
    }

    public void AddBeam(ulong previousBeamCount = 0)
    {
        HasBeam = true;
        if (previousBeamCount == 0)
            BeamCount++;
        else
            BeamCount += previousBeamCount;
    }
}