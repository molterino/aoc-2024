namespace Day24
{
    public class System
    {
        public List<Wire> Wires { get; set; } = [];
        public List<Gate> Gates { get; set; } = [];

        public void InitWiresAndGates(string[] input)
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

            var systemOutputWiresValue = Wires
                .Where(x => x.Name.StartsWith('z'))
                .OrderByDescending(x => x.Name)
                .Select(x => x.ValueAsString)
                .ToArray();

            var binaryValueAsString = string.Join("", systemOutputWiresValue);
            var result = Convert.ToInt64(binaryValueAsString, 2);

            return result;
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
