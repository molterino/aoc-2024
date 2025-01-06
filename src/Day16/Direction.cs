namespace Day16
{
    public class Direction
    {
        public int DX { get; }
        public int DY { get; }

        public Direction(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }

        public Direction Rotate() => new(-DY, DX);

        public override bool Equals(object? obj) => obj is Direction other && DX == other.DX && DY == other.DY;
        public override int GetHashCode() => HashCode.Combine(DX, DY);
    }
}
