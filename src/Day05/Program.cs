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

            foreach (var update in updates)
            {
                var correctlyOrderedUpdate = true;
                for (int i = 0; i < update.PageNumbers.Count - 1; i++)
                {
                    for (int j = i + 1; j < update.PageNumbers.Count; j++)
                    {
                        var pageToPrintFirst = update.PageNumbers[i];
                        var pageToPrintAfter = update.PageNumbers[j];
                        var properRules = rules.Count(x => x.PageToPrintFirst == pageToPrintFirst && x.PageToPrintAfter == pageToPrintAfter);
                        var violatedRules = rules.Count(x => x.PageToPrintFirst == pageToPrintAfter && x.PageToPrintAfter == pageToPrintFirst);

                        if (properRules == 0 && violatedRules != 0)
                        {
                            correctlyOrderedUpdate = false;
                        }
                    }
                }

                if (correctlyOrderedUpdate)
                {
                    middlePageNumbersSum += update.MiddlePage;
                }
            }

            Console.WriteLine($"Summary of the middles page numbers for the correctly orderes updates: {middlePageNumbersSum}");
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

        private static List<Rule> ParseRules(string[] printJob)
        {
            var tasks = printJob[0].Split(Environment.NewLine);
            var rules = new List<Rule>();

            foreach (var line in tasks)
            {
                var pages = line.Split("|");
                var rule = new Rule
                {
                    PageToPrintFirst = int.Parse(pages[0]),
                    PageToPrintAfter = int.Parse(pages[1])
                };
                rules.Add(rule);
            }

            return rules;
        }

        private static List<Update> ParseUpdates(string[] printJob)
        {
            var tasks = printJob[1].Split(Environment.NewLine);
            var updates = new List<Update>();

            foreach (var line in tasks)
            {
                var pages = line.Split(",");
                var update = new Update
                {
                    PageNumbers = Array.ConvertAll(pages, int.Parse).ToList()
                };
                updates.Add(update);
            }

            return updates;
        }
    }
}