namespace Day16;

public static class Program
{
    public static void Main()
    {
        Dictionary<string, int> bestSue = [];
        Dictionary<string, int> detections = new()
        {
            {"children", 3},
            {"cats", 7},
            {"samoyeds", 2},
            {"pomeranians", 3},
            {"akitas", 0},
            {"vizslas", 0},
            {"goldfish", 5},
            {"trees", 3},
            {"cars", 2},
            {"perfumes", 1},
        };
        Dictionary<string, Func<int, int, bool>> operators = new()
        {
            {"children", IsEqual },
            {"cats", IsEqual},
            {"samoyeds", IsEqual},
            {"pomeranians", IsEqual},
            {"akitas", IsEqual},
            {"vizslas", IsEqual},
            {"goldfish", IsEqual},
            {"trees", IsEqual},
            {"cars", IsEqual},
            {"perfumes", IsEqual},
        };
        Console.WriteLine($"Part 1: best sue is {FindSue(detections, operators)}");
        
        operators = new Dictionary<string, Func<int, int, bool>>
        {
            {"children", IsEqual },
            {"cats", SuesValueIsGreaterThan},
            {"samoyeds", IsEqual},
            {"pomeranians", SuesValueIsLessThan},
            {"akitas", IsEqual},
            {"vizslas", IsEqual},
            {"goldfish",SuesValueIsLessThan},
            {"trees", SuesValueIsGreaterThan},
            {"cars", IsEqual},
            {"perfumes", IsEqual},
        };
        Console.WriteLine($"Part 2: best sue is {FindSue(detections, operators)}");
    }

    private static int FindSue(Dictionary<string, int> detections, Dictionary<string, Func<int, int, bool>> operators)
    {
        int bestSueNumber = 0;
        int bestSueScore = 0;
        int i = 0;
        foreach (string line in File.ReadAllLines("input.txt"))
        {
            i++;
            Dictionary<string, int> sue = SueFromLine(line);
            int score = 0;
            foreach ((string detection, int detectedValue) in detections)
            {
                if (!sue.TryGetValue(detection, out int suesValue))
                    continue;

                bool checkResult = operators[detection](detectedValue, suesValue);
                if (checkResult)
                    score++;
            }

            if (score > bestSueScore)
            {
                bestSueScore = score;
                bestSueNumber = i;
            }
        }

        return bestSueNumber;
    }

    private static bool IsEqual(int a, int b) => a == b;
    private static bool SuesValueIsGreaterThan(int detected, int suesValue) => detected < suesValue;
    private static bool SuesValueIsLessThan(int detected, int suesValue) => detected > suesValue;
    
    private static Dictionary<string, int> SueFromLine(string line)
    {
        Dictionary<string, int> result = [];
        string[] properties = line[(line.IndexOf(": ", StringComparison.Ordinal) + 2)..].Split(", ");
        foreach (string property in properties)
        {
            string[] nameAndValue = property.Split(": ");
            string name = nameAndValue[0];
            int value = int.Parse(nameAndValue[1]);
            result[name] = value;
        }
        return result;
    }
}

