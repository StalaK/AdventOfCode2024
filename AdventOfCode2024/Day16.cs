namespace AdventOfCode2024;

internal static class Day16
{
    private record Point(int X, int Y);
    private record Location(Point Point, char Direction, int Score);
    private static readonly char[] Directions = ['^', 'v', '<', '>'];

    internal static void Run()
    {
        // https://adventofcode.com/2024/day/16
        var map = File.ReadAllLines("Data/Day16.txt");

        var minScore = int.MaxValue;
        Dictionary<(Point, char), int> visitedLocations = [];
        Dictionary<(Point Point, char Direction), (Point Point, char Direction)> cameFrom = [];
        PriorityQueue<Location, int> toExplore = new();
        Point start = null!;
        Point finish = null!;

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'S')
                    start = new Point(x, y);

                if (map[y][x] == 'E')
                    finish = new Point(x, y);
            }
        }

        if (start is null || finish is null)
            throw new InvalidOperationException("Start or finish point not found in the map.");

        toExplore.Enqueue(new Location(start, '>', 0), 0);
        visitedLocations[(start, '>')] = 0;

        while (toExplore.Count > 0)
        {
            var currentLocation = toExplore.Dequeue();

            // skip stale queue entries whose score no longer matches the best known
            if (visitedLocations.TryGetValue((currentLocation.Point, currentLocation.Direction), out var knownScore) && knownScore != currentLocation.Score)
                continue;

            foreach (var neighbour in GetNeighbours(map, currentLocation.Point, currentLocation.Direction))
            {
                var tentativeScore = currentLocation.Score + 1 + (currentLocation.Direction == neighbour.Direction ? 0 : 1000);
                var neighbourKey = (neighbour.Point, neighbour.Direction);

                if (!visitedLocations.TryGetValue(neighbourKey, out var previousScore) || tentativeScore < previousScore)
                {
                    visitedLocations[neighbourKey] = tentativeScore;
                    cameFrom[neighbourKey] = (currentLocation.Point, currentLocation.Direction);
                    toExplore.Enqueue(new Location(neighbour.Point, neighbour.Direction, tentativeScore), tentativeScore);
                }
            }
        }

        // Part 1: find minimal score to reach finish
        foreach (var direction in Directions)
        {
            if (visitedLocations.TryGetValue((finish, direction), out var score))
                minScore = Math.Min(minScore, score);
        }

        if (minScore == int.MaxValue)
        {
            Console.WriteLine("No path found to finish.");
            return;
        }

        var distanceToFinish = new Dictionary<(Point, char), int>();
        var backtrackSteps = new PriorityQueue<(Point Point, char Direction), int>();

        foreach (var visitedLocation in visitedLocations)
            distanceToFinish[visitedLocation.Key] = int.MaxValue;

        // Set finish state to 0
        foreach (var direction in Directions)
        {
            var key = (finish, direction);
            if (distanceToFinish.ContainsKey(key))
            {
                distanceToFinish[key] = 0;
                backtrackSteps.Enqueue(key, 0);
            }
        }

        while (backtrackSteps.Count > 0)
        {
            var currentLocation = backtrackSteps.Dequeue();
            var curDist = distanceToFinish[currentLocation];

            foreach (var pred in GetReversePredecessors(map, currentLocation.Point, currentLocation.Direction))
            {
                var tentativeScore = curDist + 1 + (pred.Direction == currentLocation.Direction ? 0 : 1000);
                var predecessorKey = (pred.Point, pred.Direction);

                if (!distanceToFinish.TryGetValue(predecessorKey, out var existing) || tentativeScore < existing)
                {
                    distanceToFinish[predecessorKey] = tentativeScore;
                    backtrackSteps.Enqueue(predecessorKey, tentativeScore);
                }
            }
        }

        var tilesOnBest = new HashSet<Point>();
        foreach (var location in visitedLocations)
        {
            if (!distanceToFinish.TryGetValue(location.Key, out var distanceToEnd) || distanceToEnd == int.MaxValue)
                continue;

            if (location.Value + distanceToEnd == minScore)
                tilesOnBest.Add(location.Key.Item1);
        }

        //Draw(map, start, finish, visitedLocations, cameFrom, minScore);
        Console.WriteLine($"Part 1: {minScore}");
        Console.WriteLine($"Part 2: {tilesOnBest.Count}");
    }
    
    private static void Draw(
        string[] map,
        Point start, Point finish, Dictionary<(Point, char), int> visitedLocations,
        Dictionary<(Point Point, char Direction), (Point Point, char Direction)> cameFrom,
        int minScore)
    {
        List<Point> bestPathNodes = [];
        foreach (var direction in Directions)
        {
            if (visitedLocations.TryGetValue((finish, direction), out var score) && score == minScore)
            {
                bestPathNodes = ReconstructPathPoints(cameFrom, start, new Location(finish, direction, score));
                break;
            }
        }

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
        var neighbours = new List<(Point Point, char Direction)>();

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

    private static List<Point> ReconstructPathPoints(
        Dictionary<(Point Point, char Direction),
        (Point Point, char Direction)> cameFrom,
        Point startPoint,
        Location currentLocation)
    {
        var path = new List<Point>();
        var key = (currentLocation.Point, currentLocation.Direction);

        // include finish
        path.Add(currentLocation.Point);

        while (!key.Equals((startPoint, '>')))
        {
            if (!cameFrom.TryGetValue(key, out var prev))
                throw new InvalidOperationException($"No predecessor for state {key}");

            key = prev;
            path.Add(key.Point);
        }

        path.Add(startPoint);
        path.Reverse();
        
        return path;
    }

    private static List<(Point Point, char Direction)> GetReversePredecessors(string[] map, Point point, char currentDirection)
    {
        var backstepNeighbours = new List<(Point, char)>();

        List<Point> nextSteps = [
            new Point(point.X, point.Y - 1),
            new Point(point.X, point.Y + 1),
            new Point(point.X - 1, point.Y),
            new Point(point.X + 1, point.Y)];

        foreach (var step in nextSteps)
        {
            if (!CanMove(map, step))
                continue;

            foreach (var possibleDirection in Directions)
            {
                var neighbours = GetNeighbours(map, step, possibleDirection);
                foreach (var neighbour in neighbours)
                {
                    if (neighbour.Point == point && neighbour.Direction == currentDirection)
                        backstepNeighbours.Add((step, possibleDirection));
                }
            }
        }

        return backstepNeighbours;
    }

    private static bool CanMove(string[] map, Point location)
    {
        if (location.Y < 0 || location.Y >= map.Length)
            return false;
            
        if (location.X < 0 || location.X >= map[location.Y].Length)
            return false;
            
        return map[location.Y][location.X] != '#';
    }
}
