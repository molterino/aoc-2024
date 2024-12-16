namespace Day13
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var distantPrices = true; // enables part 2

            var path = "input.txt"; // 29436 / 103729094227877
            //var path = "input-template.txt"; // 480 / 875318608908

            var input = File.ReadAllLines(path).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var clawMachines = new List<ClawMachine>();

            for (var i = 0; i < input.Length; i += 3)
            {
                var clawMachine = new ClawMachine
                {
                    ButtonAX = int.Parse(input[i].Split("X+")[1].Split(",")[0]),
                    ButtonAY = int.Parse(input[i].Split("Y+")[1]),
                    ButtonBX = int.Parse(input[i + 1].Split("X+")[1].Split(",")[0]),
                    ButtonBY = int.Parse(input[i + 1].Split("Y+")[1]),
                    PrizeX = int.Parse(input[i + 2].Split("X=")[1].Split(",")[0]),
                    PrizeY = int.Parse(input[i + 2].Split("Y=")[1])
                };

                if (distantPrices)
                {
                    clawMachine.PrizeX += 10000000000000;
                    clawMachine.PrizeY += 10000000000000;
                }

                clawMachines.Add(clawMachine);
            }

            long overallTokens = 0;

            foreach (var cm in clawMachines)
            {
                var leftSideA = (cm.ButtonAX * cm.ButtonBY) - (cm.ButtonAY * cm.ButtonBX);
                var rightSideA = (cm.PrizeX * cm.ButtonBY) - (cm.PrizeY * cm.ButtonBX);
                var a = Math.DivRem(rightSideA, leftSideA, out long remainderA);

                var leftSideB = cm.PrizeX - (cm.ButtonAX * a);
                var rightSideB = cm.ButtonBX;
                var b = Math.DivRem(leftSideB, rightSideB, out long remainderB);

                var isWholeNumberSolution = remainderA == 0 && remainderB == 0;
                var isPositiveNumberSolution = a >= 0 && b >= 0;
                var isLimitedButtonPushes = distantPrices || (a <= 100 && b <= 100);

                if (isWholeNumberSolution && isPositiveNumberSolution && isLimitedButtonPushes)
                {
                    var tokens = (a * 3) + b;
                    overallTokens += tokens;
                }
            }

            Console.WriteLine($"Overall tokens: {overallTokens}");
        }
    }
}
