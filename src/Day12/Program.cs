namespace Day12
{
    public static class Program
    {
        private const int MaxBlink = 25;

        private static void Main(string[] args)
        {
            var path = "input.txt"; // Part1: 1377008
            //var path = "input-template-1.txt"; // 140
            //var path = "input-template-2.txt"; // 772
            //var path = "input-template-3.txt"; // 1930

            var input = File.ReadAllLines(path);
            var rows = input.Length;
            var cols = input[0].Length;

            var garden = new char[rows, cols];
            var visitedFields = new bool[rows, cols];

            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    garden[x, y] = input[x][y];
                }
            }

            var regions = new List<Region>();
            int regionId = 0;

            int[] shiftX = { -1, 1, 0, 0 };
            int[] shiftY = { 0, 0, -1, 1 };

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    var fieldVisited = visitedFields[x, y];
                    if (!fieldVisited)
                    {
                        var regionType = garden[x, y];
                        var region = new Region(regionId, regionType);

                        regionId++;

                        var pendingFields = new Queue<(int, int)>();
                        pendingFields.Enqueue((x, y));
                        visitedFields[x, y] = true;

                        while (pendingFields.Count > 0)
                        {
                            var (queuedFieldX, queuedFieldY) = pendingFields.Dequeue();
                            region.Area++;

                            for (int i = 0; i < 4; i++)
                            {
                                int neighborX = queuedFieldX + shiftX[i];
                                int neighborY = queuedFieldY + shiftY[i];

                                var validNeighborPosition = neighborX >= 0 && neighborX < rows && neighborY >= 0 && neighborY < cols;
                                if (validNeighborPosition)
                                {
                                    var neighborType = garden[neighborX, neighborY];
                                    var isNeighborMatchingType = neighborType == region.Type;
                                    var isNeighborVisited = visitedFields[neighborX, neighborY];

                                    if (isNeighborMatchingType && !isNeighborVisited)
                                    {
                                        visitedFields[neighborX, neighborY] = true;
                                        pendingFields.Enqueue((neighborX, neighborY));
                                    }
                                    else if (!isNeighborMatchingType)
                                    {
                                        region.Perimeter++;
                                    }
                                }
                                else
                                {
                                    region.Perimeter++;
                                }
                            }
                        }

                        regions.Add(region);
                    }
                }
            }

            var overallFencingPrice = 0;
            foreach (var region in regions)
            {
                overallFencingPrice += region.FencingPrice;
                Console.WriteLine($"Region {region.Type} perimeter: {region.Perimeter}, area: {region.Area}, fencing price {region.FencingPrice}");
            }

            Console.WriteLine($"Overall fencing price: {overallFencingPrice}"); // Part1: 1377008
        }
    }
}
