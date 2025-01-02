namespace Day25
{
    public class LockSmith
    {
        public List<Schematic> Locks { get; } = [];
        public List<Schematic> Keys { get; } = [];

        public void ForgeSchematics(string[] schematics)
        {
            const int SchematicSize = 7;

            for (int i = 0; i < schematics.Length; i += SchematicSize + 1)
            {
                var data = schematics.ToList().GetRange(i, SchematicSize);
                var schematic = new Schematic(data);

                var isLock = schematic.Type == SchematicType.Lock;
                if (isLock)
                {
                    Locks.Add(schematic);
                }
                else
                {
                    Keys.Add(schematic);
                }
            }
        }

        public int CountGoodLockAndKeyPairs()
        {
            var goodLockAndKeyPairs = 0;

            foreach (var @lock in Locks)
            {
                foreach (var key in Keys)
                {
                    var isKeyFitInLock = IsKeyFitInLock(@lock, key);
                    if (isKeyFitInLock)
                    {
                        goodLockAndKeyPairs++;
                    }
                }
            }

            return goodLockAndKeyPairs;
        }

        private bool IsKeyFitInLock(Schematic @lock, Schematic key)
        {
            for (int i = 0; i < key.Height.Count; i++)
            {
                var overlap = key.Height[i] + @lock.Height[i] > 5;
                if (overlap)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
