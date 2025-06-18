using System.Diagnostics;
using Common;

namespace Day13;

public static class Program
{
    public static void Main()
    {
        const string sample = """
            Alice would gain 54 happiness units by sitting next to Bob.
            Alice would lose 79 happiness units by sitting next to Carol.
            Alice would lose 2 happiness units by sitting next to David.
            Bob would gain 83 happiness units by sitting next to Alice.
            Bob would lose 7 happiness units by sitting next to Carol.
            Bob would lose 63 happiness units by sitting next to David.
            Carol would lose 62 happiness units by sitting next to Alice.
            Carol would gain 60 happiness units by sitting next to Bob.
            Carol would gain 55 happiness units by sitting next to David.
            David would gain 46 happiness units by sitting next to Alice.
            David would lose 7 happiness units by sitting next to Bob.
            David would gain 41 happiness units by sitting next to Carol.
            """;
        SeatingArrangement sampleResult = Part1(sample.Lines());
        Console.WriteLine($"Part 1 (sample): {sampleResult}");
        SeatingArrangement part1Result = Part1(File.ReadAllLines("input.txt"));
        Console.WriteLine($"Part 1: {part1Result}");
        
        Console.WriteLine($"Part 2 (sample): {Part2(sampleResult)}");
        Console.WriteLine($"Part 2: {Part2(part1Result)}");
    }

    private static int Part2(SeatingArrangement arrangement)
    {
        SeatedGuest first = arrangement.FirstGuest;
        SeatedGuest next = first.NextGuest!;
        int smallestGain = first.HappinessFromNextGuest + first.HappinessTowardsNextGuest;

        while (next != first)
        {
            int gain = next.HappinessFromNextGuest + next.HappinessTowardsNextGuest;
            if (gain < smallestGain)
                smallestGain = gain;
            next = next.NextGuest!;
        }
        
        return arrangement.Happiness - smallestGain;
    }

    private static SeatingArrangement Part1(IEnumerable<string> inputLines)
    {
        Dictionary<string, Dictionary<string, int>> guestDict = ParseInput(inputLines);
        SeatingArrangement? best = null;
        
        foreach (string guest in guestDict.Keys)
        {
            string[] remainingGuests = guestDict
                .Where(it => it.Key != guest)
                .Select(it => it.Key)
                .ToArray();
            foreach (SeatingArrangement seatingArrangement in FindArrangements([guest], remainingGuests, guestDict))
            {
                if (best is null) 
                    best = seatingArrangement;
                else if (best.Happiness < seatingArrangement.Happiness)
                    best = seatingArrangement;
            }
        }

        return best!;
    }

    private static IEnumerable<SeatingArrangement> FindArrangements(
        string[] placedGuests, string[] remainingGuests, Dictionary<string, Dictionary<string, int>> guestDict)
    {
        if (remainingGuests.Length == 1)
            yield return new SeatingArrangement([..placedGuests, remainingGuests[0]], guestDict);
        
        foreach (string nextGuest in remainingGuests)
        {
            string[] nextRemainingGuests = remainingGuests.Where(it => it != nextGuest).ToArray();
            IEnumerable<SeatingArrangement> arrangements = FindArrangements([..placedGuests, nextGuest], nextRemainingGuests, guestDict);
            foreach (SeatingArrangement seatingArrangement in arrangements)
                yield return seatingArrangement;
        }
    }

    private static Dictionary<string, Dictionary<string, int>> ParseInput(IEnumerable<string> lines)
    {
        Dictionary<string, Dictionary<string, int>> result = [];

        foreach ((string name, int happiness, string otherName) in lines.Select(ParseLine))
        {
            if (!result.ContainsKey(name))
                result.Add(name, []);
            
            result[name].Add(otherName, happiness);
        }
        
        return result;
    }

    private static (string name, int happiness, string otherName) ParseLine(string line)
    {
        string[] words = line.Split(" ");
        string name = words[0];
        int sign = words[2] == "gain" ? 1 : -1;
        int happiness = int.Parse(words[3]) * sign;
        string otherName = words[10][..^1];
        
        return (name, happiness, otherName);
    }
}

internal sealed class SeatingArrangement
{
    public readonly SeatedGuest FirstGuest;

    public int Happiness
    {
        get
        {
            int result = 0;
            SeatedGuest last = FirstGuest;
            while (last.NextGuest != FirstGuest)
            {
                result += last.HappinessFromNextGuest + last.HappinessTowardsNextGuest;
                last = last.NextGuest!;
            }
            result += last.HappinessFromNextGuest + last.HappinessTowardsNextGuest;
            return result;
        }
    }

    public SeatingArrangement(string[] guests, Dictionary<string, Dictionary<string, int>> guestDictionary)
    {
        FirstGuest = new SeatedGuest(guests[0]);
        SeatedGuest lastPlaced = FirstGuest;
        for (int i = 1; i < guests.Length; i++)
        {
            string nextGuest = guests[i];
            SeatedGuest nextPlaced = new(nextGuest);
            lastPlaced.PlaceNextGuest(nextPlaced, guestDictionary);
            lastPlaced = nextPlaced;
        }
        lastPlaced.PlaceNextGuest(FirstGuest, guestDictionary);
    }

    public override string ToString() => $"{Happiness}";
}

internal sealed class SeatedGuest
{
    public readonly string Name;
    public SeatedGuest? NextGuest { get; private set; }
    public int HappinessTowardsNextGuest { get; private set; }
    public int HappinessFromNextGuest { get; private set; }

    public SeatedGuest(string name)
    {
        Name = name;
    }

    public void PlaceNextGuest(SeatedGuest nextGuest, Dictionary<string, Dictionary<string, int>> guestDictionary)
    {
        NextGuest = nextGuest;
        HappinessTowardsNextGuest = guestDictionary[Name][nextGuest.Name];
        HappinessFromNextGuest = guestDictionary[nextGuest.Name][Name];
    }

    public override string ToString()
    {
        return NextGuest is null
            ? Name
            : $"{Name}-{NextGuest.Name}: {HappinessTowardsNextGuest + HappinessFromNextGuest}";
    }
}