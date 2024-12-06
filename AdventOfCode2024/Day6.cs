namespace AdventOfCode2024;
internal static class Day6
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/6
        var mapData = File.ReadAllLines("Data/Day6.txt");

        var mapState = SetupMap(mapData);

        var xMax = mapData[0].Length;
        var yMax = mapData.Length;

        var inArea = true;
        var count = 1;

        while (inArea)
        {
            switch (mapState.CurrentDirection)
            {
                case Direction.Up:
                    if (mapState.CurrentPosition.y < 1)
                    {
                        inArea = false;
                        break;
                    }

                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y - 1] == '#')
                    {
                        mapState.CurrentDirection = Direction.Right;
                        break;
                    }

                    mapState.CurrentPosition = (mapState.CurrentPosition.x, mapState.CurrentPosition.y - 1);
                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                    {
                        mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                        count++;
                    }

                    break;

                case Direction.Right:
                    if (mapState.CurrentPosition.x + 1 == xMax)
                    {
                        inArea = false;
                        break;
                    }

                    if (mapState.Map[mapState.CurrentPosition.x + 1, mapState.CurrentPosition.y] == '#')
                    {
                        mapState.CurrentDirection = Direction.Down;
                        break;
                    }

                    mapState.CurrentPosition = (mapState.CurrentPosition.x + 1, mapState.CurrentPosition.y);
                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                    {
                        mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                        count++;
                    }

                    break;

                case Direction.Down:
                    if (mapState.CurrentPosition.y + 1 == yMax)
                    {
                        inArea = false;
                        break;
                    }

                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y + 1] == '#')
                    {
                        mapState.CurrentDirection = Direction.Left;
                        break;
                    }

                    mapState.CurrentPosition = (mapState.CurrentPosition.x, mapState.CurrentPosition.y + 1);
                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                    {
                        mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                        count++;
                    }

                    break;

                case Direction.Left:
                    if (mapState.CurrentPosition.x < 1)
                    {
                        inArea = false;
                        break;
                    }

                    if (mapState.Map[mapState.CurrentPosition.x - 1, mapState.CurrentPosition.y] == '#')
                    {
                        mapState.CurrentDirection = Direction.Up;
                        break;
                    }

                    mapState.CurrentPosition = (mapState.CurrentPosition.x - 1, mapState.CurrentPosition.y);
                    if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                    {
                        mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                        count++;
                    }

                    break;
            }
        }

        Console.WriteLine($"Part 1: {count}");

        var loopCount = 0;

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                mapState = SetupMap(mapData);

                if (mapState.Map[x, y] != '.')
                    continue;

                mapState.Map[x, y] = '#';

                inArea = true;
                var isLoop = false;
                int changedDirectionCount = 0;

                while (inArea && !isLoop)
                {
                    switch (mapState.CurrentDirection)
                    {
                        case Direction.Up:
                            if (mapState.CurrentPosition.y < 1)
                            {
                                inArea = false;
                                break;
                            }

                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y - 1] == '#')
                            {
                                mapState.CurrentDirection = Direction.Right;
                                changedDirectionCount++;
                                break;
                            }

                            mapState.CurrentPosition = (mapState.CurrentPosition.x, mapState.CurrentPosition.y - 1);
                            if (changedDirectionCount >= 4)
                            {
                                isLoop = true;
                                loopCount++;
                            }
                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                            {
                                mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                                changedDirectionCount = 0;
                            }

                            break;

                        case Direction.Right:
                            if (mapState.CurrentPosition.x + 1 == xMax)
                            {
                                inArea = false;
                                break;
                            }

                            if (mapState.Map[mapState.CurrentPosition.x + 1, mapState.CurrentPosition.y] == '#')
                            {
                                changedDirectionCount++;
                                mapState.CurrentDirection = Direction.Down;
                                break;
                            }

                            mapState.CurrentPosition = (mapState.CurrentPosition.x + 1, mapState.CurrentPosition.y);
                            if (changedDirectionCount >= 4)
                            {
                                isLoop = true;
                                loopCount++;
                            }
                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                            {
                                mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                                changedDirectionCount = 0;
                            }

                            break;

                        case Direction.Down:
                            if (mapState.CurrentPosition.y + 1 == yMax)
                            {
                                inArea = false;
                                break;
                            }

                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y + 1] == '#')
                            {
                                changedDirectionCount++;
                                mapState.CurrentDirection = Direction.Left;
                                break;
                            }

                            mapState.CurrentPosition = (mapState.CurrentPosition.x, mapState.CurrentPosition.y + 1);
                            if (changedDirectionCount >= 4)
                            {
                                isLoop = true;
                                loopCount++;
                            }
                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                            {
                                mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                                changedDirectionCount = 0;
                            }

                            break;

                        case Direction.Left:
                            if (mapState.CurrentPosition.x < 1)
                            {
                                inArea = false;
                                break;
                            }

                            if (mapState.Map[mapState.CurrentPosition.x - 1, mapState.CurrentPosition.y] == '#')
                            {
                                changedDirectionCount++;
                                mapState.CurrentDirection = Direction.Up;
                                break;
                            }

                            mapState.CurrentPosition = (mapState.CurrentPosition.x - 1, mapState.CurrentPosition.y);
                            if (changedDirectionCount >= 4)
                            {
                                isLoop = true;
                                loopCount++;
                            }
                            if (mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] != 'X')
                            {
                                mapState.Map[mapState.CurrentPosition.x, mapState.CurrentPosition.y] = 'X';
                                changedDirectionCount = 0;
                            }

                            break;
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {loopCount}");
    }

    private static MapState SetupMap(string[] mapData)
    {
        var map = new char[mapData[0].Length, mapData.Length];
        (int x, int y) currentPosition = new();
        var currentDirection = Direction.Up;

        for (int y = 0; y < mapData.Length; y++)
        {
            for (int x = 0; x < mapData[0].Length; x++)
            {
                map[x, y] = mapData[y][x];

                if (map[x, y] != '.' && map[x, y] != '#')
                {
                    currentPosition = (x, y);

                    currentDirection = map[x, y] switch
                    {
                        '^' => Direction.Up,
                        '>' => Direction.Right,
                        'v' => Direction.Down,
                        '<' => Direction.Right,
                        _ => throw new NotImplementedException()
                    };

                    map[x, y] = 'X';
                }
            }
        }

        return new(map, currentPosition, currentDirection);
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private class MapState(char[,] map, (int, int) currentPosition, Direction currentDirection)
    {
        public char[,] Map { get; set; } = map;
        public (int x, int y) CurrentPosition { get; set; } = currentPosition;
        public Direction CurrentDirection { get; set; } = currentDirection;
    }
}
