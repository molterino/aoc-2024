using System.Text;

namespace Day21
{
    public static class Program
    {
        //private const int DirPadRobotsCount = 2;
        private const int DirPadRobotsCount = 25;
        private const string Path = "input.txt"; // 237342 / 294585598101704
        //private const string Path = "input-template.txt"; // 126384

        private static readonly char[,] _numericKeypad = new char[,]
        {
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { ' ', '0', 'A' }
        };

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

        private static Dictionary<char, (int X, int Y)> _numericKeys = [];
        private static Dictionary<char, (int X, int Y)> _directionalKeys = [];
        private static Dictionary<string, string> _stepsMemo = [];
        private static Dictionary<(string, int), long> _dirPadInstructionsMemo = [];

        private static void Main(string[] args)
        {
            InitKeyPositions(_numericKeypad);
            InitKeyPositions(_directionalKeypad);
            InitStepsMemo();

            long complexity = 0;

            foreach (var robotInstruction in File.ReadAllLines(Path))
            {
                var numPadInstructions = GetNumPadInstructions(robotInstruction);
                var instructionsLength = GetInstructionsLength(numPadInstructions);

                complexity += GetComplexity(robotInstruction, instructionsLength);
            }

            Console.WriteLine($"Overall complexity with {DirPadRobotsCount} directional keypad robots: {complexity}");
        }

        private static void InitKeyPositions(char[,] keypad)
        {
            var keyPositions = new Dictionary<char, (int x, int y)>();

            for (int y = 0; y < keypad.GetLength(1); y++)
            {
                for (int x = 0; x < keypad.GetLength(0); x++)
                {
                    keyPositions[keypad[x, y]] = (x, y);
                }
            }

            var isNumeric = keyPositions.ContainsKey('0');
            if (isNumeric)
            {
                _numericKeys = keyPositions;
            }
            else
            {
                _directionalKeys = keyPositions;
            }
        }

        private static void InitStepsMemo()
        {
            var keys = _directionalKeys.Where(x => x.Key != ' ').Select(x => x.Key);

            foreach (var from in keys)
            {
                foreach (var to in keys)
                {
                    _stepsMemo[$"{from}{to}"] = GetStepsBetweenKeys(KeypadType.Directional, from, to);
                }
            }
        }

        private static string GetStepsBetweenKeys(KeypadType keypadType, char startKey, char endKey)
        {
            var steps = new StringBuilder();
            char[] preferredOrder = { '<', '^', 'v', '>' };

            var keys = keypadType == KeypadType.Numeric ? _numericKeys : _directionalKeys;
            var keyPad = keypadType == KeypadType.Numeric ? _numericKeypad : _directionalKeypad;

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

        private static bool IsInvalidPath(string steps, (int x, int y) start, (int x, int y) blocked)
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

        private static string GetNumPadInstructions(string instructions)
        {
            var overallSteps = string.Empty;

            for (int i = -1; i < instructions.Length - 1; i++)
            {
                var startKey = i == -1 ? 'A' : instructions[i];
                var endKey = instructions[i + 1];
                var steps = GetStepsBetweenKeys(KeypadType.Numeric, startKey, endKey);

                overallSteps += steps;
            }

            return overallSteps;
        }

        private static long GetInstructionsLength(string instruction, int robotSequenceNumber = 1)
        {
            bool existingMemo = _dirPadInstructionsMemo.TryGetValue((instruction, robotSequenceNumber), out long instructionLength);
            if (existingMemo)
            {
                return instructionLength;
            }

            char currentKey = 'A';

            foreach (var key in instruction)
            {
                var stepsMemoKey = new string([currentKey, key]);
                var isLastRobot = robotSequenceNumber == DirPadRobotsCount;

                if (isLastRobot)
                {
                    instructionLength += _stepsMemo[stepsMemoKey].Length;
                }
                else
                {
                    var nextSteps = _stepsMemo[stepsMemoKey];
                    var nextRobot = robotSequenceNumber + 1;
                    instructionLength += GetInstructionsLength(nextSteps, nextRobot);
                }

                currentKey = key;
            }

            _dirPadInstructionsMemo[(instruction, robotSequenceNumber)] = instructionLength;
            return instructionLength;
        }

        private static long GetComplexity(string instruction, long keysPressedCount)
        {
            var digits = instruction.Where(char.IsDigit).ToArray();
            var digitsString = new string(digits);
            var instructionValue = int.Parse(digitsString);

            return keysPressedCount * instructionValue;
        }
    }
}
