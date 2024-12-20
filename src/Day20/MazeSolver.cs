namespace Day20
{
    public class MazeSolver
    {
        public const char Wall = '#';
        public const char Track = '.';

        private (int, int) StartPosition;
        private (int, int) EndPostion;
        private char[,] Maze;

        public MazeSolver(char[,] maze, (int, int) startPosition, (int, int) endPosition)
        {
            Maze = maze;
            StartPosition = startPosition;
            EndPostion = endPosition;
        }

        public int FindShortestPath()
        {
            (int directionX, int directionY)[] directions = [(0, -1), (-1, 0), (0, 1), (1, 0)]; // NWSE

            int rows = Maze.GetLength(0);
            int cols = Maze.GetLength(1);

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

                    var isWall = Maze[nextNodePositionX, nextNodePositionY] == Wall;
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
    }
}