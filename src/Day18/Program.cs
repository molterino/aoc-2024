namespace Day18
{
    public static class Program
    {
        private const string Path = "input.txt"; // 70x70
        //private const string Path = "input-template.txt"; // 6x6, 22

        private const char Safe = '.';
        private const char Corrupted = '#';

        private static int _memorySize;
        //private static int _corruptedMemoryBytes = 12;
        private static int _corruptedMemoryBytes = 1024;
        private static (int x, int y) _startPosition;
        private static (int x, int y) _exitPosition;

        private static void Main(string[] args)
        {
            var shortestPathLength = int.MaxValue;
            var corruptedMemoryBytes = ReadCorruptedMemoryBytes(Path);
            CalculateMemorySize(corruptedMemoryBytes);
            var lastPassableMemoryState = new char[_memorySize, _memorySize];

            for (int i = _corruptedMemoryBytes; i < corruptedMemoryBytes.Count; i++)
            {
                var activeCorruptedMemoryBytes = corruptedMemoryBytes.Take(i).ToList();
                var latestCorruptedBytePosition = (activeCorruptedMemoryBytes[^1].x, activeCorruptedMemoryBytes[^1].y);

                var memory = ReadMemory(activeCorruptedMemoryBytes);
                var pathLength = MazeSolver.FindShortestPath(memory);

                Console.WriteLine($"CorruptedMemoryBytes: {activeCorruptedMemoryBytes.Count}, ShortestPath: {pathLength}, LatestCorruptedMemoryPosition: {latestCorruptedBytePosition}");

                if (pathLength == -1)
                {
                    Console.WriteLine($"\nShortest path: {shortestPathLength}"); // 370
                    Console.WriteLine($"Corrupted memory byte that made path impassable: {latestCorruptedBytePosition}"); // 65,6
                    Console.WriteLine("Last pasasble memory state:\n");
                    DisplayRoute(lastPassableMemoryState);

                    break;
                }

                if (pathLength < shortestPathLength)
                {
                    shortestPathLength = pathLength;
                }

                lastPassableMemoryState = memory.Clone() as char[,];
            }
        }

        private static List<(int x, int y)> ReadCorruptedMemoryBytes(string path)
        {
            var data = File.ReadAllLines(path);
            var corruptedMemorySections = new List<(int, int)>();

            foreach (var line in data)
            {
                var coordinates = line.Split(',');
                var x = int.Parse(coordinates[0]);
                var y = int.Parse(coordinates[1]);
                corruptedMemorySections.Add((x, y));
            }

            return corruptedMemorySections;
        }

        private static void CalculateMemorySize(List<(int x, int y)> corruptedMemorySections)
        {
            _memorySize = corruptedMemorySections.Max(m => Math.Max(m.x, m.y)) + 1;
        }

        private static char[,] ReadMemory(List<(int x, int y)> corruptedMemorySections)
        {
            var memory = new char[_memorySize, _memorySize];
            for (int y = 0; y < _memorySize; y++)
            {
                for (int x = 0; x < _memorySize; x++)
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

            _startPosition = (0, 0);
            _exitPosition = (_memorySize - 1, _memorySize - 1);

            MazeSolver.StartPosition = _startPosition;
            MazeSolver.EndPostion = _exitPosition;

            return memory;
        }

        private static void DisplayRoute(char[,] maze)
        {
            for (int y = 0; y < _memorySize; y++)
            {
                for (int x = 0; x < _memorySize; x++)
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
