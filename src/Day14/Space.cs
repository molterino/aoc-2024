﻿using System.Drawing;

namespace Day14
{
    public class Space
    {
        public char[,] Map { get; }
        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);
        public List<Robot> Robots { get; private set; }
        public int[] RobotsInQuadrants { get; private set; }

        public Space(int width, int height)
        {
            Map = new char[width, height];
        }

        public void InitSpace(string robots)
        {
            Robots = [];
            RobotsInQuadrants = new int[4];

            foreach (var line in File.ReadAllLines(robots))
            {
                var px = int.Parse(line.Split("p=")[1].Split(",")[0]);
                var py = int.Parse(line.Split(",")[1].Split(" ")[0]);
                var vx = int.Parse(line.Split("v=")[1].Split(",")[0]);
                var vy = int.Parse(line.Split(",")[2]);

                var robot = new Robot
                {
                    Position = new Point(px, py),
                    Velocity = new Point(vx, vy)
                };

                Robots.Add(robot);
            }
        }

        public void MoveRobots()
        {
            foreach (var robot in Robots)
            {
                robot.Move(this);
            }
        }

        public void MoveRobots(int seconds)
        {
            for (var i = 0; i < seconds; i++)
            {
                MoveRobots();
            }
        }

        public int GetSafetyFactor()
        {
            var safetyFactor = 1;

            foreach (var robots in RobotsInQuadrants)
            {
                safetyFactor *= robots;
            }

            return safetyFactor;
        }

        public void DisplayMap()
        {
            Console.SetCursorPosition(0, 0);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var robotsOnField = Robots.Count(r => r.Position.X == x && r.Position.Y == y);

                    if (robotsOnField == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(robotsOnField);
                    }
                }
                Console.WriteLine();
            }
        }

        public void DisplayQuadrants()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (y == Height / 2 || x == Width / 2)
                    {
                        Console.Write(' ');
                        continue;
                    }

                    var robotsOnField = Robots.Count(r => r.Position.X == x && r.Position.Y == y);
                    if (robotsOnField == 0)
                    {
                        Console.Write('.');
                        continue;
                    }

                    Console.Write(robotsOnField);
                }
                Console.WriteLine();
            }
        }

        public void CountRobotsInQuadrants()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var midHeight = Height / 2;
                    var midWidth = Width / 2;

                    if (y == midHeight || x == midWidth)
                    {
                        continue;
                    }

                    var robotsOnField = Robots.Count(r => r.Position.X == x && r.Position.Y == y);
                    if (robotsOnField == 0)
                    {
                        continue;
                    }

                    if (x < midWidth && y < midHeight)
                    {
                        RobotsInQuadrants[0] += robotsOnField;
                    }
                    else if (x > midWidth && y < midHeight)
                    {
                        RobotsInQuadrants[1] += robotsOnField;
                    }
                    else if (x < midWidth && y > midHeight)
                    {
                        RobotsInQuadrants[2] += robotsOnField;
                    }
                    else if (x >= midWidth && y > midHeight)
                    {
                        RobotsInQuadrants[3] += robotsOnField;
                    }
                }
            }
        }

        public bool HasAllTheRobotsUniquePosition()
        {
            foreach (var robot in Robots)
            {
                var robotsOnField = Robots.Count(r => r.Position.X == robot.Position.X && r.Position.Y == robot.Position.Y);
                if (robotsOnField > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
