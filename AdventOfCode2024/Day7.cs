﻿namespace AdventOfCode2024;
internal static class Day7
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/7
        var data = File.ReadAllLines("Data/Day7.txt");

        var total = 0L;

        foreach (var line in data)
        {
            var parts = line.Split(':');
            var answer = long.Parse(parts[0]);
            var numbers = parts[1]
                .Trim()
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToList();

            var numberCount = numbers.Count();
            var operandCount = (numberCount - 1);

            List<string> combinations = [new string('*', operandCount)];

            for (int len = 1; len <= operandCount; len++)
            {
                for (int pos = 0; pos < operandCount; pos++)
                {
                    if (pos + len > operandCount)
                        break;

                    var combination = "";
                    combination += new string('*', pos);
                    combination += new string('+', len);
                    combination += new string('*', Math.Max(operandCount - len - pos, 0));
                    combinations.Add(combination);
                }
            }

            for (var i = 0; i < combinations.Count(); i++)
            {
               var combination = combinations[i];
               var rollingTotal = (long)numbers[0];

               for (var k = 1; k < numbers.Count; k++)
               {
                   rollingTotal = combination[k - 1] == '*'
                       ? rollingTotal * numbers[k]
                       : rollingTotal + numbers[k];

                   if (rollingTotal > answer)
                       break;
               }

               if (rollingTotal == answer)
               {
                   Console.WriteLine(line);
                   total += answer;
                   break;
               }
            }
        }

        Console.WriteLine($"Part 1: {total}");
    }
}
