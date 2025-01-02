
namespace Day25
{
    public class Schematic
    {
        public List<int> Height { get; } = [];
        public SchematicType Type { get; private set; }

        public Schematic(List<string> schematic)
        {
            Create(schematic);
        }

        private void Create(List<string> schematic)
        {
            var isLock = schematic[0][0] == '#';
            if (isLock)
            {
                Type = SchematicType.Lock;
            }
            else
            {
                Type = SchematicType.Key;
            };

            for (int j = 0; j < schematic[0].Length; j++)
            {
                var height = 0;

                for (int i = 0; i < schematic.Count; i++)
                {
                    if (schematic[i][j] == '#')
                    {
                        height++;
                    }
                }

                Height.Add(height - 1);
            }
        }

        public override string ToString()
        {
            return $"{string.Join(",", Height)}";
        }
    }
}
