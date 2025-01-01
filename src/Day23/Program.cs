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

            var threeComputerGroups = network.CountThreeComputerGroups();
            Console.WriteLine($"{threeComputerGroups} sets of three inter-connected computers"); // 1599

            var largestNetworkComputers = network.FindLargestGroup();
            Console.WriteLine($"Members of the largest group: {largestNetworkComputers}"); // av,ax,dg,di,dw,fa,ge,kh,ki,ot,qw,vz,yw
        }
    }
}
