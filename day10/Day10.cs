using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day10
{
    class Day10
    {
        static List<string> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }

        static void PartAB()
        {
            List<string> input = ReadInput();
            Dictionary<int, List<int>> bots = new Dictionary<int, List<int>>();
            Dictionary<int, int> outputs = new Dictionary<int, int>();
            int botNumber = -1;
            void AddToBot(int b, int v)
            {
                if (!bots.ContainsKey(b))
                {
                    bots[b] = new List<int>();
                }
                bots[b].Add(v);
                bots[b].Sort();
            }
            void CopyValue(int source, int target, bool lowValue, bool targetBot)
            {
                int idx = lowValue ? 0 : 1;
                int sourceValue = bots[source][idx];
                if (targetBot)
                    AddToBot(target, sourceValue);
                else
                    outputs[target] = sourceValue;
            }
            void ProcessInput()
            {
                List<string> nextInput = new List<string>();
                foreach (string line in input)
                {
                    string[] s = line.Split(' ').ToArray();
                    if (s[0] == "value")
                    {
                        AddToBot(int.Parse(s[5]), int.Parse(s[1]));
                    }
                    else
                    {
                        int sender = int.Parse(s[1]);
                        if (bots.ContainsKey(sender) && bots[sender].Count() >= 2)
                        {
                            CopyValue(sender, int.Parse(s[6]), true, s[5] == "bot");
                            CopyValue(sender, int.Parse(s[11]), false, s[10] == "bot");
                            bots.Remove(sender);
                        }
                        else
                        {
                            nextInput.Add(line);
                        }
                    }
                }
                input = nextInput;
            }
            while (input.Count > 0)
            {
                ProcessInput();
                foreach (var kvp in bots)
                {
                    if ((kvp.Value.Count() >= 2) && (kvp.Value[0] == 17) && (kvp.Value[1] == 61))
                        botNumber = kvp.Key;
                }
            }
            Console.WriteLine("Part A: Result is {0}.", botNumber);
            Console.WriteLine("Part B: Result is {0}.", outputs[0] * outputs[1] * outputs[2]);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day10).Namespace + ":");
            PartAB();
        }
    }
}
