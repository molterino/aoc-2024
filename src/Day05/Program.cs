namespace Day05
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var printJob = ReadPrintJob();
            var rules = ParseRules(printJob);
            var updates = ParseUpdates(printJob);

            var middlePageNumbersSum = 0;
            var sortedMiddlePageNumbersSum = 0;

            var invalidUpdates = new List<List<int>>();

            // Part 1
            foreach (var update in updates)
            {
                var validUpdate = true;
                for (int i = 0; i < update.Count - 1; i++)
                {
                    for (int j = i + 1; j < update.Count; j++)
                    {
                        var firstPage = update[i];
                        var nextPage = update[j];
                        var properRules = rules.Count(x => x.Item1 == firstPage && x.Item2 == nextPage);
                        var violatedRules = rules.Count(x => x.Item1 == nextPage && x.Item2 == firstPage);

                        if (properRules == 0 && violatedRules != 0)
                        {
                            validUpdate = false;
                        }
                    }
                }

                if (validUpdate)
                {
                    middlePageNumbersSum += GetMiddlePageNumber(update);
                } else
                {
                    invalidUpdates.Add(update);
                }
            }

            Console.WriteLine($"Summary of the middle page numbers for the correctly ordered updates (Part1): {middlePageNumbersSum}"); //4637

            // Part 2
            foreach (var update in invalidUpdates)
            {
                var relevantRules = rules
                    .Where(x => update.Contains(x.Item1) && update.Contains(x.Item2))
                    .ToList();

                var sortedUpdates = TopologicalSort(update, relevantRules);
                if (sortedUpdates != null)
                {
                    sortedMiddlePageNumbersSum += GetMiddlePageNumber(sortedUpdates);
                }
            }

            Console.WriteLine($"Summary of the middles page numbers for the incorrectly ordered but sorted updates (Part2): {sortedMiddlePageNumbersSum}"); //6370
        }

        private static string[] ReadPrintJob()
        {
            //const string path = "input-template.txt";
            const string path = "input.txt";
            string[] sections;

            try
            {
                var data = File.ReadAllText(path);
                sections = data.Split(Environment.NewLine + Environment.NewLine);
                return sections;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during file processing: {e.Message}");
                return [];
            }
        }

        private static List<(int, int)> ParseRules(string[] printJob)
        {
            var tasks = printJob[0].Split(Environment.NewLine);
            var rules = new List<(int, int)>();

            foreach (var line in tasks)
            {
                var pages = line.Split("|");
                var rule = (int.Parse(pages[0]), int.Parse(pages[1]));
                rules.Add(rule);
            }

            return rules;
        }

        private static List<List<int>> ParseUpdates(string[] printJob)
        {
            var tasks = printJob[1].Split(Environment.NewLine);
            var updates = new List<List<int>>();

            foreach (var line in tasks)
            {
                var pages = line.Split(",");
                var update = Array.ConvertAll(pages, int.Parse).ToList();
                updates.Add(update);
            }

            return updates;
        }

        private static int GetMiddlePageNumber(List<int> pages)
        {
            var middleIndex = pages.Count / 2;
            return pages[middleIndex];
        }

        // Kahn’s algorithm for Topological Sorting
        private static List<int>? TopologicalSort(List<int> pages, List<(int first, int next)> rules)
        {
            // Init
            var graph = new Dictionary<int, List<int>>();
            var indegree = new Dictionary<int, int>();

            foreach (var page in pages)
            {
                graph[page] = [];
                indegree[page] = 0;
            }

            // Process rules to create edges and update indegree
            foreach (var (first, next) in rules)
            {
                graph[first].Add(next);
                indegree[next]++;
            }

            // Find nodes with 0 indegree
            var queue = new Queue<int>(pages.Where(page => indegree[page] == 0));
            var sortedList = new List<int>();

            // Topological sort
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                sortedList.Add(current);

                foreach (var neighbor in graph[current])
                {
                    indegree[neighbor]--;
                    if (indegree[neighbor] == 0)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // Check for cycles
            if (sortedList.Count != pages.Count)
            {
                return null;
            }

            return sortedList;
        }
    }
}
