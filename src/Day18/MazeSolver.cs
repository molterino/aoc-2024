namespace Day18
{
    public static class MazeSolver
    {
        public const char Corrupted = '#';
        public const char Safe = '.';
        public const char Path = 'O';

        public static (int, int) StartPosition;
        public static (int, int) EndPostion;

        public static int FindShortestPath(char[,] maze)
        {
            (int directionX, int directionY)[] directions = [(0, -1), (-1, 0), (0, 1), (1, 0)]; // NWSE

            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            var queue = new SortedSet<Node>(new NodeComparer());
            var costMap = new Dictionary<(int, int), int>();
            var visited = new HashSet<(int, int)>();
            var parents = new Dictionary<(int, int), (int x, int y)>();

            var startNode = new Node(point: StartPosition, cost: 0);
            queue.Add(startNode);
            costMap[StartPosition] = 0;

            while (queue.Count > 0)
            {
                var currentNode = queue.Min;
                queue.Remove(currentNode);

                if (currentNode.Position == EndPostion)
                {
                    MarkPathOnMaze(maze, parents, EndPostion, StartPosition);
                    return currentNode.Cost;
                }

                visited.Add(currentNode.Position);

                foreach (var (directionX, directionY) in directions)
                {
                    int nextNodePositionX = currentNode.X + directionX;
                    int nextNodePositionY = currentNode.Y + directionY;
                    var nextNodePosition = (nextNodePositionX, nextNodePositionY);

                    var isOutOfMaze = nextNodePositionX < 0 || nextNodePositionY < 0 || nextNodePositionX >= rows || nextNodePositionY >= cols;

                    if (isOutOfMaze)
                    {
                        continue;
                    }

                    var isWall = maze[nextNodePositionX, nextNodePositionY] == Corrupted;
                    if (isWall)
                    {
                        continue;
                    }

                    var nextNodeCost = currentNode.Cost + 1;

                    var isNodeVisited = visited.Contains(nextNodePosition);
                    var isNodeCostHigher = costMap.TryGetValue(nextNodePosition, out int existingCost) && nextNodeCost >= existingCost;

                    if (isNodeVisited || isNodeCostHigher)
                    {
                        continue;
                    }

                    costMap[nextNodePosition] = nextNodeCost;
                    var nextNode = new Node(nextNodePosition, nextNodeCost);
                    queue.Add(nextNode);
                    parents[nextNodePosition] = currentNode.Position;
                }
            }

            return -1;
        }

        private static void MarkPathOnMaze(char[,] maze, Dictionary<(int, int), (int x, int y)> parents, (int x, int y) end, (int x, int y) start)
        {
            var current = end;

            while (parents.ContainsKey(current))
            {
                maze[current.x, current.y] = Path;
                current = parents[current];
            }
            maze[start.x, start.y] = Path;
        }
    }
}