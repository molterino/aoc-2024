namespace Day06
{
    internal static class Program
    {
        private const bool UseTemplateData = false;
        private const bool DisplayEachStep = false;

        private static void Main(string[] args)
        {
            Part1(); //How many distinct positions will the guard visit before leaving the mapped area?
            Part2(); //How many different positions could you choose for this obstruction?
        }

        private static void Part1()
        {
            var map = ReadMap();
            var guard = '^';

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            var guardPosX = -1;
            var guardPosY = -1;

            // find guard init position
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    var currentField = map[x, y];
                    if (currentField == guard)
                    {
                        guardPosX = x;
                        guardPosY = y;
                        break;
                    }
                }
            }

            while (true)
            {
                DisplayMap(map);
                var nextFieldPosX = -1;
                var nextFieldPosY = -1;

                // figure the guard's out next position
                if (guard == '^')
                {
                    nextFieldPosX = guardPosX - 1;
                    nextFieldPosY = guardPosY;
                }
                else if (guard == '>')
                {
                    nextFieldPosX = guardPosX;
                    nextFieldPosY = guardPosY + 1;
                }
                else if (guard == 'ˇ')
                {
                    nextFieldPosX = guardPosX + 1;
                    nextFieldPosY = guardPosY;
                }
                else if (guard == '<')
                {
                    nextFieldPosX = guardPosX;
                    nextFieldPosY = guardPosY - 1;
                }

                // if the next field is out of boundaries the guard leaves the map
                if (nextFieldPosX > rows - 1 || nextFieldPosX < 0 || nextFieldPosY > cols - 1 || nextFieldPosY < 0)
                {
                    map[guardPosX, guardPosY] = 'X';
                    DisplayMap(map);
                    break;
                }

                // if the next field is . (unvisited) or X (visited) move to next field and update previous field to X
                var nextField = map[nextFieldPosX, nextFieldPosY];
                if (nextField == '.' || nextField == 'X')
                {
                    map[guardPosX, guardPosY] = 'X';
                    guardPosX = nextFieldPosX;
                    guardPosY = nextFieldPosY;
                    map[guardPosX, guardPosY] = guard;
                }
                // if the next field is a # (obstruction) turn 90 degrees right
                else if (nextField == '#')
                {
                    if (guard == '^') guard = '>';
                    else if (guard == '>') guard = 'ˇ';
                    else if (guard == 'ˇ') guard = '<';
                    else if (guard == '<') guard = '^';
                    map[guardPosX, guardPosY] = guard;
                }
            }

            // count visited fields
            var visitedFields = 0;
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    if (map[x, y] == 'X')
                    {
                        visitedFields++;
                    }
                }
            }

            Console.WriteLine($"How many distinct positions will the guard visit before leaving the mapped area? (Part1): {visitedFields}"); //4665
        }

        private static void Part2()
        {
            var map = ReadMap();
            var guard = '^';

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            var guardPosX = -1;
            var guardPosY = -1;
            var guardInitPosX = -1;
            var guardInitPosY = -1;
            var guardTurnFields = new List<(int, int)>();
            var infiniteLoopCounter = 0;

            // find guard init position
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    var currentField = map[x, y];
                    if (currentField == guard)
                    {
                        guardPosX = guardInitPosX = x;
                        guardPosY = guardInitPosY = y;
                        break;
                    }
                }
            }

            // using bruteforce, I'm not proud of it, but it works
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    map = ReadMap();

                    // we only want to alter the normal fields
                    if (map[x, y] != '.')
                    {
                        continue;
                    }

                    map[x, y] = 'O'; // new obstruction
                    guard = '^';
                    guardPosX = guardInitPosX;
                    guardPosY = guardInitPosY;
                    guardTurnFields = [];

                    var isInfiniteLoop = false;

                    while (true)
                    {
                        DisplayMap(map);
                        var nextFieldPosX = -1;
                        var nextFieldPosY = -1;

                        // figure the guard's out next position
                        if (guard == '^')
                        {
                            nextFieldPosX = guardPosX - 1;
                            nextFieldPosY = guardPosY;
                        }
                        else if (guard == '>')
                        {
                            nextFieldPosX = guardPosX;
                            nextFieldPosY = guardPosY + 1;
                        }
                        else if (guard == 'ˇ')
                        {
                            nextFieldPosX = guardPosX + 1;
                            nextFieldPosY = guardPosY;
                        }
                        else if (guard == '<')
                        {
                            nextFieldPosX = guardPosX;
                            nextFieldPosY = guardPosY - 1;
                        }

                        // if the next field is out of boundaries the guard leaves the map
                        if (nextFieldPosX > rows - 1 || nextFieldPosX < 0 || nextFieldPosY > cols - 1 || nextFieldPosY < 0)
                        {
                            map[guardPosX, guardPosY] = 'X';
                            DisplayMap(map);
                            break;
                        }

                        // if the next field is . (unvisited) or X (visited) move to next field and update previous field to X
                        var nextPos = map[nextFieldPosX, nextFieldPosY];
                        if (nextPos == '.' || nextPos == 'X')
                        {
                            map[guardPosX, guardPosY] = 'X';
                            guardPosX = nextFieldPosX;
                            guardPosY = nextFieldPosY;
                            map[guardPosX, guardPosY] = guard;
                        }
                        // if the next field is a # or O (obstruction) turn 90 degrees right
                        else if (nextPos == '#' || nextPos == 'O')
                        {
                            if (guard == '^') guard = '>';
                            else if (guard == '>') guard = 'ˇ';
                            else if (guard == 'ˇ') guard = '<';
                            else if (guard == '<') guard = '^';
                            map[guardPosX, guardPosY] = guard;

                            // 1 is normal turning, 2 is going backwards, any more means loop
                            var isAlreadyTurnedOnThisFieldMoreThenTwice = guardTurnFields.Count(x => x.Item1 == guardPosX && x.Item2 == guardPosY) > 2;
                            if (isAlreadyTurnedOnThisFieldMoreThenTwice)
                            {
                                isInfiniteLoop = true;
                                break;
                            }

                            var guardTurnField = (guardPosX, guardPosY);
                            guardTurnFields.Add(guardTurnField);
                        }
                    }

                    if (isInfiniteLoop)
                    {
                        infiniteLoopCounter++;
                    }
                }
            }

            Console.WriteLine($"How many different positions could you choose for this obstruction? (Part2): {infiniteLoopCounter}"); //1688
        }

        private static char[,] ReadMap()
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";

            try
            {
                var lines = File.ReadAllLines(path);
                var rows = lines.Length;
                var cols = lines[0].Length;
                var map = new char[rows, cols];

                for (int x = 0; x < rows; x++)
                {
                    for (int y = 0; y < cols; y++)
                    {
                        map[x, y] = lines[x][y];
                    }
                }

                return map;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during file processing: {e.Message}");
                return new char[0, 0];
            }
        }

        private static void DisplayMap(char[,] map)
        {
            if (!DisplayEachStep)
            {
                return;
            }

            Thread.Sleep(20);
            Console.SetCursorPosition(0, 0);

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
