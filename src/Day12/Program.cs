using System.Drawing;

namespace Day12
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var path = "input.txt"; // 1377008 / 815788
            //var path = "input-template-1.txt"; // 140 / 80
            //var path = "input-template-2.txt"; // 772 / 436
            //var path = "input-template-3.txt"; // 1930 / 1206

            var garden = File.ReadAllLines(path);
            var rows = garden.Length;
            var cols = garden[0].Length;

            int regionId = 0;
            var regions = new List<Region>();
            var visitedFields = new List<Point>();

            int[] shiftX = { -1, 1, 0, 0 };
            int[] shiftY = { 0, 0, -1, 1 };
            int[] colShiftX = { -1, 1, -1, 1 };
            int[] rowShiftY = { -1, -1, 1, 1 };
            int[] diagonalShiftX = { -1, 1, -1, 1 };
            int[] diagonalShiftY = { -1, -1, 1, 1 };

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    var field = new Point(x, y);
                    var isFieldVisited = visitedFields.Contains(field);

                    if (isFieldVisited)
                    {
                        continue;
                    }

                    var regionType = garden[x][y];
                    var region = new Region(regionId, regionType);
                    var pendingFields = new Queue<Point>();

                    visitedFields.Add(field);
                    pendingFields.Enqueue(field);

                    while (pendingFields.Count > 0)
                    {
                        var queuedField = pendingFields.Dequeue();
                        region.Fields.Add(queuedField);

                        for (int i = 0; i < 4; i++)
                        {
                            var neighbor = new Point(queuedField.X + shiftX[i], queuedField.Y + shiftY[i]);
                            var validNeighborPosition = neighbor.X >= 0 && neighbor.X < rows && neighbor.Y >= 0 && neighbor.Y < cols;

                            if (!validNeighborPosition)
                            {
                                continue;
                            }

                            var isNeighborMatchingType = garden[neighbor.X][neighbor.Y] == region.Type;
                            var isNeighborVisited = visitedFields.Contains(new Point(neighbor.X, neighbor.Y));
                            var shouldRegionGrow = isNeighborMatchingType && !isNeighborVisited;

                            if (shouldRegionGrow)
                            {
                                visitedFields.Add(neighbor);
                                pendingFields.Enqueue(neighbor);
                            }
                        }
                    }

                    regions.Add(region);
                    regionId++;
                }
            }

            foreach (var region in regions)
            {
                foreach (var field in region.Fields)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var isShiftedNeighborInRegion = region.Fields.Contains(new Point(field.X + shiftX[i], field.Y + shiftY[i]));
                        if (!isShiftedNeighborInRegion)
                        {
                            region.Perimeter++;
                        }

                        var isColShiftedNeighborInRegion = region.Fields.Contains(new Point(field.X + colShiftX[i], field.Y + 0));
                        var isRowShiftedNeighborInRegion = region.Fields.Contains(new Point(field.X + 0, field.Y + rowShiftY[i]));
                        if (!isColShiftedNeighborInRegion && !isRowShiftedNeighborInRegion)
                        {
                            region.OuterCorners++;
                        }

                        var isDiagonalShiftedNeighborInRegion = region.Fields.Contains(new Point(field.X + diagonalShiftX[i], field.Y + diagonalShiftY[i]));
                        if (!isDiagonalShiftedNeighborInRegion && isColShiftedNeighborInRegion && isRowShiftedNeighborInRegion)
                        {
                            region.InnerCorners++;
                        }
                    }
                }
            }

            var overallFencingPrice = regions.Sum(region => region.FencingPrice);
            Console.WriteLine($"Overall fencing price (Part 1): {overallFencingPrice}");

            var overallFencingPriceWithDiscount = regions.Sum(region => region.FencingPriceWithDiscount);
            Console.WriteLine($"Overall fencing price with discount (Part 2): {overallFencingPriceWithDiscount}");
        }
    }
}
