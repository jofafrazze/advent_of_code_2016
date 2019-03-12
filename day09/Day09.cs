using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day09
{
    class Day09
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        static void PartA1()
        {
            string input = ReadInput();
            Regex regex = new Regex(@"\((\d+)x(\d+)\)");
            string data = input;
            StringBuilder expanded = new StringBuilder();
            while (data.Length > 0)
            {
                Match match = regex.Match(data);
                if (match.Success)
                {
                    expanded.Append(data.Substring(0, match.Index));
                    GroupCollection groups = match.Groups;
                    int chars = int.Parse(groups[1].Value);
                    int count = int.Parse(groups[2].Value);
                    for (int i = 0; i < count; i++)
                        expanded.Append(data.Substring(match.Index + match.Length, chars));
                    data = data.Substring(match.Index + match.Length + chars);
                }
                else
                {
                    expanded.Append(data);
                    data = "";
                }
            }
            Console.WriteLine("Part A1: Result is {0}.", expanded.Length);
        }

        static long GetExpandedLength(string data, bool recurse)
        {
            long prefix = 0;
            long decoded = 0;
            long postfix = 0;
            Regex regex = new Regex(@"\((\d+)x(\d+)\)");
            Match match = regex.Match(data);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                int chars = int.Parse(groups[1].Value);
                long count = long.Parse(groups[2].Value);
                prefix = match.Index;
                if (recurse)
                    decoded = GetExpandedLength(data.Substring(match.Index + match.Length, chars), recurse) * count;
                else
                    decoded = chars * count;
                postfix = GetExpandedLength(data.Substring(match.Index + match.Length + chars), recurse);
            }
            else
            {
                prefix = data.Length;
            }
            return prefix + decoded + postfix;
        }

        static void PartA2()
        {
            string input = ReadInput();
            long len = GetExpandedLength(input, false);
            Console.WriteLine("Part A2: Result is {0}.", len);
        }

        static void PartB()
        {
            string input = ReadInput();
            long len = GetExpandedLength(input, true);
            Console.WriteLine("Part B: Result is {0}.", len);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day09).Namespace + ":");
            PartA1();
            PartA2();
            PartB();
        }
    }
}
