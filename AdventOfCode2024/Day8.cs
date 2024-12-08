namespace AdventOfCode2024;
internal static class Day8
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/8
        var map = File.ReadAllLines("Data/Day8.txt");

        List<(int x, int y, char freq)> pylons = [];
        List<(int x, int y)> antiNodes = [];

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] != '.')
                    pylons.Add((x, y, map[y][x]));
            }
        }

        foreach (var pylon in pylons)
        {
            var sameFreqPylons = pylons.Where(p =>
                p.freq == pylon.freq
                && p.x != pylon.x
                && p.y != pylon.y);

            foreach (var sameFreqPylon in sameFreqPylons)
            {
                var antiNodex = pylon.x - ((pylon.x - sameFreqPylon.x) * 2);
                var antiNodey = pylon.y - ((pylon.y - sameFreqPylon.y) * 2);

                if (antiNodex < 0
                    || antiNodey < 0
                    || antiNodex >= map[0].Length
                    || antiNodey >= map.Length
                    || antiNodes.Contains((antiNodex, antiNodey)))
                    continue;

                antiNodes.Add((antiNodex, antiNodey));
            }
        }

        Console.WriteLine($"Part 1: {antiNodes.Count}");

        antiNodes = [];

        foreach (var pylon in pylons)
        {
            var sameFreqPylons = pylons.Where(p =>
                p.freq == pylon.freq
                && p.x != pylon.x
                && p.y != pylon.y);

            if (sameFreqPylons.Any() && !antiNodes.Contains((pylon.x, pylon.y)))
                antiNodes.Add((pylon.x, pylon.y));

            foreach (var sameFreqPylon in sameFreqPylons)
            {
                var xDiff = (pylon.x - sameFreqPylon.x);
                var yDiff = (pylon.y - sameFreqPylon.y);

                var antiNodex = pylon.x - (xDiff * 2);
                var antiNodey = pylon.y - (yDiff * 2);

                while (antiNodex >= 0
                    && antiNodey >= 0
                    && antiNodex < map[0].Length
                    && antiNodey < map.Length)
                {
                    if (!antiNodes.Contains((antiNodex, antiNodey)))
                        antiNodes.Add((antiNodex, antiNodey));

                    antiNodex -= xDiff;
                    antiNodey -= yDiff;
                }
            }
        }

        Console.WriteLine($"Part 2: {antiNodes.Count}");
    }
}
