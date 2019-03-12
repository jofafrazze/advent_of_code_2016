using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day02
{
    class Day02
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

        static readonly List<List<int>> numpad = new List<List<int>>()
        {
            new List<int> () { 1, 2, 3 },
            new List<int> () { 4, 5, 6 },
            new List<int> () { 7, 8, 9 },
        };

        static void PartA()
        {
            List<string> input = ReadInput();
            int row = 1;
            int col = 1;
            string code = "";
            foreach (string line in input)
            {
                foreach (char c in line)
                {
                    if (c == 'D' && row < 2) row += 1;
                    else if (c == 'U' && row > 0) row -= 1;
                    else if (c == 'R' && col < 2) col += 1;
                    else if (c == 'L' && col > 0) col -= 1;
                }
                int key = numpad[row][col];
                code += key.ToString();
            }
            Console.WriteLine("Part A: Result is {0}.", code);
        }

        static readonly List<string> fancyNumpad = new List<string>()
        {
            "  1  ",
            " 234 ",
            "56789",
            " ABC ",
            "  D  ",
        };

        static void PartB()
        {
            List<string> input = ReadInput();
            int row = 2;
            int col = 0;
            string code = "";
            foreach (string line in input)
            {
                foreach (char c in line)
                {
                    int previousRow = row;
                    int previousCol = col;
                    if (c == 'D' && row < 4) row += 1;
                    else if (c == 'U' && row > 0) row -= 1;
                    else if (c == 'R' && col < 4) col += 1;
                    else if (c == 'L' && col > 0) col -= 1;
                    if (fancyNumpad[row][col] == ' ')
                    {
                        row = previousRow;
                        col = previousCol;
                    }
                }
                char key = fancyNumpad[row][col];
                code += key;
            }
            Console.WriteLine("Part B: Result is {0}.", code);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day02).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
