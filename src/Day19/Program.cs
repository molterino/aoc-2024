namespace Day19
{
    public static class Program
    {
        private const string Path = "input.txt"; // 276
        //private const string Path = "input-template.txt"; // 6

        private static void Main(string[] args)
        {
            var patterns = GetPatterns(Path);
            var designs = GetDesigns(Path);
            var cache = new Dictionary<string, bool>();

            var validDesignsCount = designs.Count(design => CanConstruct(design, patterns, cache));
            Console.WriteLine($"{validDesignsCount} designs are possible");
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

        static bool CanConstruct(string design, List<string> patterns, Dictionary<string, bool> cache)
        {
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
                    if (CanConstruct(remaining, patterns, cache))
                    {
                        cache[design] = true;
                        return true;
                    }
                }
            }

            cache[design] = false;
            return false;
        }
    }
}
