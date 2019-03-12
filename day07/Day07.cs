using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day07
{
    class Day07
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

        static List<string> GetSubstrings(string full, bool outsideBrackets)
        {
            List<string> stringsOut = new List<string>();
            List<string> stringsIn = new List<string>();
            int pos1 = 0;
            int pos2 = 0;
            bool outState = true;
            do
            {
                char c = outState ? '[' : ']';
                pos2 = full.IndexOf(c, pos1);
                int len = (pos2 >= 0) ? pos2 - pos1 : full.Length - pos1;
                string s = full.Substring(pos1, len);
                if (outState)
                    stringsOut.Add(s);
                else
                    stringsIn.Add(s);
                pos1 = pos2 + 1;
                outState = !outState;
            }
            while ((pos1 < full.Length) && (pos2 >= 0));
            return outsideBrackets ? stringsOut : stringsIn;
        }

        static bool HasPairAndReverse_ABBA(string s)
        {
            for (int i = 0; i < s.Length - 3; i++)
            {
                if ((s[i] == s[i + 3]) && (s[i + 1] == s[i + 2]) && (s[i] != s[i + 1]))
                    return true;
            }
            return false;
        }

        static List<string> FindSequences_ABA(string s)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < s.Length - 2; i++)
            {
                if ((s[i] == s[i + 2]) && (s[i] != s[i + 1]))
                    list.Add(s.Substring(i, 3));
            }
            return list;
        }

        static bool HasSequence_ABA(List<string> list, string sequence)
        {
            foreach (string s in list)
            {
                if (s.IndexOf(sequence) >= 0)
                    return true;
            }
            return false;
        }

        static void PartA()
        {
            List<string> input = ReadInput();
            int nFound = 0;
            foreach (string full in input)
            {
                bool pairOut = false;
                bool pairIn = false;
                foreach (string s in GetSubstrings(full, true))
                {
                    if (HasPairAndReverse_ABBA(s))
                        pairOut = true;
                }
                foreach (string s in GetSubstrings(full, false))
                {
                    if (HasPairAndReverse_ABBA(s))
                        pairIn = true;
                }
                if (pairOut && !pairIn)
                    nFound++;
            }
            Console.WriteLine("Part A: Result is {0}.", nFound);
        }

        static void PartB()
        {
            List<string> input = ReadInput();
            int nFound = 0;
            foreach (string full in input)
            {
                List<string> insideList = GetSubstrings(full, false);
                bool sslOk = false;
                foreach (string s in GetSubstrings(full, true))
                {
                    foreach (string seq in FindSequences_ABA(s))
                    {
                        string revSeq = new string(new[] { seq[1], seq[0], seq[1] });
                        if (HasSequence_ABA(insideList, revSeq))
                            sslOk = true;
                    }
                }
                if (sslOk)
                    nFound++;
            }
            Console.WriteLine("Part B: Result is {0}.", nFound);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day07).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
