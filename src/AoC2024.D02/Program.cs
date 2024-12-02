const string path = "input.txt";
const string separator = " ";

try
{
    var safeLevels = 0;

    foreach (var line in File.ReadAllLines(path))
    {
        var levels = line.Split(separator);
        var increasingCount = 0;
        var safeCount = 0;

        for (int i = 0; i < levels.Length - 1; i++)
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
        }

        var allIncreasingOrDecreasing = increasingCount == levels.Length - 1 || increasingCount == 0;
        var allLevelSafe = safeCount == levels.Length - 1;

        if (allIncreasingOrDecreasing && allLevelSafe)
        {
            safeLevels++;
        }
    }

    Console.WriteLine($"Safe levels (part 1): {safeLevels}");
}
catch (Exception e)
{
    Console.WriteLine($"Error during file processing: {e.Message}");
}
