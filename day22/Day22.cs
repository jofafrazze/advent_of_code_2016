using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Position = AdventOfCode.GenericPosition2D<int>;

namespace day22
{
    class Node : ICloneable
    {
        public Position pos;
        public int used;
        public int available;
        public Node(Position p, int u, int a) { pos = p; used = u; available = a; }
        public Node(Node n) : this(n.pos, n.used, n.available) {}
        public object Clone() { return new Node(this); }
    }

    class Day22
    {
        static List<Node> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            reader.ReadLine();
            reader.ReadLine();
            List<Node> list = new List<Node>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
                string[] p = s[0].Split('-').ToArray();
                Node n = new Node(
                    new Position(int.Parse(p[1].Substring(1)), int.Parse(p[2].Substring(1))),
                    int.Parse(s[2].TrimEnd('T')),
                    int.Parse(s[3].TrimEnd('T'))
                );
                list.Add(n);
            }
            return list;
        }

        static Node[,] ConvertToNodeArray(List<Node> list)
        {
            int w = list.Select(a => a.pos.x).Max() + 1;
            int h = list.Select(a => a.pos.y).Max() + 1;
            Node[,] array = new Node[w, h];
            foreach (Node n in list)
                array[n.pos.x, n.pos.y] = n;
            return array;
        }

        static void PartA()
        {
            List<Node> nodeList = ReadInput();
            int viablePairs = 0;
            foreach (Node n1 in nodeList)
            {
                foreach (Node n2 in nodeList)
                {
                    if ((n1 != n2) && (n1.used > 0) && (n1.used <= n2.available))
                        viablePairs++;
                }
            }
            Console.WriteLine("Part A: Result is {0}.", viablePairs);
        }

        public static T[,] DeepClone<T>(T[,] items) where T : ICloneable
        {
            T[,] copy = new T[items.GetLength(0), items.GetLength(1)];
            for (int y = 0; y < items.GetLength(1); y++)
                for (int x = 0; x < items.GetLength(0); x++)
                    copy[x, y] = (T)items[x, y].Clone();
            return copy;
        }

        static void PartB()
        {
            List<Node> nodeList = ReadInput();
            Node[,] array = ConvertToNodeArray(nodeList);
            for (int e = 1; e >= 0; e--)
            {
                StringBuilder sb = new StringBuilder("   ");
                int div = (int)Math.Pow(10, e);
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    int digit = (x / div) % 10;
                    string s = (e > 0 && digit == 0) ? " " : digit.ToString();
                    sb.Append(s);
                }
                Console.WriteLine(sb.ToString());
            }
            for (int y = 0; y < array.GetLength(1); y++)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    char c = '.';
                    if (array[x, y].used > 100) c = '#';
                    if (array[x, y].used == 0) c = '_';
                    if (x == 0 && y == 0) c = 'E';
                    if ((x == array.GetLength(0) - 1) && y == 0) c = 'S';
                    sb.Append(c);
                }
                Console.WriteLine(y.ToString("00") + " " + sb.ToString());
            }
            Console.WriteLine("S should end up at E position.");
            Console.WriteLine("Map width: {0}", array.GetLength(0));
            Console.WriteLine("Map height: {0}", array.GetLength(1));
            Console.WriteLine("Part B: Result is {0}.", "[Do it by hand...]");
            Console.WriteLine(@"
...

With my input: 

A) It takes 12 + 14 + 31 = 57 moves to get the empty node to the left of the S node.
B) Each cycle moving S one step left and cycling the empty node back to the left of S takes 5 moves, do 31 cycles.
C) In the end a single move is needed to take the S to the E position.

Thus: Moves needed = 57 + (5 * 31) + 1 = 213 moves.
");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day22).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
