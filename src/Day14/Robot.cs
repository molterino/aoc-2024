using System.Drawing;

namespace Day14
{
    public class Robot
    {
        public Point Position { get; set; }
        public Point Velocity { get; set; }
        public Point NextPostion => new(Position.X + Velocity.X, Position.Y + Velocity.Y);

        public void Move(Space space)
        {
            var nextPostionX = -1;

            if (NextPostion.X < 0)
            {
                nextPostionX = space.Width + NextPostion.X;
            }
            else if (NextPostion.X >= space.Width)
            {
                nextPostionX = NextPostion.X - space.Width;
            }
            else
            {
                nextPostionX = NextPostion.X;
            }

            var nextPostionY = -1;

            if (NextPostion.Y < 0)
            {
                nextPostionY = space.Height + NextPostion.Y;
            }
            else if (NextPostion.Y >= space.Height)
            {
                nextPostionY = NextPostion.Y - space.Height;
            }
            else
            {
                nextPostionY = NextPostion.Y;
            }

            Position = new Point(nextPostionX, nextPostionY);
        }
    }
}
