﻿using Common;

namespace Day12;

public static class Program
{
    public static void Main()
    {
        Part1("[1,2,3]", 6);
        Part1("""{"a":2,"b":4}""", 6);
        Part1("""[[[3]]]""", 3);
        Part1("""{"a":{"b":4},"c":-1}""", 3);
        Part1("""{"a":[-1,1]}""", 0);
        Part1("""[-1,{"a":1}]""", 0);
        Part1("""{}""", 0);
        Part1("""[]""", 0);
        Part1(File.ReadAllText("input.txt"));

        #region Json parsing tests
        /*
        // array document
        Console.WriteLine($"0: {SumJson("""[]""")}");
        Console.WriteLine($"1: {SumJson("""[1]""")}");
        Console.WriteLine($"6: {SumJson("""[1,2,3]""")}");
        Console.WriteLine($"342: {SumJson("""[1,20,321]""")}");
        Console.WriteLine($"-344: {SumJson("""[-1,2,-345]""")}");
        Console.WriteLine($"0: {SumJson("""[[]]""")}");
        Console.WriteLine($"6: {SumJson("""[[1],[2],[3]]""")}");
        Console.WriteLine($"2: {SumJson("""[[],[1,-2],[3]]""")}");
        Console.WriteLine($"0: {SumJson("""[""]""")}");
        Console.WriteLine($"0: {SumJson("""["whatever"]""")}");
        Console.WriteLine($"1: {SumJson("""["whatever",1,""]""")}");
        Console.WriteLine($"8: {SumJson("""["whatever",1,"2",[3,4,"red[]:,-"],0],[[[[]]]]""")}");

        // object document
        Console.WriteLine($"0: {SumJson("""{}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":0}""")}");
        Console.WriteLine($"1: {SumJson("""{"abc":1}""")}");
        Console.WriteLine($"234: {SumJson("""{"abc":234}""")}");
        Console.WriteLine($"-1: {SumJson("""{"abc":-1}""")}");
        Console.WriteLine($"-234: {SumJson("""{"abc":-234}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":""}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":"whatever"}""")}");
        Console.WriteLine($"3: {SumJson("""{"abc":1,"def":2}""")}");
        Console.WriteLine($"6: {SumJson("""{"abc":1,"def":2,"ghi":3}""")}");
        Console.WriteLine($"5: {SumJson("""{"abc":"1","def":2,"ghi":3}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":1,"def":"red","ghi":3}""")}"); // any object that includes a string property 'red' should be ignored
        Console.WriteLine($"0: {SumJson("""{"abc":[]}""")}");
        Console.WriteLine($"1: {SumJson("""{"abc":[1]}""")}");
        Console.WriteLine($"6: {SumJson("""{"abc":[1,2,3]}""")}");
        Console.WriteLine($"342: {SumJson("""{"abc":[1,20,321]}""")}");
        Console.WriteLine($"-344: {SumJson("""{"abc":[-1,2,-345]}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":[[]]}""")}");
        Console.WriteLine($"6: {SumJson("""{"abc":[[1],[2],[3]]}""")}");
        Console.WriteLine($"2: {SumJson("""{"abc":[[],[1,-2],[3]]}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":[""]}""")}");
        Console.WriteLine($"0: {SumJson("""{"abc":["whatever"]}""")}");
        Console.WriteLine($"1: {SumJson("""{"abc":["whatever",1,""]}""")}");
        Console.WriteLine($"8: {SumJson("""{"abc":["whatever",1,"2",[3,4,"red[]:,-"],0]}""")}");
        Console.WriteLine($"15: {SumJson("""{"abc":[1,2,3]},"def":[[4],5,"red"]""")}");
        
        // object in object
        Console.WriteLine($"0: {SumJson("""{"abc":{}}""")}");
        Console.WriteLine($"3: {SumJson("""{"abc":{"def":1,"ghi":[2]}}""")}");
        Console.WriteLine($"2: {SumJson("""{"abc":{"def":"red","ghi":[1]},"jkl":2}""")}"); // objects with property value of 'red' should be ignored

        // object in array document
        Console.WriteLine($"0: {SumJson("""[{}]""")}");
        Console.WriteLine($"0: {SumJson("""[{},{}]""")}");
        Console.WriteLine($"6: {SumJson("""[{},1,{"abc":2,"def":[3]},{"ghi":4,"jkl":"red","mno":5}]""")}");
        */
        #endregion

        Part2("""[1,2,3]""", 6);
        Part2("""[1,{"c":"red","b":2},3]""", 4);
        Part2("""{"d":"red","e":[1,2,3,4],"f":5}""", 0);
        Part2("""[1,"red",5]""", 6);
        Part2(File.ReadAllText("input.txt"));
    }

