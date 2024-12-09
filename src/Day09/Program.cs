namespace Day09
{
    public static class Program
    {
        private const bool UseTemplateData = false;
        private const string FreeSpace = ".";

        private static void Main(string[] args)
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";
            var diskMap = File.ReadAllText(path);

            var fileBlockId = 0;
            var diskBlocks = new List<string>();

            for(int i = 0; i < diskMap.Length; i++)
            {
                var blockLength = long.Parse(diskMap[i].ToString());
                var block = FreeSpace;

                if (i % 2 == 0) // even field - file, odd field - free space
                {
                    block = fileBlockId.ToString();
                    fileBlockId++;
                }

                for (int j = 0; j < blockLength; j++)
                {
                    diskBlocks.Add(block);
                }
            }

            // refragment
            while (true)
            {
                var indexOfFirstEmptyBlock = diskBlocks.FindIndex(block => block == FreeSpace);
                var indexOfLastFileBlock = diskBlocks.FindLastIndex(block => block != FreeSpace);

                if (indexOfFirstEmptyBlock > indexOfLastFileBlock)
                {
                    break;
                }

                diskBlocks[indexOfFirstEmptyBlock] = diskBlocks[indexOfLastFileBlock];
                diskBlocks[indexOfLastFileBlock] = FreeSpace;
            }

            // calculate filesystem checksum
            long checksum = 0;

            for (int i = 0; i < diskBlocks.FindIndex(x => x == FreeSpace); i++)
            {
                checksum += long.Parse(diskBlocks[i]) * i;
            }

            Console.WriteLine($"What is the resulting filesystem checksum? (Part1): {checksum}"); // 6432869891895
        }
    }
}
