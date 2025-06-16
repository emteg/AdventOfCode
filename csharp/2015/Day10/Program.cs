namespace Day10;

public static class Program
{
    public static void Main()
    {
        //Console.WriteLine($"Part 1: {LookAndSay("1", 5).Length}");
        Console.WriteLine($"Part 1: {LookAndSay("3113322113", 50).Length}");
    }

    private static string LookAndSay(string s, int iterations)
    {
        while (iterations > 0)
        {
            List<char> chars = [];
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                int howMany = 1;
                for (int j = i + 1; j < s.Length; j++)
                {
                    if (s[j] != c)
                        break;
                    
                    howMany++;
                }

                string howManyStr = howMany.ToString();
                chars.AddRange(howManyStr.ToCharArray());
                chars.Add(c);
                i += howMany - 1;
            }

            string result = new string(chars.ToArray());
            s = result;
            iterations--;
        }

        return s;
    }
}