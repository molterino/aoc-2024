using System.Text.RegularExpressions;

internal static class Program
{
    private static void Main(string[] args)
    {
        SolvePart1();
        SolvePart2();
    }

    private static void SolvePart1()
    {
        const string multiplyCommandPattern = @"mul\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)";

        var memory = ReadMemory();
        var result = CalculateFromCorruptedMemory(memory, multiplyCommandPattern);

        Console.WriteLine($"Multiplications result from bad memory (part 1): {result}");
    }

    private static void SolvePart2()
    {
        const string enabledInstructionsPattern = @"do\(\)([\s\S]*?)don't\(\)";
        const string multiplyCommandPattern = @"mul\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)";

        var memory = "do()" + ReadMemory() + "don't()";
        var result = 0;

        foreach (Match enabledInstruction in Regex.Matches(memory, enabledInstructionsPattern))
        {
            var memorySegment = enabledInstruction.Groups[0].Value;
            result += CalculateFromCorruptedMemory(memorySegment, multiplyCommandPattern);
        }

        Console.WriteLine($"Only enabled multiplications result from bad memory (part 2): {result}");
    }

    private static string ReadMemory()
    {
        const string path = "input.txt";

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

    private static int CalculateFromCorruptedMemory(string memorySegment, string instructionsPattern)
    {
        int result = 0;

        foreach (Match instruction in Regex.Matches(memorySegment, instructionsPattern))
        {
            var x = int.Parse(instruction.Groups[1].Value);
            var y = int.Parse(instruction.Groups[2].Value);
            var mul = x * y;

            result += mul;
        }

        return result;
    }
}