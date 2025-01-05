namespace Day17
{
    public static class Processor
    {
        private static long registerA;
        private static long registerB;
        private static long registerC;

        private static List<int> instructions = [];
        private static List<long> output = [];

        public static void Init(string[] data)
        {
            instructions = data[4].Split(": ")[1].Split(',').Select(int.Parse).ToList();
            registerA = long.Parse(data[0].Split(": ")[1]);
            registerB = long.Parse(data[1].Split(": ")[1]);
            registerC = long.Parse(data[2].Split(": ")[1]);
            output = [];
        }

        public static List<long> ExecuteProgram()
        {
            output = [];
            int instructionPointer = 0;

            while (instructionPointer < instructions.Count)
            {
                int opcode = instructions[instructionPointer];
                int operandLiteral = instructions[instructionPointer + 1];
                long operandCombo = CalculateOperandCombo(operandLiteral);

                var shouldIncrementInstructionPointer = true;
                switch (opcode)
                {
                    case 0: // adv + combo
                        registerA = (long)(registerA / Math.Pow(2, operandCombo));
                        break;
                    case 1: // bxl + literal
                        registerB ^= operandLiteral;
                        break;
                    case 2: // bst + combo
                        registerB = operandCombo % 8;
                        break;
                    case 3: // jnz + literal
                        if (registerA != 0)
                        {
                            instructionPointer = operandLiteral;
                            shouldIncrementInstructionPointer = false;
                        }
                        break;
                    case 4: // bxc + ignores operand
                        registerB ^= registerC;
                        break;
                    case 5: // out + combo
                        output.Add(operandCombo % 8);
                        break;
                    case 6: // bdv - like adv
                        registerB = (long)(registerA / Math.Pow(2, operandCombo));
                        break;
                    case 7: // cdv - like adv
                        registerC = (long)(registerA / Math.Pow(2, operandCombo));
                        break;
                }

                if (shouldIncrementInstructionPointer)
                {
                    instructionPointer += 2;
                }
            }

            return output;
        }

        public static long TryToCopyItself()
        {
            long registerValue = 0;
            long upperBound = (long) Math.Pow(8, instructions.Count);

            for (registerValue = 0; registerValue < upperBound; registerValue++)
            {
                SetRegisters(a: registerValue, b: 0, c: 0);
                ExecuteProgram();

                var hasSameLastDigits = CompareLastDigits();
                if (hasSameLastDigits)
                {
                    var hasSameNumberOfDigits = output.Count == instructions.Count;
                    if (hasSameNumberOfDigits)
                    {
                        break;
                    }

                    registerValue = (registerValue * 8) - 1;
                }
            }

            return registerValue;
        }

        public static void PrintStatus()
        {
            Console.WriteLine($"Instructions: {string.Join(',', instructions)}");
            Console.WriteLine($"A register value: {registerA}");
            Console.WriteLine($"B register value: {registerB}");
            Console.WriteLine($"C register value: {registerC}");
            Console.WriteLine($"Output: {string.Join(",", output)}");
        }

        private static long CalculateOperandCombo(long operandLiteral)
        {
            long operandCombo = -1;

            switch (operandLiteral)
            {
                case 4:
                    operandCombo = registerA;
                    break;
                case 5:
                    operandCombo = registerB;
                    break;
                case 6:
                    operandCombo = registerC;
                    break;
                case 7: // invalid
                    break;
                default: // literal
                    operandCombo = operandLiteral;
                    break;
            }

            return operandCombo;
        }

        private static bool CompareLastDigits()
        {
            var startIndex = instructions.Count - output.Count;
            var lastDigits = instructions.Skip(startIndex).ToList();
            var isLastDigitsMatching = true;

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] != lastDigits[i])
                {
                    isLastDigitsMatching = false;
                    break;
                }
            }

            return isLastDigitsMatching;
        }

        private static void SetRegisters(long a, long b, long c)
        {
            registerA = a;
            registerB = b;
            registerC = c;
        }
    }
}
