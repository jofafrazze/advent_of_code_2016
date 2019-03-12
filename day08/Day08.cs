using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;
using Position = AdventOfCode.GenericPosition2D<int>;

namespace day08
{
    class Day08
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

        static Map TransformMap(Map mapIn, List<string> operations)
        {
            Map map = new Map(mapIn);
            void RotateRow(int row)
            {
                int iMax = map.width - 1;
                char c = map.data[iMax, row];
                for (int i = iMax - 1; i >= 0; i--)
                    map.data[i + 1, row] = map.data[i, row];
                map.data[0, row] = c;
            }
            void RotateCol(int col)
            {
                int iMax = map.height - 1;
                char c = map.data[col, iMax];
                for (int i = iMax - 1; i >= 0; i--)
                    map.data[col, i + 1] = map.data[col, i];
                map.data[col, 0] = c;
            }
            foreach (string op in operations)
            {
                string[] v = op.Split(" ").ToArray();
                if (v[0] == "rect")
                {
                    int[] size = v[1].Split('x').Select(int.Parse).ToArray();
                    for (int y = 0; y < size[1]; y++)
                        for (int x = 0; x < size[0]; x++)
                            map.data[x, y] = '#';
                }
                else
                {
                    int pos = int.Parse(v[2].Substring(2));
                    int steps = int.Parse(v[4]);
                    Action<int> rotate = (v[1] == "row") ? new Action<int>(RotateRow) : new Action<int>(RotateCol);
                    for (int n = 0; n < steps; n++)
                        rotate(pos);
                }
            }
            return map;
        }

        static void PartAB()
        {
            List<string> input = ReadInput();
            Map map = new Map(50, 6, new Position(), '.');
            Map map2 = TransformMap(map, input);
            int sum = map2.data.Cast<char>().Where(x => x == '#').Count();
            Console.WriteLine("Part A: Result is {0}.", sum);
            Console.WriteLine("Part B: Result is:");
            map2.Print();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day08).Namespace + ":");
            PartAB();
        }
    }
}
