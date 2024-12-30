namespace Day22
{
    public static class Program
    {
        private const int PriceChanges = 2000;
        private const string Path = "input.txt"; // 17163502021 / 1938
        //private const string Path = "input-template.txt"; // 37327623
        //private const string Path = "input-template-2.txt";

        private static void Main(string[] args)
        {
            var initialSecretNumbers = File.ReadAllLines(Path).Select(long.Parse).ToList();

            PartOne(initialSecretNumbers);
            PartTwo(initialSecretNumbers);
        }

        private static void PartOne(List<long> initialSecretNumbers)
        {
            long sumEvolvedSecretNumbers = 0;

            foreach (var secretNumber in initialSecretNumbers)
            {
                long evolvedSecretNumber = secretNumber;

                for (var i = 0; i < PriceChanges; i++)
                {
                    evolvedSecretNumber = Evolve(evolvedSecretNumber);
                }

                sumEvolvedSecretNumbers += evolvedSecretNumber;
            }

            Console.WriteLine($"Part 1, sum of evolved secret numbers: {sumEvolvedSecretNumbers}\n");
        }

        private static void PartTwo(List<long> initialSecretNumbers)
        {
            int processCounter = 1;
            var sequences = GenerateAllPossibleSequences();

            foreach (var secretNumber in initialSecretNumbers)
            {
                Console.SetCursorPosition(0, 2);
                Console.WriteLine($"Calculating changes and prices for next task: {processCounter++}/{initialSecretNumbers.Count}");

                var keys = new List<string>();
                var changes = new List<int?>() { null };
                var prices = new List<int>() { (int)(secretNumber % 10) };
                var previousSecretNumber = secretNumber;

                for (int i = 1; i < PriceChanges; i++)
                {
                    var currentSecretNumber = Evolve(previousSecretNumber);
                    var currentLastDigit = (int)(currentSecretNumber % 10);
                    var previousLastDigit = (int)(previousSecretNumber % 10);

                    changes.Add(currentLastDigit - previousLastDigit);
                    prices.Add(currentLastDigit);

                    previousSecretNumber = currentSecretNumber;
                }

                for (int i = 1; i < PriceChanges - 4; i++)
                {
                    var key = $"{changes[i]},{changes[i + 1]},{changes[i + 2]},{changes[i + 3]}";

                    if (!keys.Contains(key))
                    {
                        keys.Add(key);
                        sequences[key] += prices[i + 3];
                    }
                }
            }

            var maxPrice = sequences.Values.Max();
            var maxSequence = sequences.First(x => x.Value == maxPrice).Key;

            Console.WriteLine($"Part 2, max price: {maxPrice}, sequence: {maxSequence}");
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

        private static long MixAndPrune(long value, long secretNumber)
        {
            var mix = value ^ secretNumber;
            var prune = mix % 16777216;

            return prune;
        }

        private static Dictionary<string, long> GenerateAllPossibleSequences()
        {
            var sequences = new Dictionary<string, long>();

            for (int i = -9; i <= 9; i++)
            {
                for (int j = -9; j <= 9; j++)
                {
                    for (int k = -9; k <= 9; k++)
                    {
                        for (int l = -9; l <= 9; l++)
                        {
                            sequences[$"{i},{j},{k},{l}"] = 0;
                        }
                    }
                }
            }

            return sequences;
        }
    }
}
