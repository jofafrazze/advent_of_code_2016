using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day20
{
    struct Range
    {
        public long ip1;
        public long ip2;
    }

    class Day20
    {
        static List<Range> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Range> list = new List<Range>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                long[] d = line.Split('-').Select(long.Parse).ToArray();
                Range r = new Range()
                {
                    ip1 = d[0],
                    ip2 = d[1]
                };
                list.Add(r);
            }
            return list;
        }

        static void PartAB()
        {
            List<Range> input = ReadInput();
            List<Range> merged = new List<Range>(input);
            int index = -1;
            do
            {
                merged = merged.OrderBy(x => x.ip1).ThenBy(x => x.ip2).ToList();
                index = -1;
                for (int i = 0; (i < merged.Count - 1) && (index < 0); i++)
                {
                    if (merged[i].ip2 >= merged[i + 1].ip1 - 1)
                        index = i;
                }
                if (index >= 0)
                {
                    Range r1 = merged[index];
                    Range r2 = merged[index + 1];
                    Range r = new Range() { ip1 = r1.ip1, ip2 = Math.Max(r1.ip2, r2.ip2) };
                    List<Range> next = merged.Take(index).ToList();
                    next.Add(r);
                    merged = next.Concat(merged.Skip(index + 2)).ToList();
                }
            }
            while (index >= 0);
            Console.WriteLine("Part A: Result is {0}.", merged.First().ip2 + 1);
            long sum = merged.First().ip1;
            sum += UInt32.MaxValue - merged.Last().ip2;
            for (int i = 0; (i < merged.Count - 1) && (index < 0); i++)
                sum += merged[i + 1].ip1 - merged[i].ip2 - 1;
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day20).Namespace + ":");
            PartAB();
        }
    }
}
