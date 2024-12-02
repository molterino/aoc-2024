
internal static class Program
{
    private static void Main(string[] args)
    {
        const string path = "input.txt";
        const string separator = " ";

        var safeReports = 0;
        var tolaretedSafeReports = 0;

        foreach (var line in ReadReports(path))
        {
            var levels = line.Split(separator).ToList();

            var safeReport = VerifyReport(levels);
            if (safeReport)
            {
                safeReports++;
            }
            else
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    var toleratedLevels = levels.Where((value, index) => index != i).ToList();

                    var tolaretedSafeReport = VerifyReport(toleratedLevels);
                    if (tolaretedSafeReport)
                    {
                        tolaretedSafeReports++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"Safe levels (part 1): {safeReports}");
        Console.WriteLine($"Safe levels with tolerance (part 2): {safeReports + tolaretedSafeReports}");
    }

    private static string[] ReadReports(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during file processing: {e.Message}");
            return Array.Empty<string>();
        }
    }

    private static bool VerifyReport(List<string> levels)
    {
        var increasingCount = 0;
        var decreasingCount = 0;
        var safeCount = 0;

        for (int i = 0; i < levels.Count - 1; i++)
        {
            var currentLevel = int.Parse(levels[i]);
            var nextLevel = int.Parse(levels[i + 1]);

            var difference = Math.Abs(currentLevel - nextLevel);
            if (difference >= 1 && difference <= 3)
            {
                safeCount++;
            }

            if (currentLevel > nextLevel)
            {
                increasingCount++;
            }
            else
            {
                decreasingCount++;
            }
        }

        var allIncreasingOrDecreasing = increasingCount == 0 || decreasingCount == 0;
        var allLevelSafe = safeCount == levels.Count - 1;

        return allIncreasingOrDecreasing && allLevelSafe;
    }
}