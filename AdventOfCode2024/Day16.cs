namespace AdventOfCode2024;

internal static class Day16
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/16
        var map = File.ReadAllLines("Data/Day16.txt");

        var minScore = int.MaxValue;
        Dictionary<(Point, char), int> visitedLocations = [];
        Dictionary<Point, (Point Point, char Direction)> cameFrom = [];
        Dictionary<Point, List<Point>> possibleRoutes = [];
        PriorityQueue<Location, int> toExplore = new();
        Point start = null!;
        Point finish = null!;

        List<Point> bestPathNodes = [];

        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'S')
                    start = new Point(x, y);   

                if (map[y][x] == 'E')
                    finish = new Point(x, y);

                if (start is not null && finish is not null)
                    break;
            }
        }

        if (start is null || finish is null)
            throw new InvalidOperationException("Start or finish point not found in the map.");

        toExplore.Enqueue(new(start, '>', 0), 0);
        visitedLocations[(start, '>')] = 0;

        while (toExplore.Count > 0)
        {
            var currentLocation = toExplore.Dequeue();
            Console.WriteLine($"Exploring {currentLocation.Point.X}, {currentLocation.Point.Y} with direction {currentLocation.Direction} and score {currentLocation.Score}");
            if (currentLocation.Point == finish)
            {
                var pathScore = RebuildPath(cameFrom, start, currentLocation.Point, currentLocation.Direction);
                
                if (pathScore <= minScore)
                {
                    if (pathScore != minScore)
                        bestPathNodes = [start, finish];

                    bestPathNodes = GetBestPathNodes(bestPathNodes, possibleRoutes, start, currentLocation.Point);
                    minScore = pathScore;
                    Draw(map, start, finish, bestPathNodes);
                }

                continue;
            }
            
            foreach (var neighbour in GetNeighbours(map, currentLocation.Point, currentLocation.Direction))
            {
                var tentativeScore = currentLocation.Score + 1 + (currentLocation.Direction == neighbour.Direction ? 0 : 1000);

                if (possibleRoutes.TryGetValue(neighbour.Point, out var currentRoutes))
                    currentRoutes.Add(currentLocation.Point);
                else
                    possibleRoutes.Add(neighbour.Point, [currentLocation.Point]);

                if (visitedLocations.TryGetValue((neighbour.Point, neighbour.Direction), out var previousScore))
                {
                    if (tentativeScore <= previousScore)
                    {
                        cameFrom[neighbour.Point] = (currentLocation.Point, currentLocation.Direction);
                        visitedLocations[(neighbour.Point, neighbour.Direction)] = tentativeScore;
                        toExplore.Enqueue(new(neighbour.Point, neighbour.Direction, tentativeScore), tentativeScore);   
                    }
                }
                else
                {
                    visitedLocations.Add((neighbour.Point, neighbour.Direction), tentativeScore);

                    if (cameFrom.TryGetValue(neighbour.Point, out var currentPrev))
                    {
                        var existingScore = RebuildPath(cameFrom, start, neighbour.Point, GetDirection(currentPrev.Point, neighbour.Point));
                        //var existingScore = RebuildPath(cameFrom, start, currentPrev.Point, currentPrev.Direction);

                        if (tentativeScore <= existingScore)
                        {
                            cameFrom[neighbour.Point] = (currentLocation.Point, currentLocation.Direction);
                            toExplore.Enqueue(new(neighbour.Point, neighbour.Direction, tentativeScore), tentativeScore);
                        }
                    }
                    else
                    {
                        cameFrom.Add(neighbour.Point, (currentLocation.Point, currentLocation.Direction));
                        toExplore.Enqueue(new(neighbour.Point, neighbour.Direction, tentativeScore), tentativeScore);
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {minScore}");
        Console.WriteLine($"Part 2: {bestPathNodes.Count}"); // 573 too low
    }

    private static char GetDirection(Point current, Point next)
    {
        if (current.X == next.X)
            return current.Y < next.Y ? '^' : 'v';
        
        return current.X < next.X ? '>' : '<';
    }

    private static void Draw(string[] map, Point start, Point finish, List<Point> bestPathNodes)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (start.X == x && start.Y == y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write('S');
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (finish.X == x && finish.Y == y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write('E');
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (bestPathNodes.Any(p => p.X == x && p.Y == y))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write('O');
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                    Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }
    }

    private static List<(Point Point, char Direction)> GetNeighbours(string[] map, Point current, char direction)
    {
        List<(Point point, char direction)> neighbours = [];

        if (map[current.Y - 1][current.X] != '#' && direction != 'v')
            neighbours.Add((new Point(current.X, current.Y - 1), '^'));
        
        if (map[current.Y + 1][current.X] != '#' && direction != '^')
            neighbours.Add((new Point(current.X, current.Y + 1), 'v'));
        
        if (map[current.Y][current.X - 1] != '#' && direction != '>')
            neighbours.Add((new Point(current.X - 1, current.Y), '<'));
        
        if (map[current.Y][current.X + 1] != '#' && direction != '<')
            neighbours.Add((new Point(current.X + 1, current.Y), '>'));

        return neighbours;
    }

    private static int RebuildPath(
        Dictionary<Point, (Point Point, char Direction)> cameFrom,
        Point startPoint,
        Point current,
        char direction)
    {
        var score = 0;

        while (current != startPoint)
        {
            var previous = cameFrom[current];

            if (previous.Point.X == current.X)
            {
                if (previous.Point.Y < current.Y)
                {
                    if (direction != 'v')
                        score += 1000;

                    direction = 'v';
                }
                else
                {
                    if (direction != '^')
                        score += 1000;
                    
                    direction = '^';
                }
            }
            else
            {
                if (previous.Point.X < current.X)
                {
                    if (direction != '>')
                        score += 1000;

                    direction = '>';
                }
                else
                {
                    if (direction != '<')
                        score += 1000;

                    direction = '<';
                }
            }

            current = previous.Point;
            score++;
        }

        if (direction != '>')
            score += 1000;

        return score;
    }

    private static List<Point> GetBestPathNodes(
        List<Point> bestPathNodes,
        Dictionary<Point, List<Point>> cameFrom,
        Point startPoint,
        Point current)
    {
        foreach (var node in cameFrom[current])
        {
            if (bestPathNodes.Contains(node) || node == startPoint)
                continue;

            bestPathNodes.Add(node);
            bestPathNodes.AddRange(GetBestPathNodes(bestPathNodes, cameFrom, startPoint, node));
        }

        return bestPathNodes.Distinct().ToList();
    }

    private record Point(int X, int Y);
    private record Location(Point Point, char Direction, int Score);
}
