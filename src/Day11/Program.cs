namespace Day11
{
    public static class Program
    {
        private const int MaxBlink = 25;

        private static void Main(string[] args)
        {
            var path = "input.txt";
            //var path = "input-template.txt"; //55312
            var line = File.ReadAllText(path);
            var stones = line.Split(" ").ToList();

            for (var blink = 1; blink <= MaxBlink; blink++)
            {
                var stonesAfterBlinking = new List<string>();

                foreach (var stone in stones)
                {
                    if (stone == "0")
                    {
                        stonesAfterBlinking.Add("1");
                    }
                    else if (stone.Length % 2 == 0)
                    {
                        var leftStone = stone.Substring(0, stone.Length / 2);
                        var rightStone = stone.Substring(stone.Length / 2).TrimStart('0');

                        if (rightStone == "")
                        {
                            rightStone = "0";
                        }

                        stonesAfterBlinking.Add(leftStone);
                        stonesAfterBlinking.Add(rightStone);
                    }
                    else
                    {
                        var newStone = long.Parse(stone) * 2024;
                        stonesAfterBlinking.Add(newStone.ToString());
                    }
                }

                stones = stonesAfterBlinking;
            }

            Console.WriteLine($"How many stones will you have after blinking 25 times? (Part1): {stones.Count}"); // 183620
        }
    }
}
