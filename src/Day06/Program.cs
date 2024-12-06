namespace Day06
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var map = ReadMap();

            var guard = '^';

            // find guard init position
            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            var guardPosX = -1;
            var guardPosY = -1;
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
                //DisplayMap(map);
                var nextFieldPosX = -1;
                var nextFieldPosY = -1;

                // Figure out next position
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

                // If the next field is out of boundaries the guard leaves the map
                if (nextFieldPosX > rows - 1 || nextFieldPosX < 0 || nextFieldPosY > cols - 1 || nextFieldPosY < 0)
                {
                    map[guardPosX, guardPosY] = 'X';
                    //DisplayMap(map);
                    break;
                }

                var nextPos = map[nextFieldPosX, nextFieldPosY];
                // If the next field is . (unvisited) or X (visited) move to next field and update previous field to X
                if (nextPos == '.' || nextPos == 'X')
                {
                    map[guardPosX, guardPosY] = 'X';
                    guardPosX = nextFieldPosX;
                    guardPosY = nextFieldPosY;
                    map[guardPosX, guardPosY] = guard;
                }
                // If the next field is a # (obstruction) turn 90 degrees right
                else if (nextPos == '#')
                {
                    if (guard == '^') guard = '>';
                    else if (guard == '>') guard = 'ˇ';
                    else if (guard == 'ˇ') guard = '<';
                    else if (guard == '<') guard = '^';
                    map[guardPosX, guardPosY] = guard;
                }
            }

            // Count visited fields
            var visitedFields = 0;
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    if (map[x,y] == 'X')
                    {
                        visitedFields++;
                    }
                }
            }

            Console.WriteLine($"The guard visited this many fields before leaving the map (Part1): {visitedFields}"); //4665
        }

        private static char[,] ReadMap()
        {
            //const string path = "input-template.txt";
            const string path = "input.txt";

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
            Thread.Sleep(100);
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
