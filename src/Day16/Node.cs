namespace Day16
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Direction { get; set; }
        public (int x, int y) Position => (X, Y);

        public Node(int x, int y, int cost, int direction)
        {
            X = x;
            Y = y;
            Cost = cost;
            Direction = direction;
        }

        public Node((int x, int y) point, int cost, int direction)
        {
            X = point.x;
            Y = point.y;
            Cost = cost;
            Direction = direction;
        }
    }
}
