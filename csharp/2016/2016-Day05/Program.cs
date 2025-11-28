using System.Security.Cryptography;
using System.Text;

namespace _2016_Day05;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine($"Password found: {Part1("abc")}");
        string input = File.ReadAllText("input.txt");
        Console.WriteLine($"Password found: {Part1(input)}");
        Console.WriteLine($"Password found: {Part2("abc")}");
        Console.WriteLine($"Password found: {Part2(input)}");
    }

    private static string Part1(string id)
    {
        int i = 0;
        char[] passwordChars = new char[8];
        int passwordCharIndex = 0;

        while (i < int.MaxValue)
        {
            string password = $"{id}{i++}";
            byte[] hash = MD5.HashData(Encoding.ASCII.GetBytes(password));
            
            // first 5 chars of hex representation of hash starts with five zeros
            //            00              00                0F
            if (hash[0] == 0 && hash[1] == 0 && hash[2] <= 0xF) 
            {
                char c = SecondCharOfHexRepresentation(hash[2]);
                passwordChars[passwordCharIndex++] = c;
                Console.WriteLine($"Algorithm 1: {new string(passwordChars.Select(it => it > 0 ? it : '_').ToArray())}");

                if (passwordCharIndex >= 8)
                    break;
            }
        }
        string result = new(passwordChars.ToArray());
        return result;
    }
    
    private static string Part2(string id)
    {
        int i = 0;
        char?[] passwordChars = new char?[8];

        while (i < int.MaxValue)
        {
            string password = $"{id}{i++}";
            byte[] hash = MD5.HashData(Encoding.ASCII.GetBytes(password));
            
            // first 5 chars of hex representation of hash starts with five zeros
            //            00              00                0F
            if (hash[0] == 0 && hash[1] == 0 && hash[2] <= 0xF)
            {
                int position = hash[2];

                if (position >= passwordChars.Length)
                    continue;
                
                if (passwordChars[position] is not null)
                    continue;
                
                char c = Convert.ToHexString([hash[3]]).ToLower()[0];
                passwordChars[position] = c;
                Console.WriteLine($"Algorithm 2: {new string(passwordChars.Select(it => it ?? '_').ToArray())}");

                if (passwordChars.All(it => it is not null))
                    break;
            }
        }
        string result = new(passwordChars.Select(it => it!.Value).ToArray());
        return result;
    }

    private static char SecondCharOfHexRepresentation(byte b)
    {
        return b switch
        {
            < 10 => (char)(48 + b),
            10 => 'a',
            11 => 'b',
            12 => 'c',
            13 => 'd',
            14 => 'e',
            15 => 'f',
            _ =>  throw new ArgumentOutOfRangeException(nameof(b), b, null)
        };
    }
}