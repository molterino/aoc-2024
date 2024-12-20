namespace Day17
{
    public static class Processor
    {
        private static int registerA = 0;
        private static int registerB = 0;
        private static int registerC = 0;

        private static List<int> instructions = [];
        private static List<int> output = [];

        public static void InitRegisters(string path)
        {
            var input = File.ReadAllLines(path);

            registerA = int.Parse(input[0].Split(": ")[1]);
            registerB = int.Parse(input[1].Split(": ")[1]);
            registerC = int.Parse(input[2].Split(": ")[1]);
        }

        public static void InitProgram(string path)
        {
            var input = File.ReadAllLines(path);

            instructions = input[4].Split(": ")[1].Split(',').Select(int.Parse).ToList();
        }

        public static void ExecuteProgram()
        {
            var instructionPointer = 0;
            while (instructionPointer < instructions.Count)
            {
                var opcode = instructions[instructionPointer];
                var operandLiteral = instructions[instructionPointer + 1];
                var operandCombo = CalculateOperandCombo(operandLiteral);

                var shouldIncrementInstructionPointer = true;
                switch (opcode)
                {
                    case 0: // adv + combo
                        registerA = (int)(registerA / Math.Pow(2, operandCombo));
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
                        registerB = (int)(registerA / Math.Pow(2, operandCombo));
                        break;
                    case 7: // cdv - like adv
                        registerC = (int)(registerA / Math.Pow(2, operandCombo));
                        break;
                }

                if (shouldIncrementInstructionPointer)
                {
                    instructionPointer += 2;
                }
            }
        }

        public static void PrintStatus()
        {
            Console.WriteLine($"A register value: {registerA}");
            Console.WriteLine($"B register value: {registerB}");
            Console.WriteLine($"C register value: {registerC}");
            Console.WriteLine($"Output: {string.Join(",", output)}");
        }

        private static int CalculateOperandCombo(int operandLiteral)
        {
            var operandCombo = -1;

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
    }
}
