using System.Diagnostics;
using Common;

namespace Day14;

public static class Program
{
    public static void Main()
    {
        const string sample = """
            Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
            Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
            """;
        Console.WriteLine($"Part 1 (sample): winner has travelled {Part1(sample.Lines(), 1000).DistanceTravelled} km");
        Console.WriteLine($"Part 1: winner has travelled {Part1(File.ReadAllLines("input.txt"), 2503).DistanceTravelled} km");
        
        Console.WriteLine($"Part 2 (sample): winner has {Part2(sample.Lines(), 1000).Points} points");
        Console.WriteLine($"Part 2: winner has {Part2(File.ReadAllLines("input.txt"), 2503).Points} points");
    }

    private static Reindeer Part1(IEnumerable<string> inputLines, int raceDurationSeconds)
    {
        List<Reindeer> reindeers = inputLines.ParseInput().ToList();
        foreach (Reindeer reindeer in reindeers)
            for (int i = 0; i < raceDurationSeconds; i++) 
                reindeer.Update();
        
        Reindeer best = reindeers.First();
        foreach (Reindeer reindeer in reindeers)
        {
            if (reindeer.DistanceTravelled > best.DistanceTravelled)
                best = reindeer;
        }

        return best;
    }

    private static Reindeer Part2(IEnumerable<string> inputLines, int raceDurationSeconds)
    {
        List<Reindeer> reindeers = inputLines.ParseInput().ToList();
        int bestDistance = 0;
        
        for (int i = 0; i < raceDurationSeconds; i++)
        {
            foreach (Reindeer reindeer in reindeers)
            {
                reindeer.Update();
                if (reindeer.DistanceTravelled > bestDistance) 
                    bestDistance = reindeer.DistanceTravelled;
            }
            
            foreach (Reindeer reindeer in reindeers.Where(reindeer => reindeer.DistanceTravelled == bestDistance))
                reindeer.AwardPoint();
        }
        
        Reindeer best = reindeers.First();
        foreach (Reindeer reindeer in reindeers)
        {
            if (reindeer.Points > best.Points)
                best = reindeer;
        }

        return best;
    }

    private static IEnumerable<Reindeer> ParseInput(this IEnumerable<string> inputLines)
    {
        return inputLines.Select(line => line.ToReindeer());
    }

    private static Reindeer ToReindeer(this string line)
    {
        string[] words = line.Split(" ");
        string name = words[0];
        int speed = int.Parse(words[3]);
        int flightDuration = int.Parse(words[6]);
        int restDuration = int.Parse(words[13]);
        
        return new Reindeer(name, speed, flightDuration, restDuration);
    }
}

[DebuggerDisplay("{Name} {DistanceTravelled}")]
internal sealed class Reindeer
{
    public readonly string Name;
    public readonly int SpeedKmPerSecond;
    public readonly int FlightDuration;
    public readonly int RestDuration;
    public int DistanceTravelled { get; private set; }
    public int Points { get; private set; }

    public Reindeer(string name, int speedKmPerSecond, int flightDuration, int restDuration)
    {
        Name = name;
        SpeedKmPerSecond = speedKmPerSecond;
        FlightDuration = flightDuration;
        RestDuration = restDuration;
        counter = FlightDuration;
    }

    public void Update()
    {
        counter--;

        if (isFlying)
            DistanceTravelled += SpeedKmPerSecond;
        
        if (counter > 0) 
            return;
        
        counter = isFlying ? RestDuration : FlightDuration;
        isFlying = !isFlying;
    }

    public void AwardPoint() => Points++;

    private bool isFlying = true;
    private int counter;
}