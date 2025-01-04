namespace Day20
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";

        private static void Main(string[] args)
        {
            var racetrackData = File.ReadAllLines(Path);

            Console.WriteLine("Calculating...");
            var racetrack = new Racetrack(racetrackData);

            var fastestTime = racetrack.FindFastestTime();
            Console.WriteLine($"The fastest way without cheats is {fastestTime} picoseconds.");

            var cheatsCount = racetrack.FindCheatsCount(range: 20, shortcutFilter: 100, displaySteps: false);
            Console.WriteLine($"Cheats that save at least 100 picoseconds: {cheatsCount}"); // 1367 / 1006850

            //var cheats = racetrack.GetCheats(shortcutFilter);
            //foreach (var cheat in cheats)
            //{
            //    Console.WriteLine($"There are {cheat.Value} cheats that save {cheat.Key} picoseconds.");
            //}
        }
    }
}
