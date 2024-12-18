namespace Day15
{
    public static class Part2
    {
        const char Robot = '@';
        const char Wall = '#';
        const char Box = 'O';
        const char BoxLeft = '[';
        const char BoxRight = ']';
        const char FreeSpace = '.';

        public static void Run()
        {
            var path = "input.txt"; // 1471049
            //var path = "input-template-1.txt";
            //var path = "input-template-2.txt";
            //var path = "input-template-3.txt";
            var warehouseDocument = File.ReadAllLines(path);

            // init widened map
            var warehouseMapData = warehouseDocument
                .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
            var cols = warehouseMapData[0].Length * 2;
            var rows = warehouseMapData.Count;
            var map = new char[rows, cols];

            for (int x = 0; x < rows; x++)
            {
                var row = warehouseMapData[x]
                    .Replace(Wall.ToString(), string.Empty + Wall + Wall)
                    .Replace(Box.ToString(), string.Empty +BoxLeft + BoxRight)
                    .Replace(FreeSpace.ToString(), string.Empty + FreeSpace + FreeSpace)
                    .Replace(Robot.ToString(), string.Empty + Robot + FreeSpace)
                    .ToCharArray();

                for (int y = 0; y < cols; y++)
                {
                    map[x, y] = row[y];
                }
            }

            Console.WriteLine("Initial state:");
            Display(map);
            Console.WriteLine("\nPress any key to start to robot...");
            Console.ReadKey();

            // init robot
            var robotPositionX = 0;
            var robotPositionY = 0;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    if (map[x, y] == Robot)
                    {
                        robotPositionX = x;
                        robotPositionY = y;
                        break;
                    }
                }
            }

            // init commands
            var robotCommandsData = warehouseDocument.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).ToList();
            var commands = string.Concat(robotCommandsData).Replace(Environment.NewLine, "");

            // init movements
            char[] robotMovementCommands = ['^', 'v', '<', '>'];
            int[] robotMovementX = [-1, 1, 0, 0];
            int[] robotMovementY = [0, 0, -1, 1];

            // move robot
            foreach (var command in commands)
            {
                var commandIndex = Array.IndexOf(robotMovementCommands, command);
                var nextPositionX = robotPositionX + robotMovementX[commandIndex];
                var nextPositionY = robotPositionY + robotMovementY[commandIndex];
                var nextPosition = map[nextPositionX, nextPositionY];

                if (command == '<')
                {
                    if (nextPosition == FreeSpace)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[robotPositionX, nextPositionY] = Robot;
                        robotPositionY = nextPositionY;
                    }
                    else if (nextPosition == BoxRight)
                    {
                        var afterNextPositionY = nextPositionY - 2;
                        var afterNextPosition = map[robotPositionX, afterNextPositionY];

                        var boxCounter = 1;

                        while (afterNextPosition != Wall)
                        {
                            if (afterNextPosition == FreeSpace)
                            {
                                map[robotPositionX, robotPositionY] = FreeSpace;
                                map[robotPositionX, nextPositionY] = Robot;

                                for (int i = 0; i < boxCounter; i++)
                                {
                                    map[robotPositionX, nextPositionY - 2 - (2 * i)] = BoxLeft;
                                    map[robotPositionX, nextPositionY - 1 - (2 * i)] = BoxRight;
                                }

                                robotPositionY = nextPositionY;
                                break;
                            }

                            boxCounter++;
                            afterNextPositionY += -2;
                            afterNextPosition = map[robotPositionX, afterNextPositionY];
                        }
                    }
                }
                else if (command == '>')
                {
                    if (nextPosition == FreeSpace)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[robotPositionX, nextPositionY] = Robot;
                        robotPositionY = nextPositionY;
                    }
                    else if (nextPosition == BoxLeft)
                    {
                        var afterNextPositionY = nextPositionY + 2;
                        var afterNextPosition = map[robotPositionX, afterNextPositionY];

                        var boxCounter = 1;

                        while (afterNextPosition != Wall)
                        {
                            if (afterNextPosition == FreeSpace)
                            {
                                map[robotPositionX, robotPositionY] = FreeSpace;
                                map[robotPositionX, nextPositionY] = Robot;

                                for (int i = 0; i < boxCounter; i++)
                                {
                                    map[robotPositionX, nextPositionY + 1 + (2 * i)] = BoxLeft;
                                    map[robotPositionX, nextPositionY + 2 + (2 * i)] = BoxRight;
                                }

                                robotPositionY = nextPositionY;
                                break;
                            }

                            boxCounter++;
                            afterNextPositionY += 2;
                            afterNextPosition = map[robotPositionX, afterNextPositionY];
                        }
                    }
                }
                else if (command == '^')
                {
                    var movable = TryMoveVertically(map, robotPositionX, robotPositionY, -1);
                    if (movable)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[nextPositionX, robotPositionY] = Robot;

                        robotPositionX = nextPositionX;
                    }
                }
                else if (command == 'v')
                {
                    var movable = TryMoveVertically(map, robotPositionX, robotPositionY, 1);

                    if (movable)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[nextPositionX, robotPositionY] = Robot;

                        robotPositionX = nextPositionX;
                    }
                }
            }

            Console.WriteLine("\nFinal state:");
            Display(map);

            var gps = CalculateGPS(map);
            Console.WriteLine($"\nGPS: {gps}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void Display(char[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static int CalculateGPS(char[,] map)
        {
            var gps = 0;

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '[')
                    {
                        gps += (100 * i) + j;
                    }
                }
            }

            return gps;
        }

        private static bool TryMoveVertically(char[,] map, int x, int y, int direction)
        {
            char nextField = map[x + direction, y];

            if (nextField == Wall)
            {
                return false;
            }

            if (nextField == FreeSpace)
            {
                map[x + direction, y] = Robot;
                map[x, y] = FreeSpace;

                return true;
            }

            if (nextField == BoxLeft || nextField == BoxRight)
            {
                var boxGroup = GetBoxGroup(map, x + direction, y, direction);
                var isBoxGroupMovable = MoveBoxGroup(map, boxGroup, direction);

                if (isBoxGroupMovable)
                {
                    map[x + direction, y] = Robot;
                    map[x, y] = FreeSpace;

                    return true;
                }
            }

            return false;
        }

        private static List<(int, int)> GetBoxGroup(char[,] map, int x, int y, int direction)
        {
            var queue = new Queue<(int, int)>();
            var visited = new List<(int, int)>();
            var boxGroup = new List<(int, int)>();

            queue.Enqueue((x, y));

            while (queue.Count > 0)
            {
                var (currentRow, currentCol) = queue.Dequeue();

                if (visited.Contains((currentRow, currentCol)))
                {
                    continue;
                }

                visited.Add((currentRow, currentCol));
                boxGroup.Add((currentRow, currentCol));

                if (map[currentRow, currentCol - 1] == BoxLeft)
                {
                    queue.Enqueue((currentRow, currentCol - 1));
                }

                if (map[currentRow, currentCol + 1] == BoxRight)
                {
                    queue.Enqueue((currentRow, currentCol + 1));
                }

                if (map[currentRow + direction, currentCol] == BoxLeft || map[currentRow + direction, currentCol] == BoxRight)
                {
                    queue.Enqueue((currentRow + direction, currentCol));
                }
            }

            return boxGroup;
        }

        private static bool MoveBoxGroup(char[,] map, List<(int, int)> boxGroup, int direction)
        {
            var isBoxGroupMovable = true;
            foreach (var (row, col) in boxGroup)
            {
                if (map[row + direction, col] == Wall)
                {
                    isBoxGroupMovable = false;
                }
            }
            
            if (isBoxGroupMovable)
            {
                var orderedBoxGroup = new List<(int, int)>();

                if (direction > 0) // down
                {
                    orderedBoxGroup = boxGroup.OrderByDescending(x => x.Item1).ThenBy(x => x.Item2).ToList();
                }
                else if (direction < 0) // up
                {
                    orderedBoxGroup = boxGroup.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();
                }

                foreach (var (row, col) in orderedBoxGroup)
                {
                    map[row + direction, col] = map[row, col];
                    map[row, col] = FreeSpace;
                }
                return true;
            }

            return false;
        }
    }
}
