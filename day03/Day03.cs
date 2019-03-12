using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day03
{
    class Day03
    {
        static List<List<int>> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<List<int>> listList = new List<List<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List<int> row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                listList.Add(row);
            }
            return listList;
        }

        static int PossibleTriangles(List<List<int>> input)
        {
            int possibleTriangle = 0;
            foreach (List<int> t in input)
            {
                t.Sort();
                if (t[0] + t[1] > t[2])
                    possibleTriangle++;
            }
            return possibleTriangle;
        }

        static void PartA()
        {
            List<List<int>> input = ReadInput();
            int n = PossibleTriangles(input);
            Console.WriteLine("Part A: Result is {0}.", n);
        }

        static void PartB()
        {
            List<List<int>> input = ReadInput();
            List<List<int>> input2 = new List<List<int>>();
            for (int a = 0; a < input.Count; a += 3)
            {
                input2.Add(new List<int>() { input[a][0], input[a + 1][0], input[a + 2][0] });
                input2.Add(new List<int>() { input[a][1], input[a + 1][1], input[a + 2][1] });
                input2.Add(new List<int>() { input[a][2], input[a + 1][2], input[a + 2][2] });
            }
            int n = PossibleTriangles(input2);
            Console.WriteLine("Part B: Result is {0}.", n);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day03).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
