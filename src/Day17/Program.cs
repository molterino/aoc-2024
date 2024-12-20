namespace Day17
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";

        private static void Main(string[] args)
        {
            Processor.InitRegisters(Path);
            Processor.InitProgram(Path);

            Processor.ExecuteProgram();
            Processor.PrintStatus(); // 2,0,7,3,0,3,1,3,7
        }
    }
}
