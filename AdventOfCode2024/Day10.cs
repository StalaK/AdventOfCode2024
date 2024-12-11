namespace AdventOfCode2024;
internal static class Day10
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/10
        var mapInput = File.ReadAllLines("Data/Day10.txt");

        var map = mapInput
            .Select(x =>
                x.Select(y => y == '.' ? int.MaxValue : int.Parse(y.ToString()))
                .ToList())
            .ToList();

        List<(int x, int y, (int x, int y) trailHead)> trailHeadPeaks = [];
        Stack<(int x, int y, (int x, int y) trailHead)> routes = [];
        Dictionary<(int x, int y), int> trailHeadRoutes = [];

        for (int y = 0; y < map.Count; y++)
        {
            for (int x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == 0)
                    routes.Push((x, y, (x, y)));
            }
        }

        while (routes.Count > 0)
        {
            var pos = routes.Pop();
            var currentHeight = map[pos.y][pos.x];

            if (currentHeight == 9)
            {
                if (trailHeadRoutes.ContainsKey((pos.x, pos.y)))
                    trailHeadRoutes[(pos.x, pos.y)]++;
                else
                    trailHeadRoutes.Add((pos.x, pos.y), 1);

                if (!trailHeadPeaks.Contains(pos))
                    trailHeadPeaks.Add(pos);

                continue;
            }

            if (pos.y > 0 && map[pos.y - 1][pos.x] == currentHeight + 1)
                routes.Push((pos.x, pos.y - 1, pos.trailHead));

            if (pos.x > 0 && map[pos.y][pos.x - 1] == currentHeight + 1)
                routes.Push((pos.x - 1, pos.y, pos.trailHead));

            if (pos.y < map.Count - 1 && map[pos.y + 1][pos.x] == currentHeight + 1)
                routes.Push((pos.x, pos.y + 1, pos.trailHead));

            if (pos.x < map[pos.y].Count - 1 && map[pos.y][pos.x + 1] == currentHeight + 1)
                routes.Push((pos.x + 1, pos.y, pos.trailHead));
        }

        Console.WriteLine($"Part 1: {trailHeadPeaks.Count}");
        Console.WriteLine($"Part 2: {trailHeadRoutes.Sum(r => r.Value)}");
    }
}
