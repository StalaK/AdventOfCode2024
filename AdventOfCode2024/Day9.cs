namespace AdventOfCode2024;
internal static class Day9
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/9
        var diskMap = File.ReadAllLines("Data/Day9.txt")[0];

        var id = 0;
        List<string> format = [];

        for (var i = 0; i < diskMap.Length; i++)
        {
            var block = i % 2 == 0;

            if (block)
            {
                format.AddRange(Enumerable.Repeat(id.ToString(), int.Parse(diskMap[i].ToString())));
                id++;
            }
            else
            {
                format.AddRange(Enumerable.Repeat(".", int.Parse(diskMap[i].ToString())));
            }
        }

        List<string> formatPart2 = [.. format];

        //for (var i = 0; i < format.Count; i++)
        //{
        //    if (format[i] != ".")
        //        continue;

        //    for (var j = format.Count - 1; j > i; j--)
        //    {
        //        if (format[j] == ".")
        //            continue;

        //        format[i] = format[j];
        //        format[j] = ".";
        //        break;
        //    }
        //}

        ulong checksum = 0;
        //var numbers = format
        //    .Where(x => x != ".")
        //    .Select(x => int.Parse(x))
        //    .ToList();

        //for (var i = 0; i < numbers.Count; i++)
        //{
        //    checksum += (ulong)(numbers[i] * i);
        //}

        //Console.WriteLine($"Part 1: {checksum}");

        for (var i = formatPart2.Count - 1; i > 0; i--)
        {
            if (formatPart2[i] == ".")
                continue;

            var startIndex = i;
            var nextNum = formatPart2[i - 1];

            while (nextNum == formatPart2[i] && startIndex > 0)
            {
                startIndex--;

                if (startIndex > 0)
                    nextNum = formatPart2[startIndex - 1];
            }

            var length = i - startIndex + 1;

            for (var j = 0; j < startIndex; j++)
            {
                if (formatPart2[j] != ".")
                    continue;

                var space = 1;
                var nextSpace = formatPart2[j + space];
                var spaceFound = space == length;

                while (nextSpace == formatPart2[j] && !spaceFound)
                {
                    space++;
                    nextSpace = formatPart2[j + space];

                    //  if (nextSpace == ".")
                    spaceFound = space == length;
                }

                if (spaceFound)
                {
                    for (var k = 0; k < length; k++)
                    {
                        formatPart2[j + k] = formatPart2[i - k];
                        formatPart2[i - k] = ".";
                    }

                    break;
                }
            }

            i = startIndex;
        }

        checksum = 0;
        var numbers = formatPart2.ConvertAll(x => x == "." ? 0 : int.Parse(x));

        for (var i = 0; i < numbers.Count; i++)
        {
            checksum += (ulong)(numbers[i] * i);
        }

        Console.WriteLine($"Part 2: {checksum}");
    }
}
