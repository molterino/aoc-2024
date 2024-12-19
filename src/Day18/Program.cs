namespace Day18
{
    public static class Program
    {
        private const string Path = "input.txt"; // 70x70
        //private const string Path = "input-template.txt"; // 6x6

        private const char Safe = '.';
        private const char Corrupted = '#';

        private static int MemoryDimension;
        //private static int CorruptedMemorySectionsLimit = 12;
        private static int CorruptedMemorySectionsLimit = 1024;
        private static (int x, int y) Start;
        private static (int x, int y) Exit;

        private static void Main(string[] args)
        {
            var memory = ReadMemory(Path);
            var result = MazeSolver.FindShortestPath(memory);

            DisplayRoute(memory);
            Console.WriteLine($"Shortest path is {result}"); // 370
        }

        private static char[,] ReadMemory(string path)
        {
            var data = File.ReadAllLines(path);
            var corruptedMemorySections = new List<(int x, int y)>();

            foreach (var line in data)
            {
                var coordinates = line.Split(',');
                var x = int.Parse(coordinates[0]);
                var y = int.Parse(coordinates[1]);
                corruptedMemorySections.Add((x, y));
            }

            MemoryDimension = corruptedMemorySections.Max(m => Math.Max(m.x, m.y)) + 1;
            corruptedMemorySections = corruptedMemorySections.Take(CorruptedMemorySectionsLimit).ToList();
            var memory = new char[MemoryDimension, MemoryDimension];

            for (int y = 0; y < MemoryDimension; y++)
            {
                for (int x = 0; x < MemoryDimension; x++)
                {
                    if (corruptedMemorySections.Contains((x, y)))
                    {
                        memory[x, y] = Corrupted;
                    }
                    else
                    {
                        memory[x, y] = Safe;
                    }
                }
            }

            Start = (0, 0);
            Exit = (MemoryDimension - 1, MemoryDimension - 1);

            MazeSolver.StartPosition = Start;
            MazeSolver.EndPostion = Exit;

            return memory;
        }

        private static void DisplayRoute(char[,] maze)
        {
            for (int y = 0; y < MemoryDimension; y++)
            {
                for (int x = 0; x < MemoryDimension; x++)
                {
                    var currentField = maze[x, y];

                    if (currentField == MazeSolver.Corrupted) Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (currentField == MazeSolver.Path) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(maze[x, y]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
