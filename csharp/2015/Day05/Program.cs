using Common;

namespace Day05;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(IsNice("ugknbfddgicrmopn"));
        Console.WriteLine(IsNice("aaa"));
        Console.WriteLine(IsNice("jchzalrnumimnmhp"));
        Console.WriteLine(IsNice("haegwjzuvuyypxyu"));
        Console.WriteLine(IsNice("dvszwmarrgswjxmb"));

        Console.WriteLine(FileReader.ReadLines("input.txt").Count(IsNice));
    }

    private static bool IsNice(string s)
    {
        uint vowels = 0;
        bool has3Vowels = false;
        bool hasDoubleLetter = false;
        char lastChar = '\0';
        bool firstChar = true;
        foreach (char c in s)
        {
            if (!has3Vowels && IsVowel(c))
            {
                vowels++;
                if (vowels >= 3)
                    has3Vowels = true;
            }

            if (firstChar)
            {
                firstChar = false;
                lastChar = c;
                continue;
            }

            if (!hasDoubleLetter && lastChar == c)
            {
                hasDoubleLetter = true;
            }

            if ((lastChar == 'a' && c == 'b') ||
                (lastChar == 'c' && c == 'd') ||
                (lastChar == 'p' && c == 'q') || 
                (lastChar == 'x' && c == 'y'))
            {
                return false;
            }

            firstChar = false;
            lastChar = c;
        }

        return has3Vowels && hasDoubleLetter;
    }

    private static bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';
}