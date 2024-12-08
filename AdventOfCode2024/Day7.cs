namespace AdventOfCode2024;
internal static class Day7
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/7
        var data = File.ReadAllLines("Data/Day7.txt");

        var total = 0L;
        Dictionary<int, List<string>> combinationLookup = [];
        const bool part1 = true;

        foreach (var line in data)
        {
            var parts = line.Split(':');
            var answer = long.Parse(parts[0]);
            var numbers = parts[1]
                .Trim()
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToList();

            var operandCount = (numbers.Count - 1);

            if (!combinationLookup.TryGetValue(operandCount, out var combinations))
            {
                combinations = [];
                if (part1)
                    GenerateCombinationsPart1(combinations, "", operandCount);
                else
                    GenerateCombinationsPart2(combinations, "", operandCount);

                combinationLookup.Add(operandCount, combinations);
            }

            for (var i = 0; i < combinations.Count; i++)
            {
                var combination = combinations[i];
                var rollingTotal = (long)numbers[0];

                for (var k = 1; k < numbers.Count; k++)
                {
                    if (combination[k - 1] == '*')
                        rollingTotal *= numbers[k];
                    else if (combination[k - 1] == '+')
                        rollingTotal += numbers[k];
                    else
                        rollingTotal = long.Parse($"{rollingTotal}{numbers[k]}");

                    if (rollingTotal > answer)
                        break;
                }

                if (rollingTotal == answer)
                {
                    total += answer;
                    break;
                }
            }
        }

        Console.WriteLine($"Part {(part1 ? "1" : "2")}: {total}");
    }

    static void GenerateCombinationsPart1(List<string> combinations, string current, int maxLength)
    {
        if (current.Length == maxLength)
        {
            combinations.Add(current);
            return;
        }

        GenerateCombinationsPart1(combinations, current + "*", maxLength);
        GenerateCombinationsPart1(combinations, current + "+", maxLength);
    }

    static void GenerateCombinationsPart2(List<string> combinations, string current, int maxLength)
    {
        if (current.Length == maxLength)
        {
            combinations.Add(current);
            return;
        }

        GenerateCombinationsPart2(combinations, current + "*", maxLength);
        GenerateCombinationsPart2(combinations, current + "+", maxLength);
        GenerateCombinationsPart2(combinations, current + "|", maxLength);
    }
}
