using Common;

namespace _2016_Day07;

public static class Program
{
    public static void Main()
    {
        // Part 1 samples
        Console.WriteLine(Ipv7Address.Parse("abba[mnop]qrst").SupportsTls); // true
        Console.WriteLine(Ipv7Address.Parse("abcd[bddb]xyyx").SupportsTls); // false
        Console.WriteLine(Ipv7Address.Parse("aaaa[qwer]tyui").SupportsTls); // false
        Console.WriteLine(Ipv7Address.Parse("ioxxoj[asdfgh]zxcvbn").SupportsTls); // true

        // Part 1
        Console.WriteLine();
        Ipv7Address[] ipv7Addresses = FileReader
            .ReadLines("input.txt")
            .Select(Ipv7Address.Parse)
            .ToArray();
        Console.WriteLine(ipv7Addresses.Count(address => address.SupportsTls));
        
        // Part2 samples
        Console.WriteLine();
        Console.WriteLine(Ipv7Address.Parse("aba[bab]xyz").SupportsSsl); // true
        Console.WriteLine(Ipv7Address.Parse("xyx[xyx]xyx").SupportsSsl); // false
        Console.WriteLine(Ipv7Address.Parse("aaa[kek]eke").SupportsSsl); // true
        Console.WriteLine(Ipv7Address.Parse("zazbz[bzb]cdb").SupportsSsl); // true
        
        // Part 2
        Console.WriteLine();
        Console.WriteLine(ipv7Addresses.Count(address => address.SupportsSsl));
    }
}

internal sealed class Ipv7Address
{
    public IReadOnlyList<Ipv7AddressSequence> Sequence => sequences;
    public IEnumerable<SupernetSequence> SupernetSequences => sequences
        .Where(sequence => sequence is SupernetSequence)
        .Cast<SupernetSequence>();
    public IEnumerable<HypernetSequence> HypernetSequences => sequences
        .Where(sequence => sequence is HypernetSequence)
        .Cast<HypernetSequence>();

    public bool SupportsTls => SupernetSequences.Any(it => it.ContainsAbbaSequence) &&
                               !HypernetSequences.Any(it => it.ContainsAbbaSequence);
    public bool SupportsSsl { get; }

    public Ipv7Address(IEnumerable<Ipv7AddressSequence> sequences)
    {
        this.sequences = sequences.ToArray();

        // An IP supports SSL if it has an ABA in any Supernet sequence AND...
        IEnumerable<string> abaSequences = SupernetSequences
            .Where(supernet => supernet.ContainsAbaSequence)
            .SelectMany(supernet => supernet.AbaSequences);
        
        // ...a corresponding BAB anywhere in the Hypernet sequences
        SupportsSsl = abaSequences
            .Any(aba => HypernetSequences.Any(hypernet => hypernet.ContainsBabSequenceFor(aba)));
    }

    public static Ipv7Address Parse(string s)
    {
        ReadOnlySpan<char> span = s.AsSpan();
        List<Ipv7AddressSequence> sequences = [];
        while (span.Length > 0)
        {
            span = NextChunk(span, out ReadOnlySpan<char> chunk, out bool isWithinBrackets);
            sequences.Add(Ipv7AddressSequence.New(chunk, isWithinBrackets));
        }
        return new Ipv7Address(sequences);
    }
    
    private static ReadOnlySpan<char> NextChunk(
        ReadOnlySpan<char> span, out ReadOnlySpan<char> chunk, out bool isWithinBrackets)
    {
        ReadOnlySpan<char> remainder;
        isWithinBrackets = span[0] == '[';

        int endIndex;
        if (!isWithinBrackets)
        {
            endIndex = span.IndexOf('[');
            if (endIndex < 0)
                endIndex = span.Length;
            chunk = span[..endIndex];
            remainder = span[endIndex..];
            return remainder;
        }

        endIndex = span.IndexOf(']');
        if (endIndex < 0)
            endIndex = span.Length - 1;
            
        chunk = span[1..endIndex];
        remainder = span[(endIndex + 1)..];
        return remainder;
    }
    
    private readonly Ipv7AddressSequence[] sequences;
}

internal abstract class Ipv7AddressSequence
{
    public string RawValue { get; }

    /// <summary>
    /// An ABBA sequence is any four-character sequence which consists of a pair of two different characters followed
    /// by the reverse of that pair, such as 'xyyx' or 'abba'
    /// </summary>
    public IReadOnlyList<string> AbbaSequences { get; }
    public bool ContainsAbbaSequence => AbbaSequences.Any();
    
    /// <summary>
    /// An ABA sequence is any three-character sequence which consists of the same character twice with a different
    /// character between them, such as xyx or aba
    /// </summary>
    public IReadOnlyList<string> AbaSequences { get; }
    public bool ContainsAbaSequence => AbaSequences.Any();

    public bool ContainsBabSequenceFor(string abaSequence)
    {
        string bab = new([abaSequence[1], abaSequence[0], abaSequence[1]]);
        return RawValue.Contains(bab);
    }

    public static Ipv7AddressSequence New(ReadOnlySpan<char> span, bool isWithinBrackets)
    {
        return isWithinBrackets
            ? new HypernetSequence(span.ToString())
            : new SupernetSequence(span.ToString());
    }
    
    protected Ipv7AddressSequence(string value)
    {
        RawValue = value;
        AbbaSequences = GetAbbaSequences(value).ToArray();
        AbaSequences = GetAbaSequences(RawValue).ToArray();
    }

    private static IEnumerable<string> GetAbbaSequences(string input)
    {
        if (input.Length < 4)
            yield break;

        for (int startIndex = 0; startIndex <= input.Length - 4; startIndex++)
        {
            if (input[startIndex] != input[startIndex + 1] &&
                input[startIndex] == input[startIndex + 3] &&
                input[startIndex + 1] == input[startIndex + 2])
                yield return new string(input.AsSpan()[..(startIndex + 4)]);
        }
    }
    
    private static IEnumerable<string> GetAbaSequences(string input)
    {
        if (input.Length < 3)
            yield break;

        for (int startIndex = 0; startIndex <= input.Length - 3; startIndex++)
        {
            if (input[startIndex] == input[startIndex + 2] && input[startIndex] != input[startIndex + 1])
                yield return new string(input.AsSpan()[startIndex..(startIndex + 3)]);
        }
    }
}

/// <summary>A subsequence of an Ipv7Address that is contained by square brackets</summary>
internal sealed class HypernetSequence : Ipv7AddressSequence
{
    public HypernetSequence(string value) : base(value) { }
}

/// <summary>A subsequence of an Ipv7Address that is NOT contained by square brackets</summary>
internal sealed class SupernetSequence : Ipv7AddressSequence
{
    public SupernetSequence(string value) : base(value) { }
}