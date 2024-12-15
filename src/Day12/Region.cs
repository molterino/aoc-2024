using System.Drawing;

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
        public int Area => Fields.Count;
        public int Perimeter { get; set; }
        public int FencingPrice => Perimeter * Area;
        public int OuterCorners { get; set; }
        public int InnerCorners { get; set; }
        public int Corners => OuterCorners + InnerCorners;
        public int FencingPriceWithDiscount => Corners * Area;
        public List<Point> Fields { get; set; } = [];
    }
}
