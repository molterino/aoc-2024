namespace Day11
{
    public static class Extensions
    {
        public static void AddOrUpdate(this Dictionary<long, long> dictionary, long key, long value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] += value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
