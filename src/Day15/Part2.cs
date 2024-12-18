namespace Day15
{
    public static class Part2
    {
        public static void Run()
        {
            const char Robot = '@';
            const char Wall = '#';
            const char Box = 'O';
            const char FreeSpace = '.';

            const string WidenRobot = "@.";
            const string WidenWall = "##";
            const string WidenBox = "[]";
            const string WidenFreeSpace = "..";

            var path = "input.txt";
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

            for (int i = 0; i < rows; i++)
            {
                var row = warehouseMapData[i]
                    .Replace(Wall.ToString(), WidenWall)
                    .Replace(Box.ToString(), WidenBox)
                    .Replace(FreeSpace.ToString(), WidenFreeSpace)
                    .Replace(Robot.ToString(), WidenRobot)
                    .ToCharArray();

                for (int j = 0; j < cols; j++)
                {
                    map[i, j] = row[j];
                }
            }

            // show widened map
            Console.WriteLine("Initial state:");
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            Console.ReadKey();

            // init robot
            var robotPositionX = 0;
            var robotPositionY = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (map[i, j] == Robot)
                    {
                        robotPositionX = i;
                        robotPositionY = j;
                    }
                }
            }

            // init movements
            var robotCommandsData = warehouseDocument.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).ToList();
            var commands = string.Concat(robotCommandsData).Replace(Environment.NewLine, "");

            char[] commandDirections = ['^', 'v', '<', '>'];
            int[] movementX = [-1, 1, 0, 0];
            int[] movementY = [0, 0, -1, 1];

            // move robot
            foreach (var command in commands)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                //var commandIndex = Array.IndexOf(commandDirections, command);

                if (command == '<')
                {
                    var nextPositionY = robotPositionY - 1;
                    var nextPosition = map[robotPositionX, nextPositionY];

                    if (nextPosition == FreeSpace)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[robotPositionX, nextPositionY] = Robot;
                        robotPositionY = nextPositionY;
                    }
                    else if (nextPosition == WidenBox[1])
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
                                    map[robotPositionX, nextPositionY - 2 - (2 * i)] = WidenBox[0];
                                    map[robotPositionX, nextPositionY - 1 - (2 * i)] = WidenBox[1];
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
                    var nextPositionY = robotPositionY + 1;
                    var nextPosition = map[robotPositionX, nextPositionY];

                    if (nextPosition == FreeSpace)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[robotPositionX, nextPositionY] = Robot;
                        robotPositionY = nextPositionY;
                    }
                    else if (nextPosition == WidenBox[0])
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
                                    map[robotPositionX, nextPositionY + 1 + (2 * i)] = WidenBox[0];
                                    map[robotPositionX, nextPositionY + 2 + (2 * i)] = WidenBox[1];
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
                    var nextPositionX = robotPositionX - 1;
                    var nextPosition = map[nextPositionX, robotPositionY];

                    var movable = TryMoveUp(map, robotPositionX, robotPositionY);
                    if (movable)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[nextPositionX, robotPositionY] = Robot;

                        robotPositionX = nextPositionX;
                    }
                }
                else if (command == 'v')
                {
                    var nextPositionX = robotPositionX + 1;
                    var nextPosition = map[nextPositionX, robotPositionY];

                    var movable = TryMoveDown(map, robotPositionX, robotPositionY);

                    if (movable)
                    {
                        map[robotPositionX, robotPositionY] = FreeSpace;
                        map[nextPositionX, robotPositionY] = Robot;

                        robotPositionX = nextPositionX;
                    }
                }
                //Console.WriteLine($"Move {command}:");
                //for (int i = 0; i < rows; i++)
                //{
                //    for (int j = 0; j < cols; j++)
                //    {
                //        Console.Write(map[i, j]);
                //    }
                //    Console.WriteLine();
                //}

                //Console.ReadKey();
            }

            // display map
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

            // calculate gps
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

            Console.WriteLine($"\nGPS: {gps}");
        }

        static bool TryMoveUp(char[,] matrix, int startX, int startY)
        {
            char above = matrix[startX - 1, startY];

            if (above == '#')
            {
                return false;
            }

            if (above == '.')
            {
                matrix[startX - 1, startY] = '@';
                matrix[startX, startY] = '.';
                return true;
            }

             if (above == '[' || above == ']')
            {
                var boxGroup = GetBoxGroupUp(matrix, startX - 1, startY);

                if (CanMoveBoxGroupUp(matrix, boxGroup))
                {
                    MoveBoxGroupUp(matrix, boxGroup);

                    matrix[startX - 1, startY] = '@';
                    matrix[startX, startY] = '.';

                    return true;
                }
            }

            return false;
        }

        static List<(int, int)> GetBoxGroupUp(char[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            var queue = new Queue<(int, int)>();
            var visited = new List<(int, int)>();
            var boxGroup = new List<(int, int)>();

            queue.Enqueue((row, col));

            while (queue.Count > 0)
            {
                var (currentRow, currentCol) = queue.Dequeue();

                if (visited.Contains((currentRow, currentCol))) continue;
                visited.Add((currentRow, currentCol));
                boxGroup.Add((currentRow, currentCol));

                if (currentCol > 0 && matrix[currentRow, currentCol - 1] == '[')
                {
                    queue.Enqueue((currentRow, currentCol - 1));
                }

                if (currentCol < m - 1 && matrix[currentRow, currentCol + 1] == ']')
                {
                    queue.Enqueue((currentRow, currentCol + 1));
                }

                if (matrix[currentRow - 1, currentCol] == '[' || matrix[currentRow - 1, currentCol] == ']')
                {
                    queue.Enqueue((currentRow - 1, currentCol));
                }
            }

            return boxGroup;
        }

        static bool CanMoveBoxGroupUp(char[,] matrix, List<(int, int)> boxGroup)
        {
            foreach (var (row, col) in boxGroup)
            {
                if (matrix[row - 1, col] == '#')
                {
                    return false;
                }
            }
            return true;
        }

        static void MoveBoxGroupUp(char[,] matrix, List<(int, int)> boxGroup)
        {
            foreach (var (row, col) in boxGroup.OrderBy(x => x.Item1).ThenBy(x => x.Item2))
            {
                matrix[row - 1, col] = matrix[row, col];
                matrix[row, col] = '.';
            }
        }

        static bool TryMoveDown(char[,] matrix, int startX, int startY)
        {
            char below = matrix[startX + 1, startY];

            if (below == '#')
            {
                return false;
            }

            if (below == '.')
            {
                matrix[startX + 1, startY] = '@';
                matrix[startX, startY] = '.';
                return true;
            }

            if (below == '[' || below == ']')
            {
                var boxGroup = GetBoxGroupDown(matrix, startX + 1, startY);

                if (CanMoveBoxGroupDown(matrix, boxGroup))
                {
                    MoveBoxGroupDown(matrix, boxGroup);

                    matrix[startX + 1, startY] = '@';
                    matrix[startX, startY] = '.';

                    return true;
                }
            }

            return false;
        }

        static List<(int, int)> GetBoxGroupDown(char[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            var queue = new Queue<(int, int)>();
            var visited = new List<(int, int)>();
            var boxGroup = new List<(int, int)>();

            queue.Enqueue((row, col));

            while (queue.Count > 0)
            {
                var (currentRow, currentCol) = queue.Dequeue();

                if (visited.Contains((currentRow, currentCol))) continue;
                visited.Add((currentRow, currentCol));
                boxGroup.Add((currentRow, currentCol));

                if (currentCol > 0 && matrix[currentRow, currentCol - 1] == '[')
                {
                    queue.Enqueue((currentRow, currentCol - 1));
                }

                if (currentCol < m - 1 && matrix[currentRow, currentCol + 1] == ']')
                {
                    queue.Enqueue((currentRow, currentCol + 1));
                }

                if (matrix[currentRow + 1, currentCol] == '[' || matrix[currentRow + 1, currentCol] == ']')
                {
                    queue.Enqueue((currentRow + 1, currentCol));
                }
            }

            return boxGroup;
        }

        static bool CanMoveBoxGroupDown(char[,] matrix, List<(int, int)> boxGroup)
        {
            foreach (var (row, col) in boxGroup)
            {
                if (matrix[row + 1, col] == '#')
                {
                    return false;
                }
            }
            return true;
        }

        static void MoveBoxGroupDown(char[,] matrix, List<(int, int)> boxGroup)
        {
            foreach (var (row, col) in boxGroup.OrderByDescending(x => x.Item1).ThenBy(x => x.Item2))
            {
                matrix[row + 1, col] = matrix[row, col];
                matrix[row, col] = '.';
            }
        }
    }
}
