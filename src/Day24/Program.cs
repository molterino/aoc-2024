namespace Day24
{
    public static class Program
    {
        private const string Path = "input.txt"; // 54715147844840
        //private const string Path = "input-template-1.txt"; // 4
        //private const string Path = "input-template-2.txt"; // 2024

        private static void Main(string[] args)
        {
            var logicGates = File.ReadAllLines(Path);

            var rca = new RippleCarryAdder(logicGates);

            var evaluationResult = rca.Evaluate();
            Console.WriteLine(evaluationResult); // 54715147844840

            var swappedOutputWires = rca.GetSwappedOutputWires();
            Console.WriteLine(swappedOutputWires); // ggn,grm,jcb,ndw,twr,z10,z32,z39
        }
    }
}
