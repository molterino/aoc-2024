namespace Day16
{
    public static class MazeSolver
    {
        public const char WallToken = '#';
        public const char FreeSpace = '.';
        public const char StartToken = 'S';
        public const char EndToken = 'E';
        public const char PathToken = 'O';

        public static (int, int) StartPosition;
        public static (int, int) EndPostion;

        public static int TurnCost = 1000;
        public static int MoveCost = 1;

        public static int FindShortestPath(char[,] maze)
        {
            (int directionX, int directionY)[] directions = [(0, -1), (-1, 0), (0, 1), (1, 0)]; // NWSE

            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            var queue = new SortedSet<Node>(new NodeComparer());
            var costMap = new Dictionary<(int, int), int>();
            var visited = new HashSet<(int, int)>();
            var parents = new Dictionary<(int, int), (int x, int y)>();

            var startNode = new Node(point: StartPosition, cost: 0, direction: -1);
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
                    var isWall = maze[nextNodePositionX, nextNodePositionY] == WallToken;

                    if (isOutOfMaze || isWall)
                    {
                        continue;
                    }

                    var nextNodeDirection = Array.IndexOf(directions, (directionX, directionY));

                    var turnCost = currentNode.Direction != -1 && currentNode.Direction != nextNodeDirection ? TurnCost : 0;
                    var nextNodeCost = currentNode.Cost + MoveCost + turnCost;

                    var isNodeVisited = visited.Contains(nextNodePosition);
                    var isNodeCostHigher = costMap.TryGetValue(nextNodePosition, out int existingCost) && nextNodeCost >= existingCost;

                    if (isNodeVisited || isNodeCostHigher)
                    {
                        continue;
                    }

                    costMap[nextNodePosition] = nextNodeCost;
                    var nextNode = new Node(nextNodePosition, nextNodeCost, nextNodeDirection);
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
                if (current != end && current != start)
                {
                    maze[current.x, current.y] = PathToken;
                }
                current = parents[current];
            }
        }
    }
}