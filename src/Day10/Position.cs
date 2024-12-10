namespace Day10
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public List<Position> Neighbors { get; set; } = [];

        public bool IsTrailHead => Height == 0;
    }
}
