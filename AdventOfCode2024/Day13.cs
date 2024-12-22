namespace AdventOfCode2024;
internal static class Day13
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/13
        var machines = File.ReadAllLines("Data/Day13.txt")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        //Part1(machines);
        Part2(machines);
    }

    private static void Part1(List<string> machines)
    {
        const int A_COST = 3;
        const int B_COST = 1;

        var tokenCount = 0;

        for (int i = 0; i < machines.Count; i += 3)
        {
            var xStart = machines[i].IndexOf("X+") + 2;
            var xLength = machines[i].IndexOf(',') - xStart;
            var x = machines[i].Substring(xStart, xLength);

            var yStart = machines[i].IndexOf("Y+") + 2;
            var y = machines[i][yStart..];

            (int x, int y) aButton = (int.Parse(x), int.Parse(y));

            xStart = machines[i + 1].IndexOf("X+") + 2;
            xLength = machines[i + 1].IndexOf(',') - xStart;
            x = machines[i + 1].Substring(xStart, xLength);

            yStart = machines[i + 1].IndexOf("Y+") + 2;
            y = machines[i + 1][yStart..];

            (int x, int y) bButton = (int.Parse(x), int.Parse(y));

            xStart = machines[i + 2].IndexOf("X=") + 2;
            xLength = machines[i + 2].IndexOf(',') - xStart;
            x = machines[i + 2].Substring(xStart, xLength);

            yStart = machines[i + 2].IndexOf("Y=") + 2;
            y = machines[i + 2][yStart..];

            (int x, int y) prize = (int.Parse(x), int.Parse(y));

            Queue<(int aPress, int bPress, int xTotal, int yTotal)> moveQueue = [];
            moveQueue.Enqueue((0, 0, 0, 0));

            Dictionary<(int, int), bool> visitedStates = [];
            visitedStates.Add((0, 0), true);

            var minTokens = int.MaxValue;

            while (moveQueue.Count > 0)
            {
                var (aPress, bPress, xTotal, yTotal) = moveQueue.Dequeue();

                // a button
                if (aPress + 1 > 100)
                    continue;

                if (!visitedStates.ContainsKey((aPress + 1, bPress)))
                {

                    visitedStates.Add((aPress + 1, bPress), true);

                    if (xTotal + aButton.x == prize.x && yTotal + aButton.y == prize.y)
                    {
                        var thisMin = ((aPress + 1) * A_COST) + (bPress * B_COST);
                        minTokens = thisMin < minTokens ? thisMin : minTokens;
                    }

                    if (xTotal + aButton.x < prize.x && yTotal + aButton.y < prize.y)
                        moveQueue.Enqueue((aPress + 1, bPress, xTotal + aButton.x, yTotal + aButton.y));
                }

                // b button
                if (bPress + 1 > 100)
                    continue;

                if (!visitedStates.ContainsKey((aPress, bPress + 1)))
                {
                    visitedStates.Add((aPress, bPress + 1), true);

                    if (xTotal + bButton.x == prize.x && yTotal + bButton.y == prize.y)
                    {
                        var thisMin = (aPress * A_COST) + ((bPress + 1) * B_COST);
                        minTokens = thisMin < minTokens ? thisMin : minTokens;
                    }

                    if (xTotal + bButton.x < prize.x && yTotal + bButton.y < prize.y)
                        moveQueue.Enqueue((aPress, bPress + 1, xTotal + bButton.x, yTotal + bButton.y));
                }
            }

            if (minTokens != int.MaxValue)
                tokenCount += minTokens;
        }

        Console.WriteLine($"Part 1: {tokenCount}");
    }

    private static void Part2(List<string> machines)
    {
        const int A_COST = 3;
        const int B_COST = 1;
        const long MISCALC = 0;

        var tokenCount = 0;

        for (int i = 0; i < machines.Count; i += 3)
        {
            var xStart = machines[i].IndexOf("X+") + 2;
            var xLength = machines[i].IndexOf(',') - xStart;
            var x = machines[i].Substring(xStart, xLength);

            var yStart = machines[i].IndexOf("Y+") + 2;
            var y = machines[i][yStart..];

            (int x, int y) aButton = (int.Parse(x), int.Parse(y));

            xStart = machines[i + 1].IndexOf("X+") + 2;
            xLength = machines[i + 1].IndexOf(',') - xStart;
            x = machines[i + 1].Substring(xStart, xLength);

            yStart = machines[i + 1].IndexOf("Y+") + 2;
            y = machines[i + 1][yStart..];

            (int x, int y) bButton = (int.Parse(x), int.Parse(y));

            xStart = machines[i + 2].IndexOf("X=") + 2;
            xLength = machines[i + 2].IndexOf(',') - xStart;
            x = machines[i + 2].Substring(xStart, xLength);

            yStart = machines[i + 2].IndexOf("Y=") + 2;
            y = machines[i + 2][yStart..];

            (long x, long y) prize = (long.Parse(x) + MISCALC, long.Parse(y) + MISCALC);

            long xa = aButton.x * bButton.y;
            long xb = bButton.x * aButton.y;
            long xdiff = xa - xb;
            //https://www.reddit.com/r/adventofcode/comments/1hd5b6o/comment/m1ub740/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button
            System.Console.WriteLine("" + xdiff);

            long pa = prize.x * bButton.y;
            long pb = prize.x * aButton.y;
            long pdiff = pa - pb;

            long o = pdiff / xdiff;

            System.Console.WriteLine(o);
        }

        Console.WriteLine($"Part 2: {tokenCount}");
    }
}