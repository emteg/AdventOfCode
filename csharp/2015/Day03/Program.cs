using Common;

namespace Day03;

public static class Program
{
    public static void Main()
    {
        Part1(">"); // 2 houses
        Part1("^>v<"); // 4 houses
        Part1("^v^v^v^v^v"); // 2 houses
        
        Part1(File.ReadAllText("input.txt"));
        
        Part2("^v"); // 3 houses
        Part2("^>v<"); // 3 houses
        Part2("^v^v^v^v^v"); // 11 houses

        Part2(File.ReadAllText("input.txt"));
    }

    private static void Part1(string input)
    {
        Dictionary<(int x, int y), Cell2D> houses = [];
        Cell2D santa = new Cell2D(0, 0);
        houses.Add((0 ,0), santa);
        
        DeliverPresents(input, santa, houses);

        Console.WriteLine($"Delivered presents to {houses.Count} houses.");
    }
    
    private static void Part2(string input)
    {
        Dictionary<(int x, int y), Cell2D> houses = [];
        Cell2D santa = new Cell2D(0, 0);
        houses.Add((0 ,0), santa);
        Cell2D roboSanta = santa;
        
        DeliverPresents(input.CharsAtEvenIndices().AsString(), santa, houses);
        DeliverPresents(input.CharsAddOddIndices().AsString(), roboSanta, houses);

        Console.WriteLine($"Delivered presents to {houses.Count} houses.");
    }

    private static void DeliverPresents(string input, Cell2D currentHouse, Dictionary<(int x, int y), Cell2D> houses)
    {
        foreach (char c in input)
        {
            (Cell2D? neighbor, Action<Cell2D> connect, int x, int y) = CheckInput(currentHouse, c);

            if (neighbor is null) 
                houses.TryGetValue((x, y), out neighbor);
            
            if (neighbor is null)
            {
                neighbor = new Cell2D(x, y);
                houses.Add((x, y), neighbor);
                connect(neighbor);
            }

            currentHouse = neighbor;
        }
    }

    private static (Cell2D? neighbor, Action<Cell2D> connect, int x, int y) CheckInput(Cell2D currentHouse, char c)
    {
        return c switch
        {
            '^' => (currentHouse.Up,    cell => currentHouse.ConnectUp(cell),    currentHouse.X,     currentHouse.Y - 1),
            '>' => (currentHouse.Right, cell => currentHouse.ConnectRight(cell), currentHouse.X + 1, currentHouse.Y),
            'v' => (currentHouse.Down,  cell => currentHouse.ConnectDown(cell),  currentHouse.X,     currentHouse.Y + 1),
            '<' => (currentHouse.Left,  cell => currentHouse.ConnectLeft(cell),  currentHouse.X - 1, currentHouse.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, "Invalid direction!")
        };
    }
}