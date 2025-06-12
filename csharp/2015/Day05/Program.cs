using Common;

namespace Day05;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(ExpectIsNice(true, "ugknbfddgicrmopn", IsNicePart1));
        Console.WriteLine(ExpectIsNice(true, "aaa", IsNicePart1));
        Console.WriteLine(ExpectIsNice(false, "jchzalrnumimnmhp", IsNicePart1));
        Console.WriteLine(ExpectIsNice(false, "haegwjzuvuyypxyu", IsNicePart1));
        Console.WriteLine(ExpectIsNice(false, "dvszwmarrgswjxmb", IsNicePart1));
        Console.WriteLine();
        Console.WriteLine($"Part 1: {FileReader.ReadLines("input.txt").Count(IsNicePart1)}");
        Console.WriteLine();
        Console.WriteLine(ExpectIsNice(true, "qjhvhtzxzqqjkmpb", IsNicePart2));
        Console.WriteLine(ExpectIsNice(true, "xxyxx", IsNicePart2));
        Console.WriteLine(ExpectIsNice(false, "uurcxstgmygtbstg", IsNicePart2));
        Console.WriteLine(ExpectIsNice(false, "ieodomkazucvgmuy", IsNicePart2));
        Console.WriteLine();
        Console.WriteLine($"Part 2: {FileReader.ReadLines("input.txt").Count(IsNicePart2)}");
    }

    private static string ExpectIsNice(bool expectation, string s, Func<string, bool> action)
    {
        return $"Expecting string '{s}' to be {(expectation ? "nice" : "naughty")} and it is {(action(s) ? "NICE" : "NAUGHTY")}";
    }

    private static bool IsNicePart2(string s)
    {
        return ContainsPairsOfAnyTwoLettersAtLeastTwice(s) && ContainsAtLeastOneSymmetricTriple(s);
    }

    private static bool ContainsPairsOfAnyTwoLettersAtLeastTwice(string s)
    {
        for (int i = 0; i < s.Length - 3; i++)
        {
            string pair = s.Substring(i, 2);
            for (int j = i + 2; j < s.Length - 1; j++)
            {
                string other = s.Substring(j, 2);
                if (pair == other) 
                    return true;
            }
        }

        return false;
    }

    private static bool ContainsAtLeastOneSymmetricTriple(string s)
    {
        for (int i = 0; i < s.Length - 2; i++)
        {
            string triple = s.Substring(i, 3);
            if (triple[0] == triple[2])
                return true;
        }
        return false;
    }

    private static bool IsNicePart1(string s)
    {
        return DoesNotContainAnyBadPairs(s) && ContainsAtLeastThreeVowels(s) && ContainsAtLeastOnePair(s);
    }

    private static bool ContainsAtLeastThreeVowels(string s) => s.Count(IsVowel) >= 3;

    private static bool ContainsAtLeastOnePair(string s)
    {
        for (int i = 0; i < s.Length - 1; i++)
        {
            if (s[i] == s[i + 1])
                return true;
        }

        return false;
    }

    private static bool DoesNotContainAnyBadPairs(string s)
    {
        return !s.Contains("ab") && !s.Contains("cd") && !s.Contains("pq") && !s.Contains("xy");
    }

    private static bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';
}