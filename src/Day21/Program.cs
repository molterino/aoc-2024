using System.Text;

namespace Day21
{
    public static class Program
    {
        private const string Path = "input.txt"; // 237342
        //private const string Path = "input-template.txt"; // 126384

        private static Dictionary<char, (int X, int Y)> _numericKeys;
        private static readonly char[,] _numbericKeypad = new char[,]
        {
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { ' ', '0', 'A' }
        };

        private static Dictionary<char, (int X, int Y)> _directionalKeys;
        private static readonly char[,] _directionalKeypad = new char[,]
        {
            { ' ', '^', 'A' },
            { '<', 'v', '>' }
        };

        private enum KeypadType
        {
            Numeric,
            Directional
        };

        private static void Main(string[] args)
        {
            _numericKeys = InitKeypadKeyPositions(_numbericKeypad);
            _directionalKeys = InitKeypadKeyPositions(_directionalKeypad);

            var robotInstructions = GetInstrucions();
            var overallComplexity = 0;

            foreach (var robotInstruction in robotInstructions)
            {
                var keysPressedByRobotA = GetOverallSteps(robotInstruction, KeypadType.Numeric);
                var keysPressedByRobotB = GetOverallSteps(keysPressedByRobotA, KeypadType.Directional);
                var keysPressedByRobotC = GetOverallSteps(keysPressedByRobotB, KeypadType.Directional);

                overallComplexity += GetComplexity(robotInstruction, keysPressedByRobotC);

                Console.WriteLine($"{robotInstruction}: {keysPressedByRobotC}");
            }

            Console.WriteLine($"\nOverall complexity: {overallComplexity}");
        }

        private static string[] GetInstrucions()
        {
            var lines = File.ReadAllLines(Path);
            return lines;
        }

        private static Dictionary<char, (int x, int y)> InitKeypadKeyPositions(char[,] keypad)
        {
            var keyPositions = new Dictionary<char, (int x, int y)>();
            for (int y = 0; y < keypad.GetLength(1); y++)
            {
                for (int x = 0; x < keypad.GetLength(0); x++)
                {
                    keyPositions[keypad[x, y]] = (x, y);
                }
            }
            return keyPositions;
        }

        private static string GetOverallSteps(string instructions, KeypadType keypadType)
        {
            var overallSteps = string.Empty;

            for (int i = -1; i < instructions.Length - 1; i++)
            {
                var startKey = i == -1 ? 'A' : instructions[i];
                var endKey = instructions[i + 1];
                var steps = GetSteps(keypadType, startKey, endKey);

                overallSteps += steps;
            }

            return overallSteps;
        }

        static string GetSteps(KeypadType keypadType, char startKey, char endKey)
        {
            var steps = new StringBuilder();
            char[] preferredOrder = { '<', '^', 'v', '>' };

            var keys = keypadType == KeypadType.Numeric ? _numericKeys : _directionalKeys;
            var keyPad = keypadType == KeypadType.Numeric ? _numbericKeypad : _directionalKeypad;

            var blindKeyPosition = keys[' '];
            var startKeyPosition = keys[startKey];
            var endKeyPosition = keys[endKey];
            var currentKeyPosition = startKeyPosition;

            while (currentKeyPosition != endKeyPosition)
            {
                if (currentKeyPosition.X < endKeyPosition.X)
                {
                    currentKeyPosition.X++;
                    steps.Append('v');
                }
                else if (currentKeyPosition.X > endKeyPosition.X)
                {
                    currentKeyPosition.X--;
                    steps.Append('^');
                }
                else if (currentKeyPosition.Y < endKeyPosition.Y)
                {
                    currentKeyPosition.Y++;
                    steps.Append('>');
                }
                else if (currentKeyPosition.Y > endKeyPosition.Y)
                {
                    currentKeyPosition.Y--;
                    steps.Append('<');
                }
            }

            var sortedSteps = new string(steps.ToString().OrderBy(c => Array.IndexOf(preferredOrder, c)).ToArray());

            var isInvalidPath = IsInvalidPath(sortedSteps, startKeyPosition, blindKeyPosition);
            if (isInvalidPath)
            {
                sortedSteps = new string(sortedSteps.Reverse().ToArray());
            }

            return sortedSteps + 'A';
        }

        static bool IsInvalidPath(string steps, (int x, int y) start, (int x, int y) blocked)
        {
            int currentX = start.x, currentY = start.y;

            foreach (var step in steps)
            {
                if (step == 'v') currentX++;
                if (step == '^') currentX--;
                if (step == '>') currentY++;
                if (step == '<') currentY--;

                if (currentX == blocked.x && currentY == blocked.y)
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetComplexity(string instruction, string keysPressed)
        {
            var digits = instruction.Where(char.IsDigit).ToArray();
            var digitsString = new string(digits);
            var instructionValue = int.Parse(digitsString);

            return keysPressed.Length * instructionValue;
        }
    }
}
