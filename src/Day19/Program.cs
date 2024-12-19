namespace Day19
{
    public static class Program
    {
        private const string Path = "input.txt"; // 276 / 681226908011510
        //private const string Path = "input-template.txt"; // 6 / 14

        private static void Main(string[] args)
        {
            var patterns = GetPatterns(Path);
            var designs = GetDesigns(Path);

            int validDesignsCount = designs.Count(design => ValidateDesign(design, patterns));
            Console.WriteLine($"{validDesignsCount} designs are possible");

            long validDesignsVariantsCount = designs.Sum(design => CountDesignVariants(design, patterns));
            Console.WriteLine($"{validDesignsVariantsCount} design variants are possible");
        }

        private static List<string> GetPatterns(string path)
        {
            var data = File.ReadAllLines(path);
            var patterns = data[0].Split(", ").ToList();

            return patterns;
        }

        private static List<string> GetDesigns(string path)
        {
            var data = File.ReadAllLines(path);
            var designs = data.Skip(2).ToList();

            return designs;
        }

        private static bool ValidateDesign(string design, List<string> patterns, Dictionary<string, bool>? cache = null)
        {
            if (cache == null)
            {
                cache = [];
            }

            if (cache.TryGetValue(design, out bool value))
            {
                return value;
            }

            if (design.Length == 0)
            {
                return true;
            }

            foreach (string pattern in patterns)
            {
                if (design.StartsWith(pattern))
                {
                    var remaining = design.Substring(pattern.Length);
                    if (ValidateDesign(remaining, patterns, cache))
                    {
                        cache[design] = true;
                        return true;
                    }
                }
            }

            cache[design] = false;
            return false;
        }

        private static long CountDesignVariants(string design, List<string> patterns, Dictionary<string, long>? cache = null)
        {
            if (cache == null)
            {
                cache = [];
            }

            if (cache.TryGetValue(design, out long value))
            {
                return value;
            }

            if (design.Length == 0)
            {
                return 1;
            }

            long totalCount = 0;

            foreach (string word in patterns)
            {
                if (design.StartsWith(word))
                {
                    string remaining = design.Substring(word.Length);
                    totalCount += CountDesignVariants(remaining, patterns, cache);
                }
            }

            cache[design] = totalCount;
            return totalCount;
        }
    }
}
