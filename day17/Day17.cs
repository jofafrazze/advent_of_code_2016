using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;
using Position = AdventOfCode.GenericPosition2D<int>;

namespace day17
{
    class Day17
    {
        static readonly Position goUp = new Position(0, -1);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly List<Position> directions = new List<Position>()
        {
            goUp, goDown, goLeft, goRight,
        };
        static readonly List<char> directionChars = new List<char>() { 'U', 'D', 'L', 'R' };

        static List<int> GetAllowedDirectionIndexes(Map map, Position p, string pathHash)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(pathHash);
            byte[] hash = md5.ComputeHash(bytes);
            string hashAllow = hash[0].ToString("x2") + hash[1].ToString("x2");
            bool IsDoorOpen(int a)
            {
                char c = hashAllow[a];
                return char.IsLetter(c) && c != 'a';
            }
            List<int> allowed = new List<int>();
            for (int i = 0; i < directions.Count; i++)
            {
                Position n = p + directions[i];
                if (map.HasPosition(n) && IsDoorOpen(i))
                    allowed.Add(i);
            }
            return allowed;
        }

        static string ReachMapPosition(Map map, Position targetPos, string baseHash, bool shortestPath)
        {
            Dictionary<string, Position> visited = new Dictionary<string, Position> { [""] = map.pos };
            List<string> toCheck = new List<string>() { "" };
            bool done = false;
            while ((toCheck.Count > 0) && !done)
            {
                List<string> nextToCheck = new List<string>();
                foreach (string path in toCheck)
                {
                    Position p = visited[path];
                    if (p != targetPos)
                    {
                        List<int> allowedDirectionIndexes = GetAllowedDirectionIndexes(map, p, baseHash + path);
                        foreach (int i in allowedDirectionIndexes)
                        {
                            string nextPath = path + directionChars[i];
                            Position nextPosition = p + directions[i];
                            visited[nextPath] = nextPosition;
                            nextToCheck.Add(nextPath);
                        }
                    }
                }
                toCheck = nextToCheck;
                if (shortestPath)
                    done = visited.ContainsValue(targetPos);
            }
            if (visited.ContainsValue(targetPos))
            {
                if (shortestPath)
                    return visited.Where(x => x.Value == targetPos).OrderBy(x => x.Key.Length).First().Key;
                else
                    return visited.Where(x => x.Value == targetPos).OrderByDescending(x => x.Key.Length).First().Key;
            }
            return "";
        }

        const string input = "qtetzkpl"; // My actual input
        //const string input = "ihgpwlah"; // Should render "DDRRRD", 370
        //const string input = "kglvqrro"; // Should render "DDUDRLRRUDRD", 492
        //const string input = "ulqzkmiv"; // Should render "DRURDRUDDLLDLUURRDULRLDUUDDDRR", 830

        static void PartA()
        {
            Position targetPos = new Position(3, 3);
            Map map = new Map(4, 4, new Position(0, 0));
            string path = ReachMapPosition(map, targetPos, input, true);
            Console.WriteLine("Part A: Result is {0}.", path);
        }

        static void PartB()
        {
            Position targetPos = new Position(3, 3);
            Map map = new Map(4, 4, new Position(0, 0));
            string path = ReachMapPosition(map, targetPos, input, false);
            Console.WriteLine("Part A: Result is {0}.", path.Length);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day17).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
