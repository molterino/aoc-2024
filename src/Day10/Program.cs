namespace Day10
{
    public static class Program
    {
        private const bool UseTemplateData = false;
        private const int MinHeight = 0;
        private const int MaxHeight = 9;

        private static void Main(string[] args)
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";
            var topographicMap = File.ReadAllLines(path);
            var rows = topographicMap.Length;
            var cols = topographicMap[0].Length;

            var positions = new List<Position>();

            // create positions
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    var position = new Position
                    {
                        X = x,
                        Y = y,
                        Height = int.Parse(topographicMap[x][y].ToString())
                    };
                    positions.Add(position);
                }
            }

            // add neighbors
            foreach (var position in positions)
            {
                if (position.Height == MaxHeight)
                {
                    position.Neighbors = [];
                    continue;
                }

                int[] validShiftX = { -1, 1, 0, 0 };
                int[] validShiftY = { 0, 0, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    int neighborX = position.X + validShiftX[i];
                    int neighborY = position.Y + validShiftY[i];
                    var validPosition = neighborX >= 0 && neighborX < rows && neighborY >= 0 && neighborY < cols;

                    if (validPosition)
                    {
                        var neighborHeight = int.Parse(topographicMap[neighborX][neighborY].ToString());
                        var validHeight = neighborHeight == position.Height + 1;

                        if (validHeight)
                        {
                            var neighbor = positions.Single(pos => pos.X == neighborX && pos.Y == neighborY);
                            position.Neighbors.Add(neighbor);
                        }
                    }
                }
            }

            // count score
            var overallScore = 0;
            var trailHeads = positions.Where(x => x.IsTrailHead).ToList();

            foreach (var trailHead in trailHeads)
            {
                var visited = new HashSet<Position>();
                var queue = new Queue<Position>();
                queue.Enqueue(trailHead);
                int score = 0;

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    if (visited.Contains(current))
                        continue;

                    visited.Add(current);

                    if (current.Height == MaxHeight)
                    {
                        score++;
                        continue;
                    }

                    foreach (var neighbor in current.Neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }

                overallScore += score;
            }

            Console.WriteLine($"What is the sum of the scores of all trailheads on your topographic map? (Part1): {overallScore}");
        }
    }
}
