using Common;

namespace Day02;

public static class Program
{
    public static void Main()
    {
        ExamplesPart1();
        Console.WriteLine(Part1());

        ExamplesPart2();
        Console.WriteLine(Part2());
    }

    private static long Part1()
    {
        long totalWrappingPaperArea = FileReader
            .ReadLines("input.txt")
            .Select(Present.FromString)
            .Sum(it => it.WrappingPaperArea);
        return totalWrappingPaperArea;
    }
    
    private static long Part2()
    {
        long totalRibbonLength = FileReader
            .ReadLines("input.txt")
            .Select(Present.FromString)
            .Sum(it => it.RibbonLength);
        return totalRibbonLength;
    }

    private static void ExamplesPart1()
    {
        string dimensions = "2x3x4";
        Present present = Present.FromString(dimensions);
        Console.WriteLine($"{dimensions} => {present.SurfaceArea} + {present.SurfaceAreaSmallestSide} = {present.WrappingPaperArea} (expected: 52 + 6 = 58)");

        dimensions = "1x1x10";
        present = Present.FromString(dimensions);
        Console.WriteLine($"{dimensions} => {present.SurfaceArea} + {present.SurfaceAreaSmallestSide} = {present.WrappingPaperArea} (expected: 42 + 1 = 43)");
    }
    
    private static void ExamplesPart2()
    {
        string dimensions = "2x3x4";
        Present present = Present.FromString(dimensions);
        Console.WriteLine($"{dimensions} => {present.PerimeterSmallestSide} + {present.Volume} = {present.RibbonLength} (expected: 10 + 24 = 34)");

        dimensions = "1x1x10";
        present = Present.FromString(dimensions);
        Console.WriteLine($"{dimensions} => {present.PerimeterSmallestSide} + {present.Volume} = {present.RibbonLength} (expected: 4 + 10 = 14)");
    }
}