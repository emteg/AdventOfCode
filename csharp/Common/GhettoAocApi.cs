namespace Common;

public static class GhettoAocApi
{
    public static string SessionCookie = string.Empty;
    
    public static IEnumerable<string> ReadLinesFromCachedPuzzleInput(ushort year, byte day)
    {
        if (File.Exists("input.txt"))
            return FileReader.ReadLines("input.txt");
        
        if (SessionCookie == string.Empty)
            throw new InvalidOperationException("Session cookie is empty");
        
        HttpRequestMessage request = new(HttpMethod.Get,
            $"https://adventofcode.com/{year}/day/{day}/input");
        request.Headers.Add("Cookie", $"session={SessionCookie}");
        HttpResponseMessage response = Client.SendAsync(request).Result.EnsureSuccessStatusCode();
        
        using Stream s = response.Content.ReadAsStream();
        using FileStream fileStream = File.OpenWrite("input.txt");
        s.CopyTo(fileStream);
        
        return FileReader.ReadLines("input.txt");
    }

    public static void QuerySubmitAnswer(ushort year, byte day, byte part, string answer)
    {
        Console.WriteLine("Submit answer now? (y/n)");
        string? l = Console.ReadLine();
        if (l == "y")
        {
            bool wasCorrect = SubmitAnswer(year, day, part, answer);
            Console.WriteLine($"Answer was correct? -> {wasCorrect}");
        }
    }

    public static bool SubmitAnswer(ushort year, byte day, byte part, string answer)
    {
        if (SessionCookie == string.Empty)
            throw new InvalidOperationException("Session cookie is empty");
        
        HttpRequestMessage request = new(HttpMethod.Post, 
            $"https://adventofcode.com/{year}/day/{day}/answer");
        
        request.Headers.Add("Cookie", $"session={SessionCookie}");
        
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("level", part.ToString()),
            new KeyValuePair<string, string>("answer", answer)
        ]);
        
        HttpResponseMessage response = Client.SendAsync(request).Result.EnsureSuccessStatusCode();
        string content = response.Content.ReadAsStringAsync().Result;

        return !content.Contains("That's not the right answer.");
    }

    private static readonly HttpClientHandler Handler = new() { UseCookies = false };
    private static readonly HttpClient Client = new(Handler);
}