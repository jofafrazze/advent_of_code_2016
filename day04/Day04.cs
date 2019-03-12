using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day04
{
    class Day04
    {
        static List<string[]> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string[]> list = new List<string[]>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                char[] ints = "0123456789".ToCharArray();
                string[] s = new string[3];
                int i1 = line.IndexOfAny(ints);
                int i2 = line.IndexOf('[');
                s[0] = line.Substring(0, i1);
                s[1] = line.Substring(i1, i2 - i1);
                s[2] = line.Substring(i2 + 1, 5);
                list.Add(s);
            }
            return list;
        }

        static void PartAB()
        {
            List<string[]> input = ReadInput();
            List<string[]> okInput = ReadInput();
            int sum = 0;
            foreach (string[] s in input)
            {
                Dictionary<char, int> freq = new Dictionary<char, int>();
                foreach (char c in s[0])
                {
                    if (c != '-')
                    {
                        if (!freq.ContainsKey(c))
                            freq[c] = 0;
                        freq[c]++;
                    }
                }
                char[] checksum = freq.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Take(5).Select(x => x.Key).ToArray();
                if (new string(checksum) == s[2])
                {
                    okInput.Add(s);
                    sum += int.Parse(s[1]);
                }
            }
            Console.WriteLine("Part A: Result is {0}.", sum);
            foreach (string[] s in input)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in s[0])
                {
                    int n = int.Parse(s[1]);
                    char x = c;
                    if (c != '-')
                    {
                        for (int i = 0; i < n; i++)
                        {
                            x++;
                            if (x > 'z')
                                x = 'a';
                        }
                        sb.Append(x);
                    }
                    else
                        sb.Append(' ');
                }
                string decoded = sb.ToString();
                if (decoded.IndexOf("north") >= 0)
                    Console.WriteLine("Part B: Result is {0}{1}", decoded, s[1]);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day04).Namespace + ":");
            PartAB();
        }
    }
}
