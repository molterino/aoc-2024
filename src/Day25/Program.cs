namespace Day25
{
    public static class Program
    {
        private const string Path = "input.txt";
        //private const string Path = "input-template.txt";

        private static void Main(string[] args)
        {
            var schematics = File.ReadAllLines(Path);

            var lockSmith = new LockSmith();
            lockSmith.ForgeSchematics(schematics);

            var pairs = lockSmith.CountGoodLockAndKeyPairs();
            Console.WriteLine($"Unique lock/key pairs fit together: {pairs}"); // 3107
        }
    }
}
