namespace Day16
{
    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Add(Direction d) => new(X + d.DX, Y + d.DY);

        public override bool Equals(object? obj) => obj is Position other && X == other.X && Y == other.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
