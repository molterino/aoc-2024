namespace Day24
{
    public class Wire
    {
        public string Name { get; }
        public bool Value { get; private set; }
        public bool HasValue { get; private set; }

        public string ValueAsString => HasValue ? Value ? "1" : "0" : "null";

        public Wire(string name)
        {
            Name = name;
            HasValue = false;
        }

        public Wire(string name, bool value)
        {
            Name = name;
            Value = value;
            HasValue = true;
        }

        public void SetValue(bool value)
        {
            Value = value;
            HasValue = true;
        }

        public override string ToString()
        {
            return $"{Name}: {ValueAsString}";
        }
    }
}
