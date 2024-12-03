
using System.Text.RegularExpressions;

internal static class Program
{
    private static void Main(string[] args)
    {
        const string path = "input.txt";

        string pattern = @"mul\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)";
        string memory = ReadMemory(path);
        int result = 0;

        foreach (Match match in Regex.Matches(memory, pattern))
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            var mul = x * y;
            result += mul;
        }

        Console.WriteLine(result);
    }

    private static string ReadMemory(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during file processing: {e.Message}");
            return string.Empty;
        }
    }
}