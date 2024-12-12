using System.Diagnostics;

namespace AdventOfCode2024;
internal static class Day11
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/11
        var stones = File.ReadAllLines("Data/Day11.txt")
            .First()
            .Split(' ')
            .Select(x => long.Parse(x))
            .ToList();

        const int BLINK_COUNT = 75;
        Dictionary<long, long> stoneCounts = [];

        foreach (var stone in stones)
        {
            if (stoneCounts.TryGetValue(stone, out var value))
                stoneCounts[stone] = ++value;
            else
                stoneCounts.Add(stone, 1);
        }

        var time = Stopwatch.StartNew();

        for (var i = 0; i < BLINK_COUNT; i++)
        {
            var currentStones = stoneCounts.Where(x => x.Value > 0).ToList();

            if (i == 25)
                Console.WriteLine($"Part 1: {stoneCounts.Sum(x => x.Value)}");

            foreach (var stone in currentStones)
            {
                if (stone.Key == 0)
                {
                    if (stoneCounts.ContainsKey(1))
                        stoneCounts[1] += stone.Value;
                    else
                        stoneCounts.Add(1, stone.Value);

                    stoneCounts[stone.Key] -= stone.Value;
                    continue;
                }

                var stoneString = stone.Key.ToString();

                if (stoneString.Length % 2 == 0)
                {
                    var halfLength = stoneString.Length / 2;
                    var left = long.Parse(stoneString[..halfLength]);
                    var right = long.Parse(stoneString[halfLength..]);

                    if (stoneCounts.ContainsKey(left))
                        stoneCounts[left] += stone.Value;
                    else
                        stoneCounts.Add(left, stone.Value);

                    if (stoneCounts.ContainsKey(right))
                        stoneCounts[right] += stone.Value;
                    else
                        stoneCounts.Add(right, stone.Value);

                    stoneCounts[stone.Key] -= stone.Value;
                }
                else
                {
                    var newVal = stone.Key * 2024;

                    if (stoneCounts.ContainsKey(newVal))
                        stoneCounts[newVal] += stone.Value;
                    else
                        stoneCounts.Add(newVal, stone.Value);

                    stoneCounts[stone.Key] -= stone.Value;
                }
            }
        }

        Console.WriteLine($"Part 2: {stoneCounts.Sum(x => x.Value)}");
    }
}
