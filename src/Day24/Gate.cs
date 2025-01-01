namespace Day24
{
    public class Gate
    {
        public GateType Type { get; set; }
        public Wire InputWireA { get; set; }
        public Wire InputWireB { get; set; }
        public Wire OutputWire { get; set; }

        public bool HasInput => InputWireA.HasValue && InputWireB.HasValue;
        public bool HasOutput => OutputWire.HasValue;
        public bool IsEvaluable => HasInput && !HasOutput;

        public Gate(GateType type, Wire inputA, Wire inputB, Wire output)
        {
            Type = type;
            InputWireA = inputA;
            InputWireB = inputB;
            OutputWire = output;
        }

        public void Evaluate()
        {
            if (!HasInput)
            {
                return;
            }

            switch (Type)
            {
                case GateType.AND:
                    OutputWire.SetValue(InputWireA.Value & InputWireB.Value);
                    break;
                case GateType.OR:
                    OutputWire.SetValue(InputWireA.Value | InputWireB.Value);
                    break;
                case GateType.XOR:
                    OutputWire.SetValue(InputWireA.Value ^ InputWireB.Value);
                    break;
            }
        }

        public override string ToString()
        {
            return $"{InputWireA.Name} {Type} {InputWireB.Name} -> {OutputWire.Name}";
        }
    }
}
