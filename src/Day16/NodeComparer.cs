namespace Day16
{
    public  class NodeComparer : IComparer<Node>
    {
        public int Compare(Node a, Node b)
        {
            int costDifference = a.Cost.CompareTo(b.Cost);
            if (costDifference == 0)
            {
                int positionDifferenceX = a.X.CompareTo(b.X);
                if (positionDifferenceX == 0)
                {
                    int positionDifferenceY = a.Y.CompareTo(b.Y);
                    return positionDifferenceY;
                }
                return positionDifferenceX;
            }
            return costDifference;
        }
    }
}