    private static void Part1(string s, int? expected = null)
    {
        if (expected.HasValue)
            Console.Write($"Part 1: '{s}' -> {SumNumbers(s)} (expected: {expected})");
        else
            Console.WriteLine($"Part 1: {SumNumbers(s)}");
    }

    private static int SumNumbers(string s)
    {
        bool isReadingNumber = false;
        int currentNumberStart = 0;
        int currentNumberLength = 0;
        int result = 0;
        
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (!isReadingNumber && c.IsDigit() || c is '-')
            {
                isReadingNumber = true;
                currentNumberStart = i;
                currentNumberLength = 1;
                continue;
            }

            if (isReadingNumber && c.IsDigit())
            {
                currentNumberLength++;
                continue;
            }

            if (isReadingNumber && !c.IsDigit())
            {
                string number = s.Substring(currentNumberStart, currentNumberLength);
                result += int.Parse(number);
                isReadingNumber = false;
                currentNumberStart = 0;
                currentNumberLength = 0;
            }
        }
        
        return result;
    }

    private static void Part2(string s, int? expected = null)
    {
        if (expected.HasValue)
            Console.WriteLine($"Part 2: '{s}' -> {SumJson(s)} (expected: {expected})");
        else
            Console.WriteLine($"Part 2: {SumJson(s)}");
    }

    private static int SumJson(string s) => SumJson(s.AsSpan());

    private static int SumJson(ReadOnlySpan<char> span)
    {
        int result = 0;
        HashSet<JsonValue> expected = [JsonValue.StartArray, JsonValue.StartObject];
        bool isReadingObject = false;
        
        for (int i = 0; i < span.Length; i++)
        {
            JsonValue current = span[i].AsJsonValue();
            string nearby = $"{span[..Math.Min(i + 10, span.Length)]}\n{(i > 0 ? new string('-', i) : "")}^";

            if (!expected.Contains(current))
                throw new Exception($"Found {current} at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");

            if (current is JsonValue.EndArray or JsonValue.EndObject)
            {
                break;
            }
            
            if (current is JsonValue.Comma && isReadingObject)
            {
                expected = [JsonValue.Quote];
                continue;
            }
            
            if (current is JsonValue.Comma && !isReadingObject)
            {
                expected = [JsonValue.StartArray, JsonValue.StartObject];
                continue;
            }

            if (current is JsonValue.Quote && isReadingObject)
            {
                _ = ReadString(span[(i + 1)..], out int len);
                i += len;
                expected = [JsonValue.Colon];
                continue;
            }

            if (current is JsonValue.Quote && !isReadingObject)
                throw new Exception($"Found {current} while reading object  at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");

            if (current is JsonValue.Colon && isReadingObject)
            {
                expected = [JsonValue.StartArray, JsonValue.StartObject];
                continue;
            }
            
            if (current is JsonValue.Colon && !isReadingObject)
            {
                throw new Exception($"Found {current} while reading object at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");
            }
            
            if (current is JsonValue.StartObject)
            {
                isReadingObject = true;
                result += SumObject(span[(i + 1)..], out int len);
                i += len; // incremented by loop
                expected = [JsonValue.EndObject, JsonValue.Comma];
                continue;
            }

            if (current is JsonValue.StartArray)
            {
                result += SumArray(span[(i + 1)..], out int len);
                i += len; // incremented by loop
                expected = [JsonValue.EndArray, JsonValue.Comma];
                continue;
            }
        }

        return result;
    }

    private static int SumObject(ReadOnlySpan<char> span, out int length)
    {
        int result = 0;
        length = 0;
        bool expectPropertyName = true;
        bool ignoreEntireObject = false;
        HashSet<JsonValue> expected = [JsonValue.Quote, JsonValue.EndObject];

        for (int i = 0; i < span.Length; i++)
        {
            JsonValue current = span[i].AsJsonValue();
            string nearby = $"{span[..Math.Min(i + 10, span.Length)]}\n{(i > 0 ? new string('-', i) : "")}^";

            if (!expected.Contains(current))
                throw new Exception($"Found {current} at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");

            if (current is JsonValue.Comma)
            {
                length++;
                expected = [JsonValue.Quote, JsonValue.EndObject];
                expectPropertyName = true;
                continue;
            }
            
            if (current is JsonValue.EndObject)
            {
                length++;
                break;
            }

            if (current is JsonValue.Quote && expectPropertyName)
            {
                length++; // opening quote
                _ = ReadString(span[(i + 1)..], out int len);
                length += len;
                i += len;
                expected = [JsonValue.Colon];
                expectPropertyName = false;
                continue;
            }

            if (current is JsonValue.Colon)
            {
                length++;
                expected = [JsonValue.Digit, JsonValue.Dash, JsonValue.Quote, JsonValue.StartArray, JsonValue.StartObject];
                continue;
            }
            
            if (current is JsonValue.Quote && !expectPropertyName)
            {
                length++; // opening quote
                string stringValue = ReadString(span[(i + 1)..], out int len);
                if (stringValue == "red")
                    ignoreEntireObject = true;
                length += len;
                i += len;
                expected = [JsonValue.Comma, JsonValue.EndObject];
                expectPropertyName = false;
                continue;
            }

            if (current is JsonValue.StartArray)
            {
                length++; // opening brackets
                result += SumArray(span[(i + 1)..], out int len);
                length += len;
                i += len;
                expected = [JsonValue.Comma, JsonValue.EndObject];
                expectPropertyName = false;
                continue;
            }

            if (current is JsonValue.Digit or JsonValue.Dash)
            {
                int n = ReadNumber(span[i..], out int len);
                result += n;
                length += len;
                i += len - 1; // has no 'end' character
                expected = [JsonValue.Comma, JsonValue.EndObject];
                continue;
            }
            
            if (current is JsonValue.StartObject)
            {
                length++; // opening curly braces
                int n = SumObject(span[(i + 1)..], out int len);
                result += n;
                length += len;
                i += len;
                expected = [JsonValue.Comma, JsonValue.EndObject];
                continue;
            }
        }
        
        return ignoreEntireObject ? 0 : result;
    }

    private static int SumArray(ReadOnlySpan<char> span, out int length)
    {
        int result = 0;
        length = 0;
        HashSet<JsonValue> expected = [JsonValue.StartArray, JsonValue.Digit, JsonValue.Dash, JsonValue.EndArray, JsonValue.Quote, JsonValue.StartObject];

        for (int i = 0; i < span.Length; i++)
        {
            JsonValue current = span[i].AsJsonValue();
            string nearby = $"{span[..Math.Min(i + 10, span.Length)]}\n{(i > 0 ? new string('-', i) : "")}^";

            if (!expected.Contains(current))
                throw new Exception($"Found {current} at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");

            if (current is JsonValue.StartArray)
            {
                length++; // opening bracket
                result += SumArray(span[(i + 1)..], out int len);
                length += len;
                i += len;
                expected = [JsonValue.Comma, JsonValue.EndArray];
                continue;
            }

            if (current is JsonValue.StartObject)
            {
                length++; // opening curly braces
                result += SumObject(span[(i + 1)..], out int len);
                length += len;
                i += len; // incremented by loop
                expected = [JsonValue.EndArray, JsonValue.Comma];
                continue;
            }

            if (current is JsonValue.Comma)
            {
                length++;
                expected = [JsonValue.Digit, JsonValue.Dash, JsonValue.StartArray, JsonValue.Quote, JsonValue.StartObject];
                continue;
            }

            if (current is JsonValue.Digit or JsonValue.Dash)
            {
                int n = ReadNumber(span[i..], out int len);
                result += n;
                length += len;
                i += len - 1; // has no 'end' character
                expected = [JsonValue.Comma, JsonValue.EndArray];
                continue;
            }

            if (current is JsonValue.Quote)
            {
                length++; // opening quote
                _ = ReadString(span[(i+1)..], out int len);
                length += len;
                i += len;
                expected = [JsonValue.Comma, JsonValue.EndArray];
                continue;
            }

            if (current is JsonValue.EndArray)
            {
                length++;
                break;
            }
        }
        
        return result;
    }

    private static int ReadNumber(ReadOnlySpan<char> span, out int length)
    {
        int startIndex = -1;
        length = 0;
        HashSet<JsonValue> expected = [JsonValue.Digit, JsonValue.Dash];
        HashSet<JsonValue> digitValues = [JsonValue.Digit, JsonValue.Dash];

        for (int i = 0; i < span.Length; i++)
        {
            JsonValue current = span[i].AsJsonValue();
            string nearby = $"{span[..Math.Min(i + 10, span.Length)]}\n{(i > 0 ? new string('-', i) : "")}^";
            
            if (!expected.Contains(current))
                throw new Exception($"Found {current} at {i} near\n{nearby}\nbut expected one of {string.Join(", ", expected)}.");
            
            if (digitValues.Contains(current))
            {
                if (startIndex < 0)
                    startIndex = i;
                length++;
                expected = [JsonValue.Digit, JsonValue.Comma, JsonValue.EndArray, JsonValue.EndObject];
                digitValues = [JsonValue.Digit];
                continue;
            }
            
            string s = span[startIndex..i].ToString();
            int result = int.Parse(s);
            return result;
        }

        throw new InvalidOperationException();
    }

    private static string ReadString(ReadOnlySpan<char> span, out int length)
    {
        int startIndex = -1;
        length = 0;

        for (int i = 0; i < span.Length; i++)
        {
            JsonValue current = span[i].AsJsonValue();
            string nearby = $"{span[..Math.Min(i + 10, span.Length)]}\n{(i > 0 ? new string('-', i) : "")}^";

            if (current is not JsonValue.Quote)
            {
                if (startIndex < 0)
                    startIndex = i;
                length++;
                continue;
            }
            
            length++;
            string s = startIndex >= 0 ? span[startIndex..i].ToString() : string.Empty;
            return s;
        }

        throw new InvalidOperationException();
    }

    private static JsonValue AsJsonValue(this char c)
    {
        return c switch
        {
            '[' => JsonValue.StartArray,
            ']' => JsonValue.EndArray,
            '{' => JsonValue.StartObject,
            '}' => JsonValue.EndObject,
            ',' => JsonValue.Comma,
            '"' => JsonValue.Quote,
            ':' => JsonValue.Colon,
            '-' => JsonValue.Dash,
            >= '0' and <= '9' => JsonValue.Digit,
            _ => JsonValue.Other
        };
    }

    private enum JsonValue
    {
        Other,
        StartArray,
        EndArray,
        StartObject,
        EndObject,
        Comma,
        Quote,
        Colon,
        Dash,
        Digit
    }
}