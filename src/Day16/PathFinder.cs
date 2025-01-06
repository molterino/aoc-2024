namespace Day16
{
    public class PathFinder
    {
        private const char WallToken = '#';
        private const char EndToken = 'E';
        private const char StartToken = 'S';

        private readonly char[,] _map;
        private readonly Position? _startPosition;
        private readonly Position _endPosition;

        private readonly Dictionary<Step, int> _costs = [];
        private readonly Dictionary<Step, List<Step>> _paths = [];
        private readonly Direction[] _directions = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];

        public PathFinder(string[] mapData)
        {
            var rows = mapData.Length;
            var cols = mapData[0].Length;
            var map = new char[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var fieldToken = mapData[i][j];
                    map[i, j] = fieldToken;

                    if (fieldToken == StartToken) _startPosition = new Position(i, j);
                    else if (fieldToken == EndToken) _endPosition = new Position(i, j);
                }
            }

            if (_startPosition == null || _endPosition == null)
            {
                throw new ArgumentException("Start or end position not found");
            }

            _map = map;
        }

        public int FindCheapestPathCost()
        {
            int mapWidth = _map.GetLength(0);
            int mapHeight = _map.GetLength(1);

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    foreach (var direction in _directions)
                    {
                        var position = new Position(x, y);
                        var step = new Step(position, direction);

                        _costs[step] = int.MaxValue;
                        _paths[step] = [];
                    }
                }
            }

            var firstDirection = _directions[2];
            var firstStep = new Step(_startPosition!, firstDirection);
            _costs[firstStep] = 0;

            var visitedSteps = new HashSet<Step>();
            var queue = new PriorityQueue<Step, int>();
            queue.Enqueue(firstStep, 0);

            while (queue.Count > 0)
            {
                var currentStep = queue.Dequeue();

                var isFinalStep = _map[currentStep.Position.X, currentStep.Position.Y] == EndToken;
                if (isFinalStep)
                {
                    break;
                }

                var isVisitedStep = visitedSteps.Contains(currentStep);
                if (isVisitedStep)
                {
                    continue;
                }

                visitedSteps.Add(currentStep);

                var neighbors = GetNeighbors(currentStep);
                foreach (var neighbor in neighbors)
                {
                    var isNeighborWall = _map[neighbor.Step.Position.X, neighbor.Step.Position.Y] == WallToken;
                    if (!isNeighborWall)
                    {
                        var nextStep = neighbor.Step;
                        var nextCost = _costs[currentStep] + neighbor.Cost;

                        var isCheaperPath = nextCost <= _costs[nextStep];
                        if (isCheaperPath)
                        {
                            _costs[nextStep] = nextCost;
                            _paths[nextStep].Add(currentStep);
                            queue.Enqueue(nextStep, nextCost);
                        }
                    }
                }
            }

            int cost = _directions.Min(direction => _costs[new Step(_endPosition, direction)]);
            return cost;
        }

        public int FindBestSeats()
        {
            var seats = new HashSet<Position>();
            var cheapestPathCost = _directions.Min(direction => _costs[new Step(_endPosition, direction)]);
            var directions = _directions.Where(direction => _costs[new Step(_endPosition, direction)] == cheapestPathCost);

            foreach (var direction in directions)
            {
                var step = new Step(_endPosition, direction);
                var beastSeats = TrackBestPathsBackwards(step, []);
                seats.UnionWith(beastSeats);
            }

            return seats.Count;
        }

        private List<(Step Step, int Cost)> GetNeighbors(Step step)
        {
            int[] costs = { 1, 1000, 2000, 1000 };
            var neighbors = new List<(Step, int)>();
            var direction = step.Direction;

            for (int i = 0; i < 4; i++)
            {
                var nextPosition = i == 0 ? step.Position.Add(step.Direction) : step.Position;
                var nextStep = new Step(nextPosition, direction);
                var neighbor = (nextStep, costs[i]);

                neighbors.Add(neighbor);
                direction = direction.Rotate();
            }

            return neighbors;
        }

        private HashSet<Position> TrackBestPathsBackwards(Step step, HashSet<Position> bestSeats)
        {
            var currentPosition = step.Position;
            var currentDirection = step.Direction;

            bestSeats.Add(currentPosition);

            var isFirstPosition = currentPosition == _startPosition;
            if (isFirstPosition)
            {
                return bestSeats;
            }

            var currentPaths = _paths[new Step(currentPosition, currentDirection)];
            var currentBestSeats = new HashSet<Position>();

            foreach (var path in currentPaths)
            {
                var pathStep = new Step(path.Position, path.Direction);
                var pathBestSeats = TrackBestPathsBackwards(pathStep, bestSeats);
                currentBestSeats.UnionWith(pathBestSeats);
            }

            bestSeats.UnionWith(currentBestSeats);
            return bestSeats;
        }
    }
}
