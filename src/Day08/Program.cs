namespace Day08
{
    public static class Program
    {
        private const bool UseTemplateData = false;

        private static void Main(string[] args)
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";
            var mapLines = File.ReadAllLines(path);
            var rows = mapLines.Length;
            var cols = mapLines[0].Length;
            var map = new char[rows, cols];
            var antennaGroups = new List<AntennaGroup>();
            var antinodesPart1 = new List<(int, int)>();
            var antinodesPart2 = new List<(int, int)>();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    map[x, y] = mapLines[x][y];

                    if (map[x, y] == '.')
                    {
                        continue;
                    }

                    var frequency = map[x, y];
                    var group = antennaGroups.FirstOrDefault(x => x.Frequency == frequency);

                    if (group is null)
                    {
                        group = new AntennaGroup { Frequency = frequency };
                        antennaGroups.Add(group);
                    }

                    var node = (x, y);
                    group.Nodes.Add(node);
                }
            }

            // calculate antinodes for part 1
            foreach (var group in antennaGroups)
            {
                for(int i = 0; i < group.Nodes.Count - 1; i++)
                {
                    for(int j = i + 1; j < group.Nodes.Count; j++)
                    {
                        var nodeA = group.Nodes[i];
                        var nodeB = group.Nodes[j];

                        var distanceX = nodeA.Item1 - nodeB.Item1;
                        var distanceY = nodeA.Item2 - nodeB.Item2;

                        var shiftX = distanceX * 2;
                        var shiftY = distanceY * 2;

                        var antinodeAX = nodeA.Item1 - shiftX;
                        var antinodeAY = nodeA.Item2 - shiftY;
                        var antinodeBX = nodeB.Item1 + shiftX;
                        var antinodeBY = nodeB.Item2 + shiftY;

                        var antinodeA = (antinodeAX, antinodeAY);
                        var antinodeB = (antinodeBX, antinodeBY);

                        var validAntinodeA = antinodeAX >= 0 && antinodeAX < cols && antinodeAY >= 0 && antinodeAY < rows;
                        var unkonwnAntinodeA = !antinodesPart1.Exists(x => x == antinodeA);
                        if (unkonwnAntinodeA && validAntinodeA)
                        {
                            antinodesPart1.Add(antinodeA);
                        }

                        var validAntinodeB = antinodeBX >= 0 && antinodeBX < cols && antinodeBY >= 0 && antinodeBY < rows;
                        var unkonwnAntinodeB = !antinodesPart1.Exists(x => x == antinodeB);
                        if (unkonwnAntinodeB && validAntinodeB)
                        {
                            antinodesPart1.Add(antinodeB);
                        }
                    }
                }
            }

            Console.WriteLine($"Nunber of unique locations within the bounds of the map containing an antinode (Part1): {antinodesPart1.Count}"); // 256

            // calculate antinodes for part 2
            foreach (var group in antennaGroups)
            {
                for (int i = 0; i < group.Nodes.Count - 1; i++)
                {
                    for (int j = i + 1; j < group.Nodes.Count; j++)
                    {
                        var nodeA = group.Nodes[i];
                        var nodeB = group.Nodes[j];

                        var distanceX = nodeA.Item1 - nodeB.Item1;
                        var distanceY = nodeA.Item2 - nodeB.Item2;

                        var stopAntinodeGeneration = false;
                        var cycle = 1;

                        while (!stopAntinodeGeneration)
                        {
                            var shiftX = distanceX * cycle;
                            var shiftY = distanceY * cycle;

                            var antinodeAX = nodeA.Item1 - shiftX;
                            var antinodeAY = nodeA.Item2 - shiftY;
                            var antinodeBX = nodeB.Item1 + shiftX;
                            var antinodeBY = nodeB.Item2 + shiftY;

                            var antinodeA = (antinodeAX, antinodeAY);
                            var antinodeB = (antinodeBX, antinodeBY);

                            var validAntinodeA = antinodeAX >= 0 && antinodeAX < cols && antinodeAY >= 0 && antinodeAY < rows;
                            var unkonwnAntinodeA = !antinodesPart2.Exists(x => x == antinodeA);
                            if (unkonwnAntinodeA && validAntinodeA)
                            {
                                antinodesPart2.Add(antinodeA);
                            }

                            var validAntinodeB = antinodeBX >= 0 && antinodeBX < cols && antinodeBY >= 0 && antinodeBY < rows;
                            var unkonwnAntinodeB = !antinodesPart2.Exists(x => x == antinodeB);
                            if (unkonwnAntinodeB && validAntinodeB)
                            {
                                antinodesPart2.Add(antinodeB);
                            }

                            if (!validAntinodeA && !validAntinodeB)
                            {
                                stopAntinodeGeneration = true;
                            }

                            cycle++;
                        }
                    }
                }
            }
            Console.WriteLine($"Nunber of unique locations within the bounds of the map containing an antinode (Part2): {antinodesPart2.Count}"); // 1005
        }
    }
}
