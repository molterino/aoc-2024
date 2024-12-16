namespace Day14
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var robots = "input.txt";
            var space = new Space(width: 101, height: 103);

            //var path = "input-template.txt";
            //var space = new Space(width: 11, height: 7); // template

            //Part 1
            space.InitSpace(robots);
            space.MoveRobots(100);
            space.DisplayQuadrants();
            space.CountRobotsInQuadrants();

            Console.WriteLine("\nRobots in quadrants:");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Quadrant {i + 1}: {space.RobotsInQuadrants[i]}");
            }
            Console.WriteLine($"\nSafety factor: {space.GetSafetyFactor()}"); //221616000
            Console.WriteLine("Press any key to continue with part 2...");
            Console.ReadKey();

            //Part 2
            space.InitSpace(robots);
            int counter = 1;

            while(true)
            {
                Console.Clear();
                Console.WriteLine($"After {counter} seconds...");

                space.MoveRobots();

                var possibleXmasTree = space.HasAllTheRobotsUniquePosition();
                if (possibleXmasTree)
                {
                    space.DisplayMap();
                    Console.WriteLine($"Xmas tree found after {counter} seconds!"); // 7572
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    break;
                }

                counter++;
            }
        }
    }
}
