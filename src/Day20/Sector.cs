namespace Day20
{
    public class Sector
    {
        public int PositionX { get; }
        public int PositionY { get; }
        public SectorType Type { get; }
        public int SequenceNumber { get; set; }

        public Sector(int positionX, int positionY, SectorType type)
        {
            PositionX = positionX;
            PositionY = positionY;
            Type = type;
            SequenceNumber = -1;
        }

        public override string ToString()
        {
            return $"Pos:{PositionX},{PositionY}, Type:{Type}, Seqno:{SequenceNumber}";
        }
    }
}
