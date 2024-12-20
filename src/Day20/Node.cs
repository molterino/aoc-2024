namespace Day20
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public (int x, int y) Position => (X, Y);

        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
        }

        public Node((int x, int y) point, int cost)
        {
            X = point.x;
            Y = point.y;
            Cost = cost;
        }
    }
}
