using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day06
{
    class Day06
    {
        static List<string> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
                list.Add(line);
            return list;
        }

        static void PartA()
        {
            List<string> input = ReadInput();
            Dictionary<char, int>[] stats = new[] {
                new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(),
                new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(),
            };
            foreach (string s in input)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!stats[i].ContainsKey(s[i]))
                        stats[i][s[i]] = 0;
                    stats[i][s[i]]++;
                }
            }
            string message = "";
            for (int i = 0; i < 8; i++)
            {
                message += stats[i].OrderByDescending(x => x.Value).First().Key;
            }
            Console.WriteLine("Part A: Result is {0}.", message);
        }

        static void PartB()
        {
            List<string> input = ReadInput();
            Dictionary<char, int>[] stats = new[] {
                new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(),
                new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(), new Dictionary<char, int>(),
            };
            foreach (string s in input)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!stats[i].ContainsKey(s[i]))
                        stats[i][s[i]] = 0;
                    stats[i][s[i]]++;
                }
            }
            string message = "";
            for (int i = 0; i < 8; i++)
            {
                message += stats[i].OrderBy(x => x.Value).First().Key;
            }
            Console.WriteLine("Part B: Result is {0}.", message);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day06).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
