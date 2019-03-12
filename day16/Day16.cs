using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day16
{
    class Day16
    {
        static string DragonCurveExpand(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Append("0");
            for (int i = s.Length - 1; i >= 0; i--)
                sb.Append(s[i] == '1' ? "0" : "1");
            return sb.ToString();
        }

        static string DragonCurveChecksum(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length - 1; i += 2)
                sb.Append(s[i] == s[i + 1] ? "1" : "0");
            return sb.ToString();
        }

        static string GetDiskChecksum(string s, int diskSize)
        {
            string data = s;
            do
            {
                data = DragonCurveExpand(data);
            }
            while (data.Length < diskSize);
            string checksum = data.Substring(0, diskSize);
            do
            {
                checksum = DragonCurveChecksum(checksum);
            }
            while (checksum.Length % 2 == 0);
            return checksum;
        }

        const string input = "11011110011011101";

        static void PartA()
        {
            const int diskSize = 272;
            string checksum = GetDiskChecksum(input, diskSize);
            Console.WriteLine("Part A: Result is {0}.", checksum);
        }

        static void PartB()
        {
            const int diskSize = 35651584;
            string checksum = GetDiskChecksum(input, diskSize);
            Console.WriteLine("Part B: Result is {0}.", checksum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day16).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
