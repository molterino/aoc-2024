namespace Day12
{
    public class Region
    {
        public Region(int id, char type)
        {
            Id = id;
            Type = type;
        }

        public int Id { get; set; }
        public char Type { get; set; }
        public int Area { get; set; }
        public int Perimeter { get; set; }
        public int FencingPrice => Area * Perimeter;
    }
}
