namespace Day23
{
    public class Computer
    {
        public string Name { get; }
        public List<string> ConnectedComputerNames { get; } = [];

        public Computer(string name, string connectedComputerName)
        {
            Name = name;
            ConnectedComputerNames.Add(connectedComputerName);
        }

        public void AddConnection(string connectedComputerName)
        {
            var isAlreadyConnected = ConnectedComputerNames.Contains(connectedComputerName);
            if (!isAlreadyConnected)
            {
                ConnectedComputerNames.Add(connectedComputerName);
            }
        }
    }
}
