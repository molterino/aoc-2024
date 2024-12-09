namespace Day09
{
    public static class Program
    {
        private const bool UseTemplateData = false;
        private const string FreeSpace = ".";

        private static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";
            var diskMap = File.ReadAllText(path);

            var fileBlockId = 0;
            var diskBlocks = new List<string>();

            for (int i = 0; i < diskMap.Length; i++)
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

        private static void Part2()
        {
            var path = UseTemplateData ? "input-template.txt" : "input.txt";
            var diskMap = File.ReadAllText(path);

            var fileBlockId = 0;
            var diskBlocks = new List<string>();

            for (int i = 0; i < diskMap.Length; i++)
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
            for(int i = fileBlockId - 1; i >= 0; i--)
            {
                var startIndexOfCurrentFileBlock = diskBlocks.FindIndex(block => block == i.ToString());
                var endIndexOfCurrentFileBlock = diskBlocks.FindLastIndex(block => block == i.ToString());
                var currentFileBlockSize = endIndexOfCurrentFileBlock - startIndexOfCurrentFileBlock + 1;

                var startIndexOfSufficientFreeSpaceBlock = FindSufficientFreeSpaceBlockStartIndex(diskBlocks, currentFileBlockSize);
                if (startIndexOfSufficientFreeSpaceBlock != -1)
                {
                    MoveFileBlock(diskBlocks, startIndexOfSufficientFreeSpaceBlock, startIndexOfCurrentFileBlock, currentFileBlockSize);
                }
            }

            // calculate filesystem checksum
            long checksum = 0;

            for (int i = 0; i < diskBlocks.Count - 1; i++)
            {
                if (diskBlocks[i] != FreeSpace)
                {
                    checksum += long.Parse(diskBlocks[i]) * i;
                }
            }

            Console.WriteLine($"What is the resulting filesystem checksum? (Part2): {checksum}"); // 6467290479134
        }

        private static int FindSufficientFreeSpaceBlockStartIndex(List<string> diskBlocks, int requiredSize)
        {
            int size = 0;

            for (int i = 0; i < diskBlocks.Count; i++)
            {
                if (diskBlocks[i] == FreeSpace)
                {
                    size++;

                    if (size == requiredSize)
                    {
                        return i - requiredSize + 1;
                    }
                }
                else
                {
                    size = 0;
                }
            }

            return -1;
        }

        private static void MoveFileBlock(List<string> diskBlocks, int startIndexOfFreeSpaceBlock, int startIndexOfFileBlock, int size)
        {
            if (startIndexOfFileBlock < startIndexOfFreeSpaceBlock)
            {
                return;
            }

            var fileBlockId = diskBlocks[startIndexOfFileBlock];

            for (int i = 0; i < size; i++)
            {
                diskBlocks[startIndexOfFreeSpaceBlock + i] = fileBlockId;
                diskBlocks[startIndexOfFileBlock + i] = FreeSpace;
            }
        }
    }
}
