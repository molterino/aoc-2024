namespace Day16
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";
        //private const string Path = "input-template-2.txt";

        private static void Main(string[] args)
        {
            var mapData = File.ReadAllLines(Path);
            var pathFinder = new PathFinder(mapData);

            var cost = pathFinder.FindCheapestPathCost();
            Console.WriteLine($"Cheapest path: {cost}"); // 93436

            var seats = pathFinder.FindBestSeats();
            Console.WriteLine($"Best seats: {seats}"); // 486
        }
    }
}