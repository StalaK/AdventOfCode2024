namespace AdventOfCode2024;

internal static class Day16
{
    private record Point(int X, int Y);
    private record Location(Point Point, char Direction, int Score);

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

        // locate start and finish
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

            // skip stale queue entries whose score no longer matches the best-known
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

        // compute minimal score to reach finish across all facing directions
        foreach (var dir in new[] { '^', 'v', '<', '>' })
        {
            if (visitedLocations.TryGetValue((finish, dir), out var score))
                minScore = Math.Min(minScore, score);
        }

        if (minScore == int.MaxValue)
        {
            Console.WriteLine("No path found to finish.");
            return;
        }

        // Reverse Dijkstra: compute minimal cost from any state to the finish (distToFinish)
        var distToFinish = new Dictionary<(Point, char), int>();
        var reversePQ = new PriorityQueue<(Point Point, char Direction), int>();

        // initialize distances to infinity
        foreach (var kv in visitedLocations)
            distToFinish[kv.Key] = int.MaxValue;

        // seed with finish states (cost 0 to be at finish)
        foreach (var dir in new[] { '^', 'v', '<', '>' })
        {
            var key = (finish, dir);
            if (distToFinish.ContainsKey(key))
            {
                distToFinish[key] = 0;
                reversePQ.Enqueue(key, 0);
            }
        }

        while (reversePQ.Count > 0)
        {
            var current = reversePQ.Dequeue();
            var curDist = distToFinish[current];

            // consider predecessors that can move to `current`
            foreach (var pred in GetReversePredecessors(map, current.Point, current.Direction))
            {
                var predKey = (pred.Point, pred.Direction);
                // cost from pred -> current
                var cost = 1 + (pred.Direction == current.Direction ? 0 : 1000);
                var tentative = curDist + cost;
                if (!distToFinish.TryGetValue(predKey, out var existing) || tentative < existing)
                {
                    distToFinish[predKey] = tentative;
                    reversePQ.Enqueue(predKey, tentative);
                }
            }
        }

        // collect all points that lie on any optimal path: distStart[state] + distToFinish[state] == minScore
        var tilesOnBest = new HashSet<Point>();
        foreach (var kv in visitedLocations)
        {
            var state = kv.Key;
            var dStart = kv.Value;
            if (!distToFinish.TryGetValue(state, out var dEnd) || dEnd == int.MaxValue)
                continue;

            if (dStart + dEnd == minScore)
                tilesOnBest.Add(state.Item1);
        }

        // For visualization, reconstruct a single best path (pick any facing at finish with minScore)
        List<Point> bestPathNodes = [];
        foreach (var dir in new[] { '^', 'v', '<', '>' })
        {
            if (visitedLocations.TryGetValue((finish, dir), out var score) && score == minScore)
            {
                bestPathNodes = ReconstructPathPoints(cameFrom, start, new Location(finish, dir, score));
                break;
            }
        }

        Draw(map, start, finish, bestPathNodes);
        Console.WriteLine($"Part 1: {minScore}");
        Console.WriteLine($"Part 2: {tilesOnBest.Count}");
    }

    private static char GetDirection(Point current, Point next)
    {
        if (current.X == next.X)
            return current.Y < next.Y ? 'v' : '^';
        
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
        Dictionary<(Point Point, char Direction), (Point Point, char Direction)> cameFrom,
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

    // For reverse Dijkstra: find all predecessor states (point + direction) that can move to (point,direction)
    private static List<(Point Point, char Direction)> GetReversePredecessors(string[] map, Point point, char direction)
    {
        // A predecessor is a state (p, dir) such that one of its neighbours equals `point` with direction `direction`.
        // So we check the four adjacent tiles and, for each, determine if moving from that tile into `point` would produce the desired `direction`.
        var preds = new List<(Point, char)>();

        // up -> predecessor is (point.X, point.Y-1) moving 'v' into point
        var up = new Point(point.X, point.Y - 1);
        if (IsWalkable(map, up) && direction == '^' /* arriving facing up means previous moved down? */)
        {
            // If the predecessor was at up and moved down into point, then its neighbour direction is 'v'
            preds.Add((up, 'v'));
        }

        // down -> predecessor is (point.X, point.Y+1)
        var down = new Point(point.X, point.Y + 1);
        if (IsWalkable(map, down) && direction == 'v')
        {
            preds.Add((down, '^'));
        }

        // left -> predecessor is (point.X-1, point.Y)
        var left = new Point(point.X - 1, point.Y);
        if (IsWalkable(map, left) && direction == '<')
        {
            preds.Add((left, '>'));
        }

        // right -> predecessor is (point.X+1, point.Y)
        var right = new Point(point.X + 1, point.Y);
        if (IsWalkable(map, right) && direction == '>')
        {
            preds.Add((right, '<'));
        }

        // The above logic attempted to infer predecessor facing, but a safer approach is to enumerate all four adjacent positions
        // and all possible facing states, then filter by whether moving from that state to `point` yields (point,direction).
        // For simplicity and correctness, regenerate by checking all four adjacent tiles and all four facings.
        preds.Clear();
        var adj = new[] { up, down, left, right };
        var facings = new[] { '^', 'v', '<', '>' };

        foreach (var p in adj)
        {
            if (!IsWalkable(map, p)) continue;
            foreach (var f in facings)
            {
                // if from state (p,f) one of its neighbours is (point,direction)
                var neighbours = GetNeighbours(map, p, f);
                foreach (var n in neighbours)
                {
                    if (n.Point == point && n.Direction == direction)
                        preds.Add((p, f));
                }
            }
        }

        return preds;
    }

    private static bool IsWalkable(string[] map, Point p)
    {
        if (p.Y < 0 || p.Y >= map.Length)
            return false;
        if (p.X < 0 || p.X >= map[p.Y].Length)
            return false;
            
        return map[p.Y][p.X] != '#';
    }

    private static int ComputeScoreFromPath(List<Point> pathPoints, char initialFacing)
    {
        if (pathPoints == null || pathPoints.Count == 0) return int.MaxValue;

        var score = 0;
        var facing = initialFacing;

        for (int i = 1; i < pathPoints.Count; i++)
        {
            var prev = pathPoints[i - 1];
            var cur = pathPoints[i];
            var moveDir = GetDirection(prev, cur);
            if (moveDir != facing)
                score += 1000;

            facing = moveDir;
            score += 1;
        }

        if (facing != '>') score += 1000;
        return score;
    }

    private static int RebuildPath(
        Dictionary<(Point Point, char Direction), (Point Point, char Direction)> cameFrom,
        Point startPoint,
        Point current,
        char direction)
    {
        var score = 0;

        while (current != startPoint)
        {
            // Try to find the previous entry using the exact (current, direction) key first.
            var key = (current, direction);

            if (!cameFrom.TryGetValue(key, out (Point Point, char Direction) previousEntry))
            {
                // Fallback: find any entry for this point (may occur if directions differ)
                bool found = false;
                foreach (var k in cameFrom.Keys)
                {
                    if (k.Point == current)
                    {
                        previousEntry = cameFrom[k];
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new InvalidOperationException($"No predecessor found for point {current}");
            }

            var previousPoint = previousEntry.Point;
            // movement direction from previous -> current
            var movementDir = GetDirection(previousPoint, current);

            if (direction != movementDir)
                score += 1000;

            direction = movementDir;
            current = previousPoint;
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
}
