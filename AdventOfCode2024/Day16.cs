namespace AdventOfCode2024;

internal static class Day16
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/16
        var map = File.ReadAllLines("Data/Day16.txt");

        var minScore = int.MaxValue;
        Dictionary<(Point, char), int> visitedLocations = [];
        Stack<Location> toExplore = [];
        
        for(int y = 0; y < map.Length; y++)
        {
            for(int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'S')
                    toExplore.Push(new(new Point(x, y), '>', 0));
                
                if (toExplore.Count > 0)
                    break;
            }

            if (toExplore.Count > 0)
                break;
        }

        while(toExplore.Count > 0)
        {
            var currentLocation = toExplore.Pop();
            //System.Console.WriteLine($"Exploring: ({currentLocation.Point.X}, {currentLocation.Point.Y}). Queued: {toExplore.Count()}");

            if (map[currentLocation.Point.Y - 1][currentLocation.Point.X] == 'E')
            {
                var sameDirection = currentLocation.Direction == '^';
                var totalScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);
                minScore = Math.Min(minScore, totalScore);
                continue;
            }

            if (map[currentLocation.Point.Y + 1][currentLocation.Point.X] == 'E')
            {
                var sameDirection = currentLocation.Direction == 'v';
                var totalScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);
                minScore = Math.Min(minScore, totalScore);
                continue;
            }

            if (map[currentLocation.Point.Y][currentLocation.Point.X - 1] == 'E')
            {
                var sameDirection = currentLocation.Direction == '<';
                var totalScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);
                minScore = Math.Min(minScore, totalScore);
                continue;
            }

            if (map[currentLocation.Point.Y][currentLocation.Point.X + 1] == 'E')
            {
                var sameDirection = currentLocation.Direction == '>';
                var totalScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);
                minScore = Math.Min(minScore, totalScore);
                continue;
            }

            if (map[currentLocation.Point.Y - 1][currentLocation.Point.X] != '#' && currentLocation.Direction != 'v')
            {   
                var sameDirection = currentLocation.Direction == '^';
                var currentScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);

                if (visitedLocations.TryGetValue((new Point(currentLocation.Point.X, currentLocation.Point.Y - 1), '^'), out var previousVisitScore))
                {
                    if (previousVisitScore <= currentScore)
                        continue;
                    
                    visitedLocations[(new Point(currentLocation.Point.X, currentLocation.Point.Y - 1), '^')] = currentScore;
                }

                toExplore.Push(new(new Point(currentLocation.Point.X, currentLocation.Point.Y - 1), '^', currentScore));
            }

            if (map[currentLocation.Point.Y + 1][currentLocation.Point.X] != '#' && currentLocation.Direction != '^')
            {   
                var sameDirection = currentLocation.Direction == 'v';
                var currentScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);

                if (visitedLocations.TryGetValue((new Point(currentLocation.Point.X, currentLocation.Point.Y + 1), 'v'), out var previousVisitScore))
                {
                    if (previousVisitScore <= currentScore)
                        continue;
                    
                    visitedLocations[(new Point(currentLocation.Point.X, currentLocation.Point.Y + 1), 'v')] = currentScore;
                }

                toExplore.Push(new(new Point(currentLocation.Point.X, currentLocation.Point.Y + 1), 'v', currentScore));
            }

            if (map[currentLocation.Point.Y][currentLocation.Point.X - 1] != '#' && currentLocation.Direction != '>')
            {   
                var sameDirection = currentLocation.Direction == '<';
                var currentScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);

                if (visitedLocations.TryGetValue((new Point(currentLocation.Point.X - 1, currentLocation.Point.Y), '<'), out var previousVisitScore))
                {
                    if (previousVisitScore <= currentScore)
                        continue;

                    visitedLocations[(new Point(currentLocation.Point.X - 1, currentLocation.Point.Y), '<')] = currentScore;
                }

                toExplore.Push(new(new Point(currentLocation.Point.X - 1, currentLocation.Point.Y), '<', currentScore));
            }

            if (map[currentLocation.Point.Y][currentLocation.Point.X + 1] != '#' && currentLocation.Direction != '<')
            {   
                var sameDirection = currentLocation.Direction == '>';
                var currentScore = currentLocation.Score + 1 + (sameDirection ? 0 : 1000);

                if (visitedLocations.TryGetValue((new Point(currentLocation.Point.X + 1, currentLocation.Point.Y), '>'), out var previousVisitScore))
                {
                    if (previousVisitScore <= currentScore)
                        continue;

                    visitedLocations[(new Point(currentLocation.Point.X + 1, currentLocation.Point.Y), '>')] = currentScore;
                }

                toExplore.Push(new(new Point(currentLocation.Point.X + 1, currentLocation.Point.Y), '>', currentScore));
            }

            visitedLocations.TryAdd((currentLocation.Point, currentLocation.Direction), currentLocation.Score);
        }

        // 100520 too high
        Console.WriteLine($"Part 1: {minScore}");
    }

    private record Point(int X, int Y);
    private record Location(Point Point, char Direction, int Score);
}
