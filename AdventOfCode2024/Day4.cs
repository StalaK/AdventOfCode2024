namespace AdventOfCode2024;

internal static class Day4
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/4
        var data = File.ReadAllLines("Data/Day4.txt");

        var count = 0;

        for (var i = 0; i < data.Length; i++)
        {
            for (var j = 0; j < data[i].Length; j++)
            {
                if (data[i][j] != 'X')
                    continue;

                var canForwardCheck = j < data[i].Length - 3;
                var canBackCheck = j > 2;
                var canUpCheck = i > 2;
                var canDownCheck = i < data.Length - 3;

                if (canForwardCheck)
                {
                    var forwardCheck = data[i].Substring(j, 4);

                    if (forwardCheck == "XMAS")
                        count++;
                }

                if (canBackCheck)
                {
                    var backwardCheck = data[i].Substring(j - 3, 4);
                    if (backwardCheck == "SAMX")
                        count++;
                }

                if (canUpCheck)
                {
                    var upWord = string.Concat(data[i][j], data[i - 1][j], data[i - 2][j], data[i - 3][j]);
                    if (upWord == "XMAS" || upWord.Reverse().ToString() == "XMAS")
                        count++;

                    if (canForwardCheck)
                    {
                        var udForward = string.Concat(data[i][j], data[i - 1][j + 1], data[i - 2][j + 2], data[i - 3][j + 3]);
                        if (udForward == "XMAS" || udForward.Reverse().ToString() == "XMAS")
                            count++;
                    }

                    if (canBackCheck)
                    {
                        var udBackward = string.Concat(data[i][j], data[i - 1][j - 1], data[i - 2][j - 2], data[i - 3][j - 3]);
                        if (udBackward == "XMAS" || udBackward.Reverse().ToString() == "XMAS")
                            count++;
                    }
                }

                if (canDownCheck)
                {
                    var downWord = string.Concat(data[i][j], data[i + 1][j], data[i + 2][j], data[i + 3][j]);
                    if (downWord == "XMAS" || downWord.Reverse().ToString() == "XMAS")
                        count++;

                    if (canForwardCheck)
                    {
                        var ddForward = string.Concat(data[i][j], data[i + 1][j + 1], data[i + 2][j + 2], data[i + 3][j + 3]);
                        if (ddForward == "XMAS" || ddForward.Reverse().ToString() == "XMAS")
                            count++;
                    }

                    if (canBackCheck)
                    {
                        var ddBackward = string.Concat(data[i][j], data[i + 1][j - 1], data[i + 2][j - 2], data[i + 3][j - 3]);
                        if (ddBackward == "XMAS" || ddBackward.Reverse().ToString() == "XMAS")
                            count++;
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {count}");

        count = 0;

        for (var i = 1; i < data.Length - 1; i++)
        {
            for (var j = 1; j < data[i].Length - 1; j++)
            {
                if (data[i][j] != 'A')
                    continue;

                var tb = string.Concat(data[i - 1][j - 1], data[i][j], data[i + 1][j + 1]);
                var bt = string.Concat(data[i + 1][j - 1], data[i][j], data[i - 1][j + 1]);

                var tbMas = tb == "MAS" || tb == "SAM";
                var btMas = bt == "MAS" || bt == "SAM";

                if (tbMas && btMas)
                    count++;
            }
        }

        Console.WriteLine($"Part 2: {count}");
    }
}