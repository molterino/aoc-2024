namespace Day07
{
    public static class Program
    {
        private const bool UseTemplateData = false;

        private static void Main(string[] args)
        {
            var equations = ReadEquations();

            // Part1: Determine which equations could possibly be true. What is their total calibration result?
            var operators = "+*";
            long totalCalibrationResult = CalculateTotalCalibrationResult(operators, equations);
            Console.WriteLine($"What is their total calibration result? (Part1): {totalCalibrationResult}"); //12940396350192

            // Part2: New operator to concatenate numbers. What is the total calibration result for valid equations?
            var operatorsExtended = "+*|";
            long totalCalibrationResultExtended = CalculateTotalCalibrationResult(operatorsExtended, equations);
            Console.WriteLine($"What is their total calibration result? (Part2): {totalCalibrationResultExtended}"); //106016735664498
        }

        private static List<string> GenerateOperatorCombinations(string operators, int length)
        {
            var results = new List<string>();
            int totalCombinations = (int)Math.Pow(operators.Length, length);

            for (int i = 0; i < totalCombinations; i++)
            {
                var combination = "";
                int current = i;

                for (int j = 0; j < length; j++)
                {
                    combination += operators[current % operators.Length];
                    current /= operators.Length;
                }

                results.Add(combination);
            }

            return results;
        }

        private static long CalculateEquation(List<long> numbers, string operators)
        {
            long result = numbers[0];

            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] == '+')
                {
                    result += numbers[i + 1];
                }
                else if (operators[i] == '*')
                {
                    result *= numbers[i + 1];
                }
                else if (operators[i] == '|')
                {
                    result = long.Parse(result.ToString() + numbers[i + 1].ToString());
                }
            }

            return result;
        }

        private static long CalculateTotalCalibrationResult(string operators, List<Equation> equations)
        {
            long totalCalibrationResult = 0;

            foreach (var equation in equations)
            {
                var operatorCombinations = GenerateOperatorCombinations(operators, equation.Numbers.Count - 1);
                foreach (var operatorCombination in operatorCombinations)
                {
                    var result = CalculateEquation(equation.Numbers, operatorCombination);

                    if (equation.TestValue == result)
                    {
                        totalCalibrationResult += result;
                        break;
                    }
                }
            }

            return totalCalibrationResult;
        }

        private static List<Equation> ReadEquations()
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";

            try
            {
                var lines = File.ReadAllLines(path);
                var equations = new List<Equation>();

                foreach (var line in lines)
                {
                    var equationParts = line.Split(":");
                    var testValue = long.Parse(equationParts[0]);
                    var numbers = equationParts[1]
                        .Split(" ")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(long.Parse)
                        .ToList();

                    var equation = new Equation
                    {
                        TestValue = testValue,
                        Numbers = numbers
                    };

                    equations.Add(equation);
                }

                return equations;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during file processing: {e.Message}");
                return [];
            }
        }
    }
}
