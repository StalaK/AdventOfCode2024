using System.Text.RegularExpressions;

namespace AdventOfCode2024;
internal static partial class Day3
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/3
        var memoryLines = File.ReadAllLines("Data/Day3.txt");
        var memory = string.Concat(memoryLines);

        var total = MulFunctions()
            .Matches(memory)
            .Select(x => x.Value
                .Replace("mul(", "")
                .Replace(")", "")
                .Split(","))
            .Sum(x => int.Parse(x[0]) * int.Parse(x[1]));

        Console.WriteLine($"Part One: {total}");

        total = 0;

        var matches = MulFunctions().Matches(memory);
        var i = 0;

        while (i != -1)
        {
            var offPosition = memory.IndexOf("don't()", i);

            if (offPosition < 0)
                offPosition = memory.Length;

            total += matches
                .Where(x => x.Index >= i && x.Index < offPosition)
                .Select(x => x.Value
                    .Replace("mul(", "")
                    .Replace(")", "")
                    .Split(","))
                .Sum(x => int.Parse(x[0]) * int.Parse(x[1]));

            i = memory.IndexOf("do()", offPosition);
        }

        Console.WriteLine($"Part Two: {total}");
    }

    [GeneratedRegex(@"mul[(]\d{1,3},\d{1,3}[)]")]
    private static partial Regex MulFunctions();
}