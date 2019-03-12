using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;
using Position = AdventOfCode.GenericPosition2D<int>;

namespace day13
{
    class Day13
    {
        static int PositionNumber(int x, int y, int puzzle)
        {
            return (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + puzzle;
        }
        static int CountBits(int a)
        {
            int bits = 0;
            for (int n = 0; n < 31; n++)
                if ((a & (1 << n)) > 0)
                    bits++;
            return bits;
        }
        static void FillMap(ref Map map, int puzzle)
        {
            for (int y = 0; y < map.height; y++)
                for (int x = 0; x < map.width; x++)
                    map.data[x, y] = CountBits(PositionNumber(x, y, puzzle)) % 2 == 0 ? '.' : '#';
        }

        static readonly Position goUp = new Position(0, -1);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly List<Position> directions = new List<Position>()
        {
            goUp, goRight, goDown, goLeft,
        };

        static int ReachMapPosition(Map map, Position targetPos, out int nPosIn50Steps)
        {
            Dictionary<Position, int> visited = new Dictionary<Position, int>();
            visited[map.pos] = 0;
            List<Position> toCheck = new List<Position>() { map.pos };
            while ((toCheck.Count > 0) && !visited.ContainsKey(targetPos))
            {
                List<Position> nextToCheck = new List<Position>();
                foreach (Position p in toCheck)
                {
                    foreach (Position d in directions)
                    {
                        Position neighbor = p + d;
                        if (map.HasPosition(neighbor) && (map[neighbor] != '#'))
                        {
                            // Position is valid and reachable, check if we've been here already
                            int steps = visited[p] + 1;
                            if (!visited.ContainsKey(neighbor) || (visited[neighbor] > steps))
                            {
                                visited[neighbor] = steps;
                                nextToCheck.Add(neighbor);
                            }
                        }
                    }
                }
                toCheck = nextToCheck;
            }
            nPosIn50Steps = visited.Where(a => a.Value <= 50).Count();
            return visited.ContainsKey(targetPos) ? visited[targetPos] : -1;
        }

        static void PartAB()
        {
            const int input = 1352;
            Position targetPos = new Position(31, 39);
            Map map = new Map(100, 100, new Position(1, 1));
            FillMap(ref map, input);
            //map.Print();
            int n = ReachMapPosition(map, targetPos, out int nPosIn50Steps);
            Console.WriteLine("Part A: Result is {0}.", n);
            Console.WriteLine("Part B: Result is {0}.", nPosIn50Steps);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day13).Namespace + ":");
            PartAB();
        }
    }
}
