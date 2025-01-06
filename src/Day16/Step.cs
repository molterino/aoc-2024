namespace Day16
{
    public class Step
    {
        public Position Position { get; }
        public Direction Direction { get; }

        public Step(Position position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public override bool Equals(object? obj) => obj is Step other && Position.Equals(other.Position) && Direction.Equals(other.Direction);
        public override int GetHashCode() => HashCode.Combine(Position, Direction);
    }
}
