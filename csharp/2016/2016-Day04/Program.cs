using Common;

namespace _2016_Day04;

public static class Program
{
    public static void Main()
    {
        // Part 1 sample
        string sampleInput = """
            aaaaa-bbb-z-y-x-123[abxyz]
            a-b-c-d-e-f-g-h-987[abcde]
            not-a-real-room-404[oarel]
            totally-real-room-200[decoy]
            """;
        int sumOfRealRoomIds = sampleInput
            .Lines()
            .Select(ParseRoom)
            .Where(room => room.IsReal)
            .Sum(room => room.Id);
        Console.WriteLine($"Part 1 sample: {sumOfRealRoomIds}");
        
        // Part 1
        sumOfRealRoomIds = FileReader
            .ReadLines("input.txt")
            .Select(ParseRoom)
            .Where(room => room.IsReal)
            .Sum(room => room.Id);
        Console.WriteLine($"Part 1: {sumOfRealRoomIds}");
        
        // Part 2 sample
        Room r = new("qzmt-zixmtkozy-ivhz", 343, "");
        if (r.Name != "very encrypted name")
            throw new Exception($"Part 2 sample room name wasn't decrypted correct: expected 'very encrypted name', got '{r.Name}'");
        Console.WriteLine($"Part 2 decryption sample: '{r.Name}'");
        
        // Part 
        Room northPoleObjectsRoom = FileReader
            .ReadLines("input.txt")
            .Select(ParseRoom)
            .Where(room => room.IsReal)
            .First(room => room.Name.Contains("object"));
        Console.WriteLine($"Part 2: {northPoleObjectsRoom.Id}: {northPoleObjectsRoom.Name}");
    }

    private static Room ParseRoom(string s)
    {
        ReadOnlySpan<char> span = s.AsSpan();
        
        string name = span[..span.LastIndexOf('-')].ToString();
        span = span[(span.LastIndexOf('-') + 1)..];
        string id = span[..span.IndexOf('[')].ToString();
        span = span[(span.IndexOf('[') + 1)..];
        string checksum = span[..5].ToString();
        
        return new Room(name, int.Parse(id), checksum);
    }
}

internal readonly struct Room
{
    public readonly string Name;
    public readonly int Id;
    public readonly string MaybeFalseChecksum;
    public readonly string Checksum;
    public readonly bool IsReal;

    public Room(string encryptedName, int id, string maybeFalseChecksum)
    {
        Id = id;
        MaybeFalseChecksum = maybeFalseChecksum;
        Checksum = CalculateActualChecksum(encryptedName);
        IsReal = MaybeFalseChecksum == Checksum;
        Name = DecryptName(encryptedName, id);
    }

    private static string CalculateActualChecksum(string encryptedName)
    {
        Dictionary<char, ushort> nameCharCounts = [];
        foreach (char c in encryptedName.Where(c => c != '-'))
        {
            if (nameCharCounts.TryGetValue(c, out ushort count))
                nameCharCounts[c] = ++count;
            else
                nameCharCounts[c] = 1;
        }

        return string.Concat(nameCharCounts
            .Select(pair => (pair.Key, pair.Value))
            .OrderByDescending(tuple => tuple.Value)
            .ThenBy(tuple => tuple.Key)
            .Take(5)
            .ToArray()
            .Select(tuple => tuple.Key)
        );
    }

    private static string DecryptName(string encryptedName, int id)
    {
        int actualRotationsToDo = id % 26;
        char[] chars = encryptedName.Replace('-', ' ').ToCharArray();
        
        for (int i = 0; i < chars.Length; i++)
        {
            char original = chars[i];
            
            if (original == ' ')
                continue;
            
            char intermediate = (char)(chars[i] + actualRotationsToDo);
            
            char decrypted = intermediate > 122 // 'z'
                ? (char)(intermediate - 26) 
                : intermediate;

            chars[i] = decrypted;
        }
        
        return string.Concat(chars);
    }
}