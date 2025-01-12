namespace Day01;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(Part1());
        Console.WriteLine(Part2());
    }

    private static int Part1()
    {
        int result = 0;
        foreach (char c in File.ReadAllText("input.txt"))
        {
            if (c == '(')
                result++;
            else if (c == ')')
                result--;
            
        }

        return result;
    }
    
    private static uint Part2()
    {
        int result = 0;
        uint position = 0;
        foreach (char c in File.ReadAllText("input.txt"))
        {
            position++;
            
            if (c == '(')
                result++;
            else if (c == ')')
                result--;
            
            if (result < 0)
                break;
        }

        return position;
    }
}