using System.Security.Cryptography;
using System.Text;

namespace Day04;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(Part1("abcdef"));
        Console.WriteLine(Part1("pqrstuv"));
        string puzzleInput = File.ReadAllText("input.txt");
        Console.WriteLine(Part1(puzzleInput));
        
        Console.WriteLine(Part2(puzzleInput));
    }

    private static uint Part1(string secretKey)
    {
        return Execute(secretKey, HashStartsWithFiveZeros);
    }
    
    private static uint Part2(string secretKey)
    {
        return Execute(secretKey, HashStartsWithSixZeros);
    }
    
    private static uint Execute(string secretKey, Func<string, uint, bool> action)
    {
        uint i = 0;
        while (!action(secretKey, i))
        {
            if (i % 30_000 == 0)
                Console.WriteLine(i);
            
            if (i == uint.MaxValue)
                break;
            
            i++;
        }
        return i;
    }

    public static bool HashStartsWithFiveZeros(string secretKey, uint n)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes($"{secretKey}{n}"));
        return hash[0] == 0 && hash[1] == 0 && hash[2] <= 15;
    }
    
    public static bool HashStartsWithSixZeros(string secretKey, uint n)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes($"{secretKey}{n}"));
        return hash[0] == 0 && hash[1] == 0 && hash[2] == 0;
    }
}