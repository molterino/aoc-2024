namespace Day13
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var path = "input.txt"; // 29436 / ?
            //var path = "input-template.txt"; // 480 / ?
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
                clawMachines.Add(clawMachine);
            }

            var overallTokens = 0;

            foreach (var cm in clawMachines)
            {
                var leftSideA = (cm.ButtonAX * cm.ButtonBY) - (cm.ButtonAY * cm.ButtonBX);
                var rightSideA = (cm.PrizeX * cm.ButtonBY) - (cm.PrizeY * cm.ButtonBX);
                var a = Math.DivRem(rightSideA, leftSideA, out int remainderA);

                var leftSideB = cm.PrizeX - (cm.ButtonAX * a);
                var rightSideB = cm.ButtonBX;
                var b = Math.DivRem(leftSideB, rightSideB, out int remainderB) ;

                var isWholeNumberSolution = remainderA == 0 && remainderB == 0;
                var isPositiveNumberSolution = a >= 0 && b >= 0;
                var isPlayableSolution = a <= 100 && b <= 100;

                if (isWholeNumberSolution && isPositiveNumberSolution && isPlayableSolution)
                {
                    var tokens = ((int)a * 3) + (int)b;
                    overallTokens += tokens;
                    Console.WriteLine($"A: {a}, B: {b}, tokens: {tokens}");
                }
                else
                {
                    Console.WriteLine($"There is no valid solution! A: {a}, B: {b}, WholeNums:{isWholeNumberSolution}, PosNums:{isPositiveNumberSolution}, 100+:{isPlayableSolution}");
                }
            }

            Console.WriteLine($"Overall tokens (Part 1): {overallTokens}");
        }
    }
}
