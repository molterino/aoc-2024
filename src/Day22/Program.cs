namespace Day22
{
    public static class Program
    {
        private const string Path = "input.txt"; // 17163502021
        //private const string Path = "input-template.txt"; // 37327623

        private static void Main(string[] args)
        {
            var initialSecretNumbers = File.ReadAllLines(Path).Select(long.Parse).ToList();
            long sumEvolvedSecretNumbers = 0;

            foreach (var secretNumber in initialSecretNumbers)
            {
                var evolvedSecretNumber = Evolve2000(secretNumber);
                sumEvolvedSecretNumbers += evolvedSecretNumber;
                Console.WriteLine($"{secretNumber}: {evolvedSecretNumber}");
            }

            Console.WriteLine($"\nSum: {sumEvolvedSecretNumbers}");
        }

        private static long Evolve(long secretNumber)
        {
            // Step 1
            long stepOneMultiply = secretNumber * 64;
            long stepOneSecretNumber = MixAndPrune(stepOneMultiply, secretNumber);

            // Step 2
            decimal stepTwoDivide = stepOneSecretNumber / 32;
            long stepTwoFloor = (long) Math.Floor(stepTwoDivide);
            long stepTwoSecretNumber = MixAndPrune(stepTwoFloor, stepOneSecretNumber);

            // Step 3
            long stepThreeMultiply = stepTwoSecretNumber * 2048;
            long stepThreeSecretNumber = MixAndPrune(stepThreeMultiply, stepTwoSecretNumber);

            return stepThreeSecretNumber;
        }

        private static long Evolve2000(long secretNumber)
        {
            var evolvedSecretNumber = secretNumber;
            for (var i = 0; i < 2000; i++)
            {
                evolvedSecretNumber = Evolve(evolvedSecretNumber);
            }
            return evolvedSecretNumber;
        }

        private static long Mix(long value, long secretNumber)
        {
            return value ^ secretNumber;
        }

        private static long Prune(long secretNumber)
        {
            return secretNumber % 16777216;
        }

        private static long MixAndPrune(long value, long secretNumber)
        {
            var mix = Mix(value, secretNumber);
            var prune = Prune(mix);

            return prune;
        }
    }
}
