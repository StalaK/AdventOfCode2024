namespace AdventOfCode2024;

internal static class Day5
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/5
        var data = File.ReadAllLines("Data/Day5.txt");

        var splitPoint = data
            .Index()
            .Where(x => string.IsNullOrEmpty(x.Item))
            .Select(x => x.Index)
            .First();

        var rulesData = data[..splitPoint];
        var orders = data[^(data.Length - splitPoint - 1)..];

        var rules = CreateRules(rulesData);

        var orderedTotal = 0;
        var reorderedTotal = 0;

        foreach (var order in orders)
        {
            var pages = order
                .Split(',')
                .Select(x => int.Parse(x))
                .ToList();

            List<int> printedPages = [];
            var ordered = true;

            foreach (var page in pages)
            {
                if (!rules.ContainsKey(page))
                {
                    printedPages.Add(page);
                    continue;
                }

                var dependencies = pages.Intersect(rules[page]);
                var allDependenciesPrinted = dependencies.All(x => printedPages.Contains(x));

                if (allDependenciesPrinted)
                {
                    printedPages.Add(page);
                }
                else
                {
                    ordered = false;
                    break;
                }
            }

            var midPage = (int)Math.Floor(pages.Count / 2m);

            if (ordered)
            {
                orderedTotal += pages[midPage];
            }
            else
            {
                var orderedList = Reorder(pages, rules);
                reorderedTotal += orderedList[midPage];
            }
        }

        Console.WriteLine($"Part 1: {orderedTotal}");
        Console.WriteLine($"Part 2: {reorderedTotal}");
    }

    private static Dictionary<int, List<int>> CreateRules(string[] rulesData)
    {
        Dictionary<int, List<int>> rules = [];

        foreach (var rule in rulesData)
        {
            var pageDependency = rule.Split("|");

            var page = int.Parse(pageDependency[1]);
            var dependency = int.Parse(pageDependency[0]);

            if (rules.TryGetValue(page, out var value))
                value.Add(dependency);
            else
                rules.Add(page, [dependency]);
        }

        return rules;
    }

    private static List<int> Reorder(IEnumerable<int> pages, Dictionary<int, List<int>> rules)
    {
        List<int> printedPages = [];
        Queue<int> unprintedPages = new(pages);

        while (unprintedPages.Count > 0)
        {
            var page = unprintedPages.Dequeue();

            if (!rules.ContainsKey(page))
            {
                printedPages.Add(page);
                continue;
            }

            var dependencies = pages.Intersect(rules[page]);

            var allDependenciesPrinted = dependencies.All(x => printedPages.Contains(x));

            if (allDependenciesPrinted)
                printedPages.Add(page);
            else
                unprintedPages.Enqueue(page);
        }

        return printedPages;
    }
}
