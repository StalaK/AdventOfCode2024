namespace AdventOfCode2024;

internal static class Day1
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/1
        var locationData = File.ReadAllLines("Data/Day1.txt");

        List<int> list1 = [];
        List<int> list2 = [];

        foreach (var line in locationData)
        {
            var locations = line
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            list1.Add(int.Parse(locations[0]));
            list2.Add(int.Parse(locations[1]));
        }

        var sortedList1 = list1.Order().ToList();
        var sortedList2 = list2.Order().ToList();

        int total = 0;

        for (var i = 0; i < sortedList1.Count; i++)
        {
            total += Math.Abs(sortedList1[i] - sortedList2[i]);
        }

        Console.WriteLine($"Part 1: {total}");

        var similarityScore = 0;

        foreach (var location in list1)
        {
            similarityScore += list2.Count(x => x == location) * location;
        }

        Console.WriteLine($"Part 2: {similarityScore}");
    }
}
