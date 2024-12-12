namespace Day11
{
    public static class Program
    {
        //private const int MaxBlink = 25; // Part 1: 183620
        private const int MaxBlink = 75; // Part 2: 220377651399268

        private static void Main(string[] args)
        {
            const string path = "input.txt";
            //const string path = "input-template.txt"; //55312

            var currentStones = File.ReadAllText(path).Split(" ").ToDictionary(long.Parse, _ => (long)1);

            for (var blink = 1; blink <= MaxBlink; blink++)
            {
                var newStones = new Dictionary<long, long>();

                foreach (var stone in currentStones)
                {
                    if (stone.Key == 0)
                    {
                        newStones.AddOrUpdate(1, stone.Value);
                    }
                    else if (stone.Key.ToString().Length % 2 == 0)
                    {
                        var leftStone = stone.Key.ToString().Substring(0, stone.Key.ToString().Length / 2);
                        var rightStone = stone.Key.ToString().Substring(stone.Key.ToString().Length / 2);

                        newStones.AddOrUpdate(long.Parse(leftStone), stone.Value);
                        newStones.AddOrUpdate(long.Parse(rightStone), stone.Value);
                    }
                    else
                    {
                        newStones.AddOrUpdate(stone.Key * 2024, stone.Value);
                    }
                }

                currentStones = new Dictionary<long, long>(newStones);
                Console.WriteLine($"blink: {blink}, stones: {currentStones.Values.Sum()}");
            }

            Console.WriteLine($"How many stones will you have after blinking {MaxBlink} times?: {currentStones.Values.Sum()}");
        }
    }
}
