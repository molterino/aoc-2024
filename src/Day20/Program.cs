namespace Day20
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";

        private const char StartToken = 'S';
        private const char EndToken= 'E';
        private const char Track = '.';

        private static (int x, int y) _startPosition;
        private static (int x, int y) _endPosition;

        private static void Main(string[] args)
        {
            var raceTrack = ReadRaceTrack(Path);
            var mazeSolverWithoutCheats = new MazeSolver(raceTrack, _startPosition, _endPosition);
            var picosecondsWithoutCheats = mazeSolverWithoutCheats.FindShortestPath();

            var savedPicoseconds = new Dictionary<int, int>();
            var rows = raceTrack.GetLength(0);
            var cols = raceTrack.GetLength(1);

            for (int y = 1; y < cols - 1; y++) // skip outer walls
            {
                for (int x = 1; x < rows - 1; x++) // skip outer walls
                {
                    var shortcutPosition = (x, y);
                    var shortcut = raceTrack[shortcutPosition.x, shortcutPosition.y];

                    if (shortcut == Track)
                    {
                        continue;
                    }

                    var raceTrackWithShortcut = CreateRaceTrackWithShortcut(raceTrack, shortcutPosition);
                    var mazeSolver = new MazeSolver(raceTrackWithShortcut, _startPosition, _endPosition);
                    var picoseconds = mazeSolver.FindShortestPath();

                    if (picoseconds >= picosecondsWithoutCheats)
                    {
                        continue;
                    }

                    var savedPicosecond = picosecondsWithoutCheats - picoseconds;
                    if (savedPicoseconds.TryGetValue(savedPicosecond, out var count))
                    {
                        savedPicoseconds[savedPicosecond] = count + 1;
                    }
                    else
                    {
                        savedPicoseconds[savedPicosecond] = 1;
                    }
                }
            }

            Console.WriteLine($"The fastest way without cheats is {picosecondsWithoutCheats} picoseconds.\n");
            foreach (var result in savedPicoseconds.OrderBy(x => x.Key))
            {
                Console.WriteLine($"There are {result.Value} cheats that save {result.Key} picoseconds.");
            }

            var numberOfCheatsSavedMoreThan100Picoseconds = savedPicoseconds.Where(x => x.Key >= 100).Sum(x => x.Value);
            Console.WriteLine($"\nThere are {numberOfCheatsSavedMoreThan100Picoseconds} cheats that save at least 100 picoseconds."); // 1367
        }

        private static char[,] ReadRaceTrack(string path)
        {
            var data = File.ReadAllLines(path);
            var rows = data.Length;
            var cols = data[0].Length;
            var map = new char[rows, cols];

            for (int y = 0; y < cols; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    var field = data[y][x];

                    if (field == StartToken)
                    {
                        _startPosition = (x, y);
                    }
                    else if (field == EndToken)
                    {
                        _endPosition = (x, y);
                    }

                    map[x, y] = field;
                }
            }

            return map;
        }

        private static char[,] CreateRaceTrackWithShortcut(char[,] raceTrack, (int, int) shortcutPosition)
        {
            var raceTrackWithShortcut = raceTrack.Clone() as char[,];
            raceTrackWithShortcut[shortcutPosition.Item1, shortcutPosition.Item2] = Track;

            return raceTrackWithShortcut;
        }

        private static void Display(char[,] maps)
        {
            Console.Clear();

            var rows = maps.GetLength(0);
            var cols = maps.GetLength(1);

            for (int y = 0; y < cols; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Console.Write(maps[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
