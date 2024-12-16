namespace Day15
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            const char Robot = '@';
            const char Wall = '#';
            const char Box = 'O';
            const char FreeSpace = '.';

            var path = "input.txt"; // 1465523
            //var path = "input-template-1.txt"; // 2028
            //var path = "input-template-2.txt"; // 10092
            var warehouseDocument = File.ReadAllLines(path);

            // init map
            var warehouseMapRaw = warehouseDocument.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var warehouseMap = new char[warehouseMapRaw.Length, warehouseMapRaw[0].Length];

            var robotPositionX = 0;
            var robotPositionY = 0;

            for (var i = 0; i < warehouseMapRaw.Length; i++)
            {
                for (var j = 0; j < warehouseMapRaw[i].Length; j++)
                {
                    warehouseMap[i, j] = warehouseMapRaw[i][j];

                    if (warehouseMap[i, j] == Robot)
                    {
                        robotPositionX = i;
                        robotPositionY = j;
                    }
                }
            }

            // init movements
            var robotMovementCommandsRaw = warehouseDocument.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).ToList();
            var robotMovementCommands = string.Concat(robotMovementCommandsRaw).Replace(Environment.NewLine, "");

            char[] RobotMovementCommands = ['^', 'v', '<', '>'];
            int[] RobotMovementX = [-1, 1, 0, 0];
            int[] RobotMovementY = [0, 0, -1, 1];

            // move robot
            foreach (var command in robotMovementCommands)
            {
                var commandIndex = Array.IndexOf(RobotMovementCommands, command);
                var nextPositionX = robotPositionX + RobotMovementX[commandIndex];
                var nextPositionY = robotPositionY + RobotMovementY[commandIndex];
                var nextPosition = warehouseMap[nextPositionX, nextPositionY];

                if (nextPosition == FreeSpace)
                {
                    warehouseMap[robotPositionX, robotPositionY] = FreeSpace;
                    warehouseMap[nextPositionX, nextPositionY] = Robot;
                    robotPositionX = nextPositionX;
                    robotPositionY = nextPositionY;
                }
                else if (nextPosition == Box)
                {
                    var afterNextPositionX = nextPositionX + RobotMovementX[commandIndex];
                    var afterNextPositionY = nextPositionY + RobotMovementY[commandIndex];
                    var afterNextPosition = warehouseMap[afterNextPositionX, afterNextPositionY];

                    while (afterNextPosition != Wall)
                    {
                        if (afterNextPosition == FreeSpace)
                        {
                            warehouseMap[robotPositionX, robotPositionY] = FreeSpace;
                            warehouseMap[nextPositionX, nextPositionY] = Robot;
                            warehouseMap[afterNextPositionX, afterNextPositionY] = Box;
                            robotPositionX = nextPositionX;
                            robotPositionY = nextPositionY;
                            break;
                        }

                        afterNextPositionX += RobotMovementX[commandIndex];
                        afterNextPositionY += RobotMovementY[commandIndex];
                        afterNextPosition = warehouseMap[afterNextPositionX, afterNextPositionY];
                    }
                }
            }

            // display map
            for (var i = 0; i < warehouseMap.GetLength(0); i++)
            {
                for (var j = 0; j < warehouseMap.GetLength(1); j++)
                {
                    Console.Write(warehouseMap[i, j]);
                }
                Console.WriteLine();
            }

            // calculate gps
            var gps = 0;
            for (var i = 0; i < warehouseMap.GetLength(0); i++)
            {
                for (var j = 0; j < warehouseMap.GetLength(1); j++)
                {
                    if (warehouseMap[i, j] == Box)
                    {
                        gps += (100 * i) + j;
                    }
                }
            }

            Console.WriteLine($"\nGPS: {gps}");
        }
    }
}
