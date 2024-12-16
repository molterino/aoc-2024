using System.Drawing;

namespace Day14
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            const int seconds = 100;

            var path = "input.txt"; // 221616000 / ?
            var space = new Space(width: 101, height: 103);

            //var path = "input-template.txt"; // 12 / ?
            //var space = new Space(width: 11, height: 7); // template

            foreach (var line in File.ReadAllLines(path))
            {
                var px = int.Parse(line.Split("p=")[1].Split(",")[0]);
                var py = int.Parse(line.Split(",")[1].Split(" ")[0]);
                var vx = int.Parse(line.Split("v=")[1].Split(",")[0]);
                var vy = int.Parse(line.Split(",")[2]);

                var robot = new Robot
                {
                    Position = new Point(px, py),
                    Velocity = new Point(vx, vy)
                };

                space.Robots.Add(robot);
            }

            for (var i = 0; i < seconds; i++)
            {
                space.MoveRobots();
            }

            space.DisplayQuadrants();

            Console.WriteLine("\nRobots in quadrants:");
            space.CountRobotsInQuadrants();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Quadrant {i + 1}: {space.RobotsInQuadrants[i]}");
            }
            Console.WriteLine($"\nSafety factor: {space.GetSafetyFactor()}");
        }
    }
}
