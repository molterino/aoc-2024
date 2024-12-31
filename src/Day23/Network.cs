namespace Day23
{
    public class Network
    {
        public string[] Connections { get; }
        public List<Computer> Computers { get; }

        public Network(string[] connections)
        {
            Connections = connections;
            Computers = InitComputers(connections);
        }

        public int CountThreeComputerGroups()
        {
            var groups = new SortedSet<string>();

            foreach (var computer in Computers)
            {
                for (int i = 0; i < computer.ConnectedComputerNames.Count - 1; i++)
                {
                    for (int j = i + 1; j < computer.ConnectedComputerNames.Count; j++)
                    {
                        var a = computer.ConnectedComputerNames[i];
                        var b = computer.ConnectedComputerNames[j];
                        var ab = a + "-" + b;
                        var ba = b + "-" + a;
                        var isExistingConnection = Connections.Contains(ab) || Connections.Contains(ba);

                        if (!isExistingConnection)
                        {
                            continue;
                        }

                        var group = new SortedSet<string>() { computer.Name, a, b };
                        var isChiefHistorianComputerInGroup = group.Any(x => x.StartsWith('t'));

                        if (!isChiefHistorianComputerInGroup)
                        {
                            continue;
                        }

                        var groupLiteral = string.Join(",", group);
                        groups.Add(groupLiteral);
                    }
                }
            }

            return groups.Count;
        }

        private static List<Computer> InitComputers(string[] connections)
        {
            var network = new List<Computer>();

            foreach (var connection in connections)
            {
                var computerNameA = connection.Substring(0, 2);
                var computerNameB = connection.Substring(3, 2);

                AddOrUpdateComputer(network, computerNameA, computerNameB);
                AddOrUpdateComputer(network, computerNameB, computerNameA);
            }

            return network;
        }

        private static void AddOrUpdateComputer(List<Computer> network, string computerName, string connectedComputerName)
        {
            var computer = network.FirstOrDefault(x => x.Name == computerName);
            if (computer is null)
            {
                computer = new Computer(computerName, connectedComputerName);
                network.Add(computer);
            }
            else
            {
                computer.ConnectedComputerNames.Add(connectedComputerName);
            }
        }
    }
}
