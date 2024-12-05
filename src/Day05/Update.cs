namespace Day05
{
    internal class Update
    {
        public List<int> PageNumbers { get; set; } = new List<int>();
        public int MiddlePage
        {
            get
            {
                var middleIndex = PageNumbers.Count() / 2;
                return PageNumbers[middleIndex];
            }
        }
    }
}
