namespace Day24
{
    public class RippleCarryAdder
    {
        public List<Wire> Wires { get; set; } = [];
        public List<Gate> Gates { get; set; } = [];

        public RippleCarryAdder(string[] input)
        {
            InitWiresAndGates(input);
        }

        public long Evaluate()
        {
            while (true)
            {
                var gatesToEvaluate = Gates.Where(g => g.IsEvaluable).ToList();

                if (gatesToEvaluate.Count == 0)
                {
                    break;
                }

                foreach (var gate in gatesToEvaluate)
                {
                    gate.Evaluate();
                }
            }

            var wires = Wires
                .Where(x => x.Name.StartsWith('z'))
                .OrderByDescending(x => x.Name)
                .Select(x => x.ValueAsString)
                .ToArray();

            var binaryValueAsString = string.Concat(wires);
            var binaryValueAsDecimal = Convert.ToInt64(binaryValueAsString, 2);

            return binaryValueAsDecimal;
        }

        public string GetSwappedOutputWires()
        {
            var invalidGatesRule1 = Gates
                .Where(x => x.Type != GateType.XOR
                    && x.OutputWire.Name.Contains('z')
                    && x.OutputWire.Name != "z45")
                .Select(x => x.OutputWire.Name)
                .ToList();

            var invalidGatesRule2 = Gates
                .Where(x => x.Type == GateType.XOR
                    && !x.OutputWire.Name.Contains('z')
                    && !x.InputWireA.Name.Contains('x') && !x.InputWireB.Name.Contains('x')
                    && !x.InputWireA.Name.Contains('y') && !x.InputWireB.Name.Contains('y'))
                .Select(x => x.OutputWire.Name)
                .ToList();

            var invalidGatesRule3 = Gates
                .Where(x => x.Type == GateType.XOR
                    && (x.InputWireA.Name.Contains('x') || x.InputWireB.Name.Contains('x') || x.InputWireA.Name.Contains('y') || x.InputWireB.Name.Contains('y'))
                    && !Gates.Any(y => y.Type == GateType.XOR && (y.InputWireA.Name == x.OutputWire.Name || y.InputWireB.Name == x.OutputWire.Name))
                    && !x.InputWireA.Name.Contains("00"))
                .Select(x => x.OutputWire.Name)
                .ToList();

            var invalidGatesRule4 = Gates
                .Where(x => x.Type == GateType.AND
                    && (x.InputWireA.Name.Contains('x') || x.InputWireB.Name.Contains('x') || x.InputWireA.Name.Contains('y') || x.InputWireB.Name.Contains('y'))
                    && !Gates.Any(y => y.Type == GateType.OR && (y.InputWireA.Name == x.OutputWire.Name || y.InputWireB.Name == x.OutputWire.Name))
                    && !x.InputWireA.Name.Contains("00"))
                .Select(x => x.OutputWire.Name)
                .ToList();

            var invalidGates = invalidGatesRule1
                .Concat(invalidGatesRule2)
                .Concat(invalidGatesRule3)
                .Concat(invalidGatesRule4)
                .Distinct()
                .ToList();

            invalidGates.Sort();

            return string.Join(",", invalidGates);
        }

        private void InitWiresAndGates(string[] input)
        {
            var wiresWithInput = input.TakeWhile(x => x.Length > 0);

            foreach (var wireWithInput in wiresWithInput)
            {
                var wireName = wireWithInput.Substring(0, 3);
                var wireValue = wireWithInput.Substring(5, 1) == "1";

                var wire = new Wire(wireName, wireValue);
                Wires.Add(wire);
            }

            var gatesWithWires = input.SkipWhile(x => x.Length > 0).Skip(1);

            foreach (var gateWithWire in gatesWithWires)
            {
                var gateParts = gateWithWire.Split(' ').Where(x => x != "->").ToArray();
                var gateType = Enum.Parse<GateType>(gateParts[1]);
                var inputWireA = GetOrCreateWire(gateParts[0]);
                var inputWireB = GetOrCreateWire(gateParts[2]);
                var outputWire = GetOrCreateWire(gateParts[3]);

                var gate = new Gate(gateType, inputWireA, inputWireB, outputWire);
                Gates.Add(gate);
            }
        }

        private Wire GetOrCreateWire(string wireName)
        {
            var wire = Wires.SingleOrDefault(x => x.Name == wireName);
            if (wire == null)
            {
                wire = new Wire(wireName);
                Wires.Add(wire);
            }

            return wire;
        }
    }
}
