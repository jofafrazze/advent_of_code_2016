using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day01
{
    public struct Position : IComparable<Position>
    {
        public int x;
        public int y;

        public Position(Position p)
        {
            x = p.x;
            y = p.y;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(Position p)    // Reading order
        {
            if (x == p.x && y == p.y)
                return 0;
            else if (y == p.y)
                return (x < p.x) ? -1 : 1;
            else
                return (y < p.y) ? -1 : 1;
        }
        public override bool Equals(Object obj)
        {
            return obj is Position && Equals((Position)obj);
        }
        public bool Equals(Position p)
        {
            return (x == p.x) && (y == p.y);
        }
        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }
        public static Position operator +(Position p1, int k)
        {
            return p1 + new Position(k, k);
        }
        public static Position operator +(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x += p2.x;
            p.y += p2.y;
            return p;
        }
        public static Position operator -(Position p1, int k)
        {
            return p1 + (-k);
        }
        public static Position operator -(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x -= p2.x;
            p.y -= p2.y;
            return p;
        }
        public static Position operator *(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x *= k;
            p.y *= k;
            return p;
        }
        public static Position operator /(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x /= k;
            p.y /= k;
            return p;
        }
    }

    class Day01
    {
        static string[] ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string line = reader.ReadLine();
            return line.Split(", ").ToArray();
        }

        static void PartA()
        {
            string[] data = ReadInput();
            int x = 0;
            int y = 0;
            int direction = 0; // 0 = N, 1 = W, 2 = S, 3 = E
            foreach(string s in data)
            {
                bool left = (s[0] == 'L');
                direction += (left) ? 1 : -1;
                direction = (direction + 4) % 4;
                int steps = int.Parse(s.Substring(1));
                if (direction == 0)
                    y += steps;
                else if (direction == 1)
                    x -= steps;
                else if (direction == 2)
                    y -= steps;
                else
                    x += steps;
            }
            int sum = Math.Abs(x) + Math.Abs(y);
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            string[] data = ReadInput();
            int direction = 0; // 0 = N, 1 = W, 2 = S, 3 = E
            HashSet<Position> visited = new HashSet<Position>();
            Position pos = new Position(0, 0);
            Position result = new Position(0, 0);
            bool done = false;
            visited.Add(pos);
            foreach (string s in data)
            {
                bool left = (s[0] == 'L');
                direction += (left) ? 1 : -1;
                direction = (direction + 4) % 4;
                int steps = int.Parse(s.Substring(1));
                Position dir = new Position();
                if (direction == 0)
                    dir.y = 1;
                else if (direction == 1)
                    dir.x = -1;
                else if (direction == 2)
                    dir.y = -1;
                else
                    dir.x = 1;
                for (int i = 0; i < steps; i++)
                {
                    pos += dir;
                    if (!done && visited.Contains(pos))
                    {
                        result = pos;
                        done = true;
                    }
                    visited.Add(pos);
                }
            }
            int sum = Math.Abs(result.x) + Math.Abs(result.y);
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day01).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
