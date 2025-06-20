﻿using System.Diagnostics;
using Common;

namespace Day15;

public static class Program
{
    public static void Main()
    {
        const string sample = """
                              Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
                              Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3
                              """;
        Ingredient[] sampleIngredients = sample.Lines().Select(Ingredient.FromLine).ToArray();
        Console.WriteLine($"Part 1 (sample): {Part1For2Ingredients(sampleIngredients)}");
        
        Ingredient[] part1Ingredients = File.ReadAllLines("input.txt").Select(Ingredient.FromLine).ToArray();
        Console.WriteLine($"Part 1: {Part1For4Ingredients(part1Ingredients)}");
        
        Console.WriteLine($"Part 2 (sample): {Part1For2Ingredients(sampleIngredients, true)}");
        Console.WriteLine($"Part 2: {Part1For4Ingredients(part1Ingredients, true)}");
    }

    private static int Part1For2Ingredients(Ingredient[] ingredients, bool limitTo500Calories = false)
    {
        int bestScore = 0;
        for (int a = 0; a <= 100; a++)
        {
            for (int b = 0; b <= 100; b++)
            {
                if (a + b != 100)
                    continue;
                
                int score = CalculateScore(ingredients, [a, b], limitTo500Calories);
                Console.Write($"{a}/{b}: {score}");
                if (bestScore < score)
                {
                    Console.Write("-> new best score!");
                    bestScore = score;
                }
                Console.WriteLine();
            }
        }
        return bestScore;
    }
    
    private static int Part1For4Ingredients(Ingredient[] ingredients, bool limitTo500Calories = false)
    {
        int bestScore = 0;
        for (int a = 0; a <= 100; a++)
        {
            for (int b = 0; b <= 100; b++)
            {
                for (int c = 0; c <= 100; c++)
                {
                    int d = 100 - a - b - c;
                    
                    if (a + b + c + d != 100)
                        continue;
                    
                    int score = CalculateScore(ingredients, [a, b, c, d], limitTo500Calories);
                        
                    if (score == 0)
                        continue;
                        
                    if (score < bestScore)
                        continue;
                        
                    Console.WriteLine($"{a}/{b}/{c}/{d}: {score} -> new best score!");
                    bestScore = score;
                }
            }
        }
        return bestScore;
    }

    private static int CalculateScore(Ingredient[] ingredients, int[] weights, bool limitTo500Calories = false)
    {
        Ingredient scoreIngr = ingredients[0].Mult(weights[0]);
        for (int i = 1; i < ingredients.Length; i++)
        {
            Ingredient tmp = ingredients[i].Mult(weights[i]);
            scoreIngr = scoreIngr.Add(tmp);
        }
        int score = scoreIngr.Score(limitTo500Calories);
        return score;
    }
}

[DebuggerDisplay("{Name}: capacity {Capacity}, durability {Durability}, flavor {Flavor}, texture {Texture}, calories {Calories}")]
internal readonly struct Ingredient
{
    public readonly string Name;
    public readonly int Capacity;
    public readonly int Durability;
    public readonly int Flavor;
    public readonly int Texture;
    public readonly int Calories;

    public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
    {
        Name = name;
        Capacity = capacity;
        Durability = durability;
        Flavor = flavor;
        Texture = texture;
        Calories = calories;
    }

    public Ingredient Mult(int multiplier)
    {
        return new Ingredient(Name, Capacity * multiplier, Durability * multiplier, Flavor * multiplier, Texture * multiplier, Calories * multiplier);
    }

    public Ingredient Add(Ingredient other)
    {
        int newCap = Capacity + other.Capacity;
        int newDur = Durability + other.Durability;
        int newFlv = Flavor + other.Flavor;
        int newTxt = Texture + other.Texture;
        return new Ingredient("Added", newCap, newDur, newFlv, newTxt, Calories + other.Calories);
    }

    public int Score(bool limitTo500Calories = false)
    {
        if (Capacity < 0 || Durability < 0 || Flavor < 0 || Texture < 0)
            return 0;
        if (limitTo500Calories && Calories != 500)
            return 0;
        return Capacity * Durability * Flavor * Texture;
    }

    public static Ingredient FromLine(string line)
    {
        string[] nameAndValues = line.Split(": ");
        string name = nameAndValues[0];
        string[] values = nameAndValues[1].Split(", ");
        int capacity = GetValue(values[0]);
        int durability = GetValue(values[1]);
        int flavor = GetValue(values[2]);
        int texture = GetValue(values[3]);
        int calories = GetValue(values[4]);
        
        return new Ingredient(name, capacity, durability, flavor, texture, calories);
    }

    private static int GetValue(string nameAndValue)
    {
        string[] items = nameAndValue.Split(' ');
        return int.Parse(items[1]);
    }
}