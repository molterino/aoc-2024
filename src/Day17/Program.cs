namespace Day17
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";
        //private const string Path = "input-template-2.txt";

        private static void Main(string[] args)
        {
            var data = File.ReadAllLines(Path);
            Processor.Init(data);

            // Part 1
            var output = Processor.ExecuteProgram(); // 2,0,7,3,0,3,1,3,7
            Console.WriteLine($"Output: {string.Join(',', output)}");

            // Part 2
            var registerA = Processor.TryToCopyItself(); // 247839539763386
            Console.WriteLine($"RegisterA: {registerA}");
        }
    }
}
