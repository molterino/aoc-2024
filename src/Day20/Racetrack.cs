namespace Day20
{
    public class Racetrack
    {
        private List<Sector> Sectors { get; } = [];
        private List<Sector> PathSectors { get; } = [];
        private SortedDictionary<int, int> Cheats { get; } = [];

        public Racetrack(string[] racetrackData)
        {
            CreateSectors(racetrackData);
            FindPath();
        }

        public int FindFastestTime()
        {
            return PathSectors.Single(s => s.Type == SectorType.End).SequenceNumber;
        }

        public Dictionary<int, int> GetCheats(int shortcutFilter)
        {
            return Cheats.Where(x => x.Key >= shortcutFilter).ToDictionary();
        }

        public int FindCheatsCount(int range, int shortcutFilter, bool displaySteps)
        {
            var pathSectorsInOrder = PathSectors.OrderBy(s => s.SequenceNumber).ToList();
            var overallShortcutsCount = 0;

            foreach (var sector in pathSectorsInOrder)
            {
                overallShortcutsCount += FindShortcuts(sector, range, shortcutFilter, displaySteps);
            }

            return overallShortcutsCount;
        }

        private void CreateSectors(string[] racetrackData)
        {
            for (int y = 0; y < racetrackData.Length; y++)
            {
                for (int x = 0; x < racetrackData[y].Length; x++)
                {
                    var sectorType = racetrackData[y][x] switch
                    {
                        'S' => SectorType.Start,
                        'E' => SectorType.End,
                        '.' => SectorType.Track,
                        '#' => SectorType.Wall,
                        _ => throw new Exception("Unexpected sector type")
                    };

                    var sector = new Sector(x, y, sectorType);

                    Sectors.Add(sector);
                }
            }
        }

        private void FindPath()
        {
            var startSector = Sectors.Single(s => s.Type == SectorType.Start);
            var endSector = Sectors.Single(s => s.Type == SectorType.End);

            startSector.SequenceNumber = 0;
            PathSectors.Add(startSector);

            var currentSector = startSector;

            while (currentSector.Type != SectorType.End)
            {
                var adjacentSectors = FindAdjacentSectors(currentSector);
                var nextSector = adjacentSectors.Single(s => s.Type != SectorType.Wall && !PathSectors.Contains(s));

                nextSector.SequenceNumber = currentSector.SequenceNumber + 1;
                PathSectors.Add(nextSector);
                currentSector = nextSector;
            }
        }

        private List<Sector> FindAdjacentSectors(Sector currentSector)
        {
            var curPosX = currentSector.PositionX;
            var curPosY = currentSector.PositionY;

            var adjacentSectors = Sectors
                .Where(
                    s => (s.PositionX == curPosX - 1 && s.PositionY == curPosY)
                      || (s.PositionX == curPosX + 1 && s.PositionY == curPosY)
                      || (s.PositionX == curPosX && s.PositionY == curPosY - 1)
                      || (s.PositionX == curPosX && s.PositionY == curPosY + 1))
                .ToList();

            return adjacentSectors;
        }

        private int DistanceBetween(Sector sectorA, Sector sectorB)
        {
            return Math.Abs(sectorA.PositionX - sectorB.PositionX) + Math.Abs(sectorA.PositionY - sectorB.PositionY);
        }

        private int FindShortcuts(Sector currentSector, int range, int shortcutFilter, bool displaySteps)
        {
            var sectorsInRange = PathSectors
                .Where(otherSector => otherSector.SequenceNumber > currentSector.SequenceNumber
                    && DistanceBetween(otherSector, currentSector) <= range)
                .ToList();

            var shortcutSectors = new List<Sector>();

            foreach (var shortcutSector in sectorsInRange)
            {
                var actualDistance = DistanceBetween(currentSector, shortcutSector);
                var cheatDistance = shortcutSector.SequenceNumber - currentSector.SequenceNumber - actualDistance;
                if (cheatDistance >= shortcutFilter)
                {
                    shortcutSectors.Add(shortcutSector);

                    if (Cheats.TryGetValue(cheatDistance, out var count))
                    {
                        Cheats[cheatDistance] = count + 1;
                    }
                    else
                    {
                        Cheats[cheatDistance] = 1;
                    }
                }
            }

            if (displaySteps)
            {
                DisplaySteps(currentSector, sectorsInRange, shortcutSectors, range);
            }

            return shortcutSectors.Count;
        }

        private void DisplaySteps(Sector currentSector, List<Sector> sectorsInRange, List<Sector> shortcutSectors, int range)
        {
            var sectorsToDisplay = Sectors
                .Where(s => (Math.Abs(s.PositionX - currentSector.PositionX) + Math.Abs(s.PositionY - currentSector.PositionY)) <= range * 3)
                .ToList();

            Console.Clear();
            for (int y = sectorsToDisplay.Min(s => s.PositionY); y <= sectorsToDisplay.Max(s => s.PositionY); y++)
            {
                for (int x = sectorsToDisplay.Min(s => s.PositionX); x <= sectorsToDisplay.Max(s => s.PositionX); x++)
                {
                    var sector = sectorsToDisplay.SingleOrDefault(s => s.PositionX == x && s.PositionY == y);
                    var displayChar = sector == null ? ' ' : sector.Type switch
                    {
                        SectorType.Start => 'S',
                        SectorType.End => 'E',
                        SectorType.Track => '.',
                        SectorType.Wall => '#',
                        _ => throw new Exception("Unexpected sector type")
                    };

                    if (sector == currentSector)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (shortcutSectors.Contains(sector))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (sectorsInRange.Contains(sector))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write(displayChar);
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
