namespace AdventOfCode2024;

internal static class Day2
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/2
        var reports = File.ReadAllLines("Data/Day2.txt");

        var safeReports = 0;

        const int SAFE_MIN = 1;
        const int SAFE_MAX = 3;

        foreach (var report in reports)
        {
            var levels = report
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToList();

            var safe = true;
            int? increasing = null;

            for (var i = 1; i < levels.Count; i++)
            {
                var levelDiff = Math.Abs(levels[i - 1] - levels[i]);
                increasing ??= Math.Sign(levels[i] - levels[i - 1]);

                if (levelDiff < SAFE_MIN || levelDiff > SAFE_MAX || Math.Sign(levels[i] - levels[i - 1]) != increasing)
                {
                    safe = false;
                    break;
                }
            }

            if (safe)
                safeReports++;
        }

        Console.WriteLine($"Part 1: {safeReports}");

        safeReports = 0;

        foreach (var report in reports)
        {
            var levels = report
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToList();

            var safe = true;
            int? increasing = null;
            bool damperTriggered = false;

            for (var i = 1; i < levels.Count; i++)
            {
                var levelDiff = Math.Abs(levels[i - 1] - levels[i]);
                increasing ??= Math.Sign(levels[i] - levels[i - 1]);

                if (levelDiff < SAFE_MIN || levelDiff > SAFE_MAX || Math.Sign(levels[i] - levels[i - 1]) != increasing)
                {
                    if (damperTriggered)
                    {
                        safe = false;
                        break;
                    }
                    else
                    {
                        damperTriggered = true;

                        if (i == 1)
                            increasing = null;

                        continue;
                    }
                }
            }

            if (safe)
                safeReports++;
        }

        Console.WriteLine($"Part 2: {safeReports}");
    }
}
