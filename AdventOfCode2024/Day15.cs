namespace AdventOfCode2024;

internal static class Day15
{
    internal static void Run()
    {
        // https://adventofcode.com/2024/day/15
        var data = File.ReadAllLines("Data/Day15.txt");

        //Part1(data);
        Part2(data);
    }
    
    private static void Part1(string[] data)
    {
        var splitPoint = data
            .Index()
            .Where(x => string.IsNullOrEmpty(x.Item))
            .Select(x => x.Index)
            .First();

        var map = data[..splitPoint];
        var moves = string.Concat(data[^(data.Length - splitPoint - 1)..]);
        
        Point robot = new(-1,-1);
        List<Point> boxes =[];

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'O')
                    boxes.Add(new Point(x, y));

                if (map[y][x] == '@')
                    robot = new Point(x, y);
            }
        }

        map = map
            .Select(x =>
                x.Replace('O', '.')
                .Replace('@', '.'))
            .ToArray();

        foreach (var move in moves)
        {
            if (CanMove(map, robot, boxes, move))
                (robot, boxes) = Move(map, robot, robot, boxes, move, true);           
        }

        var result = boxes.Sum(x => (100 * x.Y) + x.X);
        Console.WriteLine($"Part 1: {result}");
    }

    private static void Part2(string[] data)
    {
        var splitPoint = data
            .Index()
            .Where(x => string.IsNullOrEmpty(x.Item))
            .Select(x => x.Index)
            .First();

        var inputMap = data[..splitPoint];
        var moves = string.Concat(data[^(data.Length - splitPoint - 1)..]);

        var map = new string[inputMap.Length];

        for (var i = 0; i < inputMap.Length; i++)
        {
            map[i] = inputMap[i]
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.");
        }

        Point robot = new(-1,-1);
        List<Point> boxes =[];

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '[')
                    boxes.Add(new Point(x, y));

                if (map[y][x] == '@')
                    robot = new Point(x, y);
            }
        }

        map = map
            .Select(x =>
                x.Replace("[]", "..")
                .Replace('@', '.'))
            .ToArray();

        //Draw(map, robot, boxes);

        foreach (var (i, move) in moves.Index())
        {
            if (CanMovePart2(map, robot, boxes, move, true))
                (robot, boxes) = MovePart2(map, robot, robot, boxes, move, true);

            // System.Console.WriteLine();
            // Draw(map, robot, boxes);
        }

        var result = boxes.Sum(x => (100 * x.Y) + x.X);
        Console.WriteLine($"Part 2: {result}");
    }


    private static bool CanMove(string[] map, Point movingObject, List<Point> boxes, char direction)
    {
        var newPosition = direction switch
        {
            '^' => new Point(movingObject.X, movingObject.Y - 1),
            'v' => new Point(movingObject.X, movingObject.Y + 1),
            '<' => new Point(movingObject.X - 1, movingObject.Y),
            '>' => new Point(movingObject.X + 1, movingObject.Y),
            _ => throw new NotImplementedException(),
        };

        if (map[newPosition.Y][newPosition.X] == '#')
            return false;

        var connectedObject = boxes.FirstOrDefault(b => b.X == newPosition.X && b.Y == newPosition.Y);

        if (connectedObject is not null)
            return CanMove(map, connectedObject, boxes, direction);

        return true;
    }

    private static bool CanMovePart2(string[] map, Point movingObject, List<Point> boxes, char direction, bool isRobot)
    {
        var newPosition = direction switch
        {
            '^' => new Point(movingObject.X, movingObject.Y - 1),
            'v' => new Point(movingObject.X, movingObject.Y + 1),
            '<' => new Point(movingObject.X - 1, movingObject.Y),
            '>' => new Point(movingObject.X + 1, movingObject.Y),
            _ => throw new NotImplementedException(),
        };

        if (map[newPosition.Y][newPosition.X] == '#' || (!isRobot && map[newPosition.Y][newPosition.X + 1] == '#'))
            return false;

        var connectedObjects = boxes.Where(b => 
            ((b.X == newPosition.X && b.Y == newPosition.Y)
            || (b.X + 1 == newPosition.X && b.Y == newPosition.Y)
            || (!isRobot && b.X == newPosition.X + 1 && b.Y == newPosition.Y)
            || (!isRobot && b.X + 1 == newPosition.X + 1 && b.Y == newPosition.Y))
            && (b.X != movingObject.X || b.Y != movingObject.Y));


        var result = true;

        foreach (var connectedObject in connectedObjects)
        {
            if (connectedObject is not null)
                result &= CanMovePart2(map, connectedObject, boxes, direction, false);
        }

        return result;
    }

    private static (Point robot, List<Point> boxes) Move(
        string[] map,
        Point robot,
        Point movingObject,
        List<Point> boxes,
        char direction,
        bool isRobot)
    {
        var oldBox = new Point(movingObject.X, movingObject.Y);

        movingObject = direction switch
        {
            '^' => new(movingObject.X, movingObject.Y - 1),
            'v' => new Point(movingObject.X, movingObject.Y + 1),
            '<' => new Point(movingObject.X - 1, movingObject.Y),
            '>' => new Point(movingObject.X + 1, movingObject.Y),
            _ => throw new NotImplementedException(),
        };

        var connectedObject = boxes.FirstOrDefault(b => b.X == movingObject.X && b.Y == movingObject.Y);

        if (isRobot)
        {
            robot = movingObject;
        }
        else
        {
            var boxMoved = false;
            boxes = boxes
                .Select(b => 
                {
                    if (boxMoved)
                    {
                        return b;
                    }
                    else if (b.X == oldBox.X && b.Y == oldBox.Y)
                    {
                        boxMoved = true;
                        return new Point(movingObject.X, movingObject.Y);
                    }
                    else
                    {
                        return b;
                    }
                })
                .ToList();
        }

        if (connectedObject is not null)
            return Move(map, robot, connectedObject, boxes, direction, false);

        return (robot, boxes);
    }

    private static (Point robot, List<Point> boxes) MovePart2(
        string[] map,
        Point robot,
        Point movingObject,
        List<Point> boxes,
        char direction,
        bool isRobot)
    {
        var oldPoint = new Point(movingObject.X, movingObject.Y);

        movingObject = direction switch
        {
            '^' => new(movingObject.X, movingObject.Y - 1),
            'v' => new Point(movingObject.X, movingObject.Y + 1),
            '<' => new Point(movingObject.X - 1, movingObject.Y),
            '>' => new Point(movingObject.X + 1, movingObject.Y),
            _ => throw new NotImplementedException(),
        };

        var connectedObjects = boxes.Where(b => 
            ((b.X == movingObject.X && b.Y == movingObject.Y)
            || (b.X + 1 == movingObject.X && b.Y == movingObject.Y)
            || (!isRobot && b.X == movingObject.X + 1 && b.Y == movingObject.Y)
            || (!isRobot && b.X + 1 == movingObject.X + 1 && b.Y == movingObject.Y))
             && (b.X != oldPoint.X || b.Y != oldPoint.Y));

        if (isRobot)
        {
            robot = movingObject;
        }
        else
        {
            var boxMoved = false;
            boxes = boxes
                .Select(b => 
                {
                    if (boxMoved)
                    {
                        return b;
                    }
                    else if (b.X == oldPoint.X && b.Y == oldPoint.Y)
                    {
                        boxMoved = true;
                        return new Point(movingObject.X, movingObject.Y);
                    }
                    else
                    {
                        return b;
                    }
                })
                .ToList();
        }

        foreach (var connectedObject in connectedObjects)
        {
            if (connectedObject is not null)
                (robot, boxes) = MovePart2(map, robot, connectedObject, boxes, direction, false);
        }

        return (robot, boxes);
    }

    private static void Draw(string[] map, Point robot, List<Point> boxes)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (robot.X == x && robot.Y == y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write('@');
                }
                else if (boxes.Any(b => b.X == x && b.Y == y))
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write('[');
                }
                else if (boxes.Any(b => b.X == x - 1 && b.Y == y))
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(']');
                }
                else
                {
                    if (map[y][x] == '#')
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.Write(map[y][x]);
                }
            }

            Console.WriteLine();
        }
    }

    private class Point(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }
}