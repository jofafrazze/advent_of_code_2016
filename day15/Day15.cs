using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day15
{
    struct Disc
    {
        public int size;
        public int startPos;
        public int posWhenFirstReached;
        public int dropTimeForPassing;
    }

    class Day15
    {
        static List<Disc> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Disc> list = new List<Disc>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(' ').ToArray();
                Disc d = new Disc()
                {
                    size = int.Parse(s[3]),
                    startPos = int.Parse(s[11].TrimEnd('.')),
                    posWhenFirstReached = 0,
                    dropTimeForPassing = 0
                };
                list.Add(d);
            }
            return list;
        }

        static int FindFirstPassingTime(List<Disc> discs)
        {
            for (int a = 0; a < discs.Count; a++)
            {
                Disc d = discs[a];
                d.posWhenFirstReached = (d.startPos + (a + 1)) % d.size;
                d.dropTimeForPassing = d.size - d.posWhenFirstReached;
                discs[a] = d;
            }
            Disc first = discs[0];
            List<Disc> rest = discs.Skip(1).ToList();
            bool done = false;
            int i = 0;
            for (i = first.dropTimeForPassing; !done; i += first.size)
            {
                done = true;
                for (int n = 1; (n < discs.Count) && done; n++)
                    if ((i - discs[n].dropTimeForPassing) % discs[n].size != 0)
                        done = false;
            }
            return i - first.size;
        }

        static void PartA()
        {
            List<Disc> input = ReadInput();
            int time = FindFirstPassingTime(input);
            Console.WriteLine("Part A: Result is {0}.", time);
        }

        static void PartB()
        {
            List<Disc> input = ReadInput();
            Disc last = new Disc()
            {
                size = 11,
                startPos = 0,
                posWhenFirstReached = 0,
                dropTimeForPassing = 0
            };
            input.Add(last);
            int time = FindFirstPassingTime(input);
            Console.WriteLine("Part B: Result is {0}.", time);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day15).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
