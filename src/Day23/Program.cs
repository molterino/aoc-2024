namespace Day23
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";

        private static void Main(string[] args)
        {
            var connections = File.ReadAllLines(Path);
            var network = new Network(connections);
            var groups = network.CountThreeComputerGroups();

            Console.WriteLine($"{groups} sets of three inter-connected computers"); // 1599
        }
    }
}
