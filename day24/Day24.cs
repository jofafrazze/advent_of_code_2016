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

namespace day24
{
    class Day24
    {
        static Map ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
                list.Add(line);
            Map map = new Map(list[0].Length, list.Count, new Position());
            for (int y = 0; y < list.Count; y++)
            {
                for (int x = 0; x < list[0].Length; x++)
                {
                    char c = list[y][x];
                    map.data[x, y] = c;
                    if (c == '0')
                        map.pos = new Position(x, y);
                }
            }
            return map;
        }

        static Dictionary<int, Position> FindPositions(Map map)
        {
            Dictionary<int, Position> locations = new Dictionary<int, Position>();
            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    char c = map.data[x, y];
                    if (char.IsDigit(c))
                        locations[int.Parse(c.ToString())] = new Position(x, y);
                }
            }
            return locations;
        }

        static readonly Position goUp = new Position(0, -1);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly List<Position> directions = new List<Position>()
        {
            goUp, goRight, goDown, goLeft,
        };

        static Dictionary<int, int> FindDistances(Map map, Position startPos)
        {
            Dictionary<int, int> endingDistances = new Dictionary<int, int>();
            Dictionary<Position, int> posDistances = new Dictionary<Position, int>() { [startPos] = 0 };
            List<Position> toCheck = new List<Position>() { startPos };
            while (toCheck.Count > 0)
            {
                List<Position> nextToCheck = new List<Position>();
                foreach (Position p in toCheck)
                {
                    foreach (Position d in directions)
                    {
                        Position n = p + d;
                        char c = map[n];
                        if (map.HasPosition(n) && (c != '#'))
                        {
                            int steps = posDistances[p] + 1;
                            if (!posDistances.ContainsKey(n) || (posDistances[n] > steps))
                            {
                                posDistances[n] = steps;
                                nextToCheck.Add(n);
                                if (char.IsDigit(c))
                                    endingDistances[int.Parse(c.ToString())] = steps;
                            }
                        }
                    }
                }
                toCheck = nextToCheck;
            }
            return endingDistances;
        }

        static List<Dictionary<int, int>> FindAllDistances(Map map, Dictionary<int, Position> locations)
        {
            List<Dictionary<int, int>> distances = new List<Dictionary<int, int>>();
            for (int i = 0; locations.ContainsKey(i); i++)
            {
                distances.Add(FindDistances(map, locations[i]));
            }
            return distances;
        }

        static int GetMinSteps(bool returnToZero)
        {
            Map map = ReadInput();
            Dictionary<int, Position> locations = FindPositions(map);
            List<Dictionary<int, int>> distances = FindAllDistances(map, locations);
            List<int> locationsToGoto = locations.Keys.ToList();
            locationsToGoto.Remove(0);
            List<List<int>> combos = Algorithms.HeapPermutation(locationsToGoto);
            int minSteps = int.MaxValue;
            foreach (List<int> listOthers in combos)
            {
                List<int> list = new List<int>() { 0 };
                list.AddRange(listOthers);
                if (returnToZero)
                    list.Add(0);
                int steps = 0;
                for (int i = 0; i < list.Count - 1; i++)
                    steps += distances[list[i]][list[i + 1]];
                if (steps < minSteps)
                    minSteps = steps;
            }
            return minSteps;
        }

        static void PartA()
        {
            int steps = GetMinSteps(false);
            Console.WriteLine("Part A: Result is {0}.", steps);
        }

        static void PartB()
        {
            int steps = GetMinSteps(true);
            Console.WriteLine("Part B: Result is {0}.", steps);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day24).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
