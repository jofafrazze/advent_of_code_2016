using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;

namespace day21
{
    class Day21
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

        static string ScrambleOne(string a, string rule)
        {
            string[] r = rule.Split(' ').ToArray();
            if (r[0] == "swap")
            {
                bool direct = (r[1] == "position");
                int i1 = direct ? int.Parse(r[2]) : a.IndexOf(r[2][0]);
                int i2 = direct ? int.Parse(r[5]) : a.IndexOf(r[5][0]);
                char[] c = a.ToCharArray();
                char w = c[i1];
                c[i1] = c[i2];
                c[i2] = w;
                return new string(c);
            }
            else if (r[0] == "rotate")
            {
                bool posBased = (r[1] == "based");
                bool right = (r[1] == "right") || posBased;
                int n = posBased ? a.IndexOf(r[6][0]) + 1 : int.Parse(r[2]);
                n += posBased && (n >= 5) ? 1 : 0;
                string b = a;
                for (int i = 0; i < n; i++)
                {
                    string pre = right ? b[b.Length - 1].ToString() : "";
                    string post = right ? "" : b[0].ToString();
                    b = pre + b.Substring(post.Length, b.Length - 1) + post;
                }
                return b;
            }
            else if (r[0] == "reverse")
            {
                int i1 = int.Parse(r[2]);
                int i2 = int.Parse(r[4]);
                char[] c = a.ToCharArray();
                char[] flip = a.Substring(i1, i2 - i1 + 1).ToArray();
                return new string(c.Take(i1).Concat(flip.Reverse()).Concat(c.Skip(i2 + 1)).ToArray());
            }
            else if (r[0] == "move")
            {
                int i1 = int.Parse(r[2]);
                int i2 = int.Parse(r[5]);
                List<char> w = new List<char>() { a[i1] };
                List<char> c = a.ToCharArray().ToList();
                c.RemoveAt(i1);
                return new string(c.Take(i2).Concat(w).Concat(c.Skip(i2)).ToArray());
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        static string Scramble(string original, List<string> operations)
        {
            string scrambled = original;
            foreach (string rule in operations)
            {
                scrambled = ScrambleOne(scrambled, rule);
            }
            return scrambled;
        }

        static void PartA()
        {
            List<string> operations = ReadInput();
            const string original = "abcdefgh";
            string scrambled = Scramble(original, operations);
            Console.WriteLine("Part A: Result is {0}.", scrambled);
        }

        static void PartB()
        {
            List<string> operations = ReadInput();
            const string original = "abcdefgh";
            const string target = "fbgdceah";
            List<List<char>> combos = Algorithms.HeapPermutation(original.ToCharArray().ToList());
            Console.WriteLine("Brute forcing testing {0} combinations...", combos.Count);
            string currentOriginal = "";
            string currentScrambled = "";
            foreach (List<char> list in combos)
            {
                currentOriginal = new string(list.ToArray());
                currentScrambled = Scramble(currentOriginal, operations);
                if (currentScrambled == target)
                    break;
            }
            Console.WriteLine("Part B: Result is {0}.", currentOriginal);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day21).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
