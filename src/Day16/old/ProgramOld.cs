namespace Day16.old
{
    public static class ProgramOld
    {
        private const string Path = "input.txt"; // 93436
        //private const string Path = "input-template-1.txt"; // 7036
        //private const string Path = "input-template-2.txt"; // 11048

        //private static void Main(string[] args)
        //{
        //    var maze = GetMazeMatrix(Path);

        //    var result = MazeSolver.FindShortestPath(maze);
        //    Display(maze);
        //    Console.WriteLine($"\nScore: {result}");
        //    Console.ReadKey();
        //}

        private static char[,] GetMazeMatrix(string path)
        {
            var mazeData = File.ReadAllLines(path);

            var rows = mazeData.Length;
            var cols = mazeData[0].Length;
            var maze = new char[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var currentField = mazeData[i][j];
                    maze[i, j] = currentField;

                    if (currentField == MazeSolver.StartToken)
                    {
                        MazeSolver.StartPosition = (i, j);
                    }
                    else if (currentField == MazeSolver.EndToken)
                    {
                        MazeSolver.EndPostion = (i, j);
                    }
                }
            }

            return maze;
        }

        private static void Display(char[,] maze)
        {
            for (var i = 0; i < maze.GetLength(0); i++)
            {
                for (var j = 0; j < maze.GetLength(1); j++)
                {
                    var currentField = maze[i, j];

                    if (currentField == MazeSolver.StartToken) Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else if (currentField == MazeSolver.EndToken) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (currentField == MazeSolver.WallToken) Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (currentField == MazeSolver.PathToken) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
