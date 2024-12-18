namespace AdventOfCode2024;
internal static class Day12
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/12
        var plots = File.ReadAllLines("Data/Day12.txt");

        List<(int x, int y)> plotsCalculated = [];
        Stack<(int x, int y)> searchPoints = [];

        var totalPart1 = 0;
        var totalPart2 = 0;

        for (int y = 0; y < plots.Length; y++)
        {
            for (int x = 0; x < plots[y].Length; x++)
            {
                if (plotsCalculated.Contains((x, y)))
                    continue;

                searchPoints.Push((x, y));
                // # of corners == # of sides
                var corners = 0;
                var area = 0;
                var fences = 0;

                while (searchPoints.Count > 0)
                {
                    area++;
                    var searchPoint = searchPoints.Pop();
                    plotsCalculated.Add((searchPoint.x, searchPoint.y));

                    var canSearchUp = searchPoint.y != 0 && plots[searchPoint.y - 1][searchPoint.x] == plots[y][x];
                    var shouldSearchUp = !plotsCalculated.Contains((searchPoint.x, searchPoint.y - 1)) && !searchPoints.Contains((searchPoint.x, searchPoint.y - 1));

                    var canSearchDown = searchPoint.y != plots.Length - 1 && plots[searchPoint.y + 1][searchPoint.x] == plots[y][x];
                    var shouldSearchDown = !plotsCalculated.Contains((searchPoint.x, searchPoint.y + 1)) && !searchPoints.Contains((searchPoint.x, searchPoint.y + 1));

                    var canSearchLeft = searchPoint.x != 0 && plots[searchPoint.y][searchPoint.x - 1] == plots[y][x];
                    var shouldSearchLeft = !plotsCalculated.Contains((searchPoint.x - 1, searchPoint.y)) && !searchPoints.Contains((searchPoint.x - 1, searchPoint.y));

                    var canSearchRight = searchPoint.x != plots[y].Length - 1 && plots[searchPoint.y][searchPoint.x + 1] == plots[y][x];
                    var shouldSearchRight = !plotsCalculated.Contains((searchPoint.x + 1, searchPoint.y)) && !searchPoints.Contains((searchPoint.x + 1, searchPoint.y));

                    if (canSearchUp && shouldSearchUp)
                        searchPoints.Push((searchPoint.x, searchPoint.y - 1));

                    if (canSearchDown && shouldSearchDown)
                        searchPoints.Push((searchPoint.x, searchPoint.y + 1));

                    if (canSearchLeft && shouldSearchLeft)
                        searchPoints.Push((searchPoint.x - 1, searchPoint.y));

                    if (canSearchRight && shouldSearchRight)
                        searchPoints.Push((searchPoint.x + 1, searchPoint.y));

                    fences += canSearchUp ? 0 : 1;
                    fences += canSearchDown ? 0 : 1;
                    fences += canSearchLeft ? 0 : 1;
                    fences += canSearchRight ? 0 : 1;

                    if (!canSearchUp && !canSearchLeft)
                        corners++;

                    if (!canSearchUp && !canSearchRight)
                        corners++;

                    if (!canSearchDown && !canSearchLeft)
                        corners++;

                    if (!canSearchDown && !canSearchRight)
                        corners++;

                    if (canSearchUp && canSearchLeft && plots[searchPoint.y - 1][searchPoint.x - 1] != plots[searchPoint.y][searchPoint.x])
                        corners++;

                    if (canSearchUp && canSearchRight && plots[searchPoint.y - 1][searchPoint.x + 1] != plots[searchPoint.y][searchPoint.x])
                        corners++;

                    if (canSearchDown && canSearchLeft && plots[searchPoint.y + 1][searchPoint.x - 1] != plots[searchPoint.y][searchPoint.x])
                        corners++;

                    if (canSearchDown && canSearchRight && plots[searchPoint.y + 1][searchPoint.x + 1] != plots[searchPoint.y][searchPoint.x])
                        corners++;
                }

                totalPart1 += area * fences;
                totalPart2 += area * corners;
            }
        }

        Console.WriteLine($"Part 1: {totalPart1}");
        Console.WriteLine($"Part 2: {totalPart2}");
    }
}