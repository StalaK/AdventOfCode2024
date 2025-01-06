namespace AdventOfCode2024;
internal static class Day13
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/13
        var machines = File.ReadAllLines("Data/Day13.txt")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        Part1(machines);
        Part2(machines);
    }

    private static void Part1(List<string> machines)
    {
        const int A_COST = 3;
        const int B_COST = 1;
        const long MISCALC = 0;

        var tokenCount = 0L;

        for (int i = 0; i < machines.Count; i += 3)
        {
            var aButton = GetAButton(machines, i);
            var bButton = GetBButton(machines, i);
            var prize = GetPrizeLocation(machines, i, MISCALC);

            var aButtonPresses = GetAButtonPresses(aButton, bButton, prize);

            if (aButtonPresses < 0)
                continue;

            var bButtonPresses = GetBButtonPresses(aButton, bButton, prize, aButtonPresses);

            if (bButtonPresses < 0)
                continue;

            tokenCount += (aButtonPresses * A_COST) + (bButtonPresses * B_COST);
        }

        Console.WriteLine($"Part 1: {tokenCount}");
    }

    private static void Part2(List<string> machines)
    {
        const int A_COST = 3;
        const int B_COST = 1;
        const long MISCALC = 10000000000000;

        var tokenCount = 0L;

        for (int i = 0; i < machines.Count; i += 3)
        {
            var aButton = GetAButton(machines, i);
            var bButton = GetBButton(machines, i);
            var prize = GetPrizeLocation(machines, i, MISCALC);

            var aButtonPresses = GetAButtonPresses(aButton, bButton, prize);

            if (aButtonPresses < 0)
                continue;

            var bButtonPresses = GetBButtonPresses(aButton, bButton, prize, aButtonPresses);

            if (bButtonPresses < 0)
                continue;

            tokenCount += (aButtonPresses * A_COST) + (bButtonPresses * B_COST);
        }

        Console.WriteLine($"Part 2: {tokenCount}");
    }

    private static Button GetAButton(List<string> machines, int i)
    {
        var xStart = machines[i].IndexOf("X+") + 2;
        var xLength = machines[i].IndexOf(',') - xStart;
        var x = machines[i].Substring(xStart, xLength);
        var yStart = machines[i].IndexOf("Y+") + 2;
        var y = machines[i][yStart..];

        return new Button(int.Parse(x), int.Parse(y));
    }

    private static Button GetBButton(List<string> machines, int i)
    {
        var xStart = machines[i + 1].IndexOf("X+") + 2;
        var xLength = machines[i + 1].IndexOf(',') - xStart;
        var x = machines[i + 1].Substring(xStart, xLength);
        var yStart = machines[i + 1].IndexOf("Y+") + 2;
        var y = machines[i + 1][yStart..];

        return new Button(int.Parse(x), int.Parse(y));
    }

    private static Prize GetPrizeLocation(List<string> machines, int i, long miscalc)
    {
        var xStart = machines[i + 2].IndexOf("X=") + 2;
        var xLength = machines[i + 2].IndexOf(',') - xStart;
        var x = machines[i + 2].Substring(xStart, xLength);
        var yStart = machines[i + 2].IndexOf("Y=") + 2;
        var y = machines[i + 2][yStart..];

        return new Prize(long.Parse(x) + miscalc, long.Parse(y) + miscalc);
    }

    private static long GetAButtonPresses(Button aButton, Button bButton, Prize prize)
    {
        long xa = (aButton.x * bButton.y) + (bButton.x * bButton.y);
        long xprize = prize.x * bButton.y;

        long ya = (aButton.y * bButton.x) + (bButton.y * bButton.x);
        long yprize = prize.y * bButton.x;

        long t = xa - ya;
        long p = xprize - yprize;

        var ans = p / t;
        return p % t == 0
            ? ans
            : -1;
    }

    private static long GetBButtonPresses(Button aButton, Button bButton, Prize prize, long aPresses)
    {
        var b = (prize.x - (aButton.x * aPresses));
        return b % bButton.x == 0
            ? b / bButton.x
            : -1;
    }

    private record Button(int x, int y);
    private record Prize(long x, long y);
}