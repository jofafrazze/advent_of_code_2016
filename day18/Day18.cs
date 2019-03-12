using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day18
{
    class Day18
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        static string GetNextRow(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                bool l = (i > 0) ? input[i - 1] == '^' : false;
                bool c = input[i] == '^';
                bool r = (i < input.Length - 1) ? input[i + 1] == '^' : false;
                bool trap = (l && c && !r) || (!l && c && r) || (l && !c && !r) || (!l && !c && r);
                sb.Append(trap ? '^' : '.');
            }
            return sb.ToString();
        }

        static int GetSafeTiles(string firstRow, int rows)
        {
            List<string> list = new List<string>() { firstRow };
            for (int i = 0; i < rows - 1; i++)
                list.Add(GetNextRow(list[i]));
            int sum = 0;
            foreach (string s in list)
                sum += s.Where(x => x == '.').Count();
            return sum;
        }

        static void PartA()
        {
            string input = ReadInput();
            int sum = GetSafeTiles(input, 40);
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            string input = ReadInput();
            int sum = GetSafeTiles(input, 400000);
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day18).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
