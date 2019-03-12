using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day05
{
    class Day05
    {
        const string input = "uqwqemis";

        static void PartA()
        {
            MD5 md5 = MD5.Create();
            string password = "";
            int n = 0;
            for (; password.Length < 8; n++)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input + n.ToString());
                byte[] hash = md5.ComputeHash(bytes);
                if ((hash[0] == 0) && (hash[1] == 0) && ((hash[2] & 0xf0) == 00))
                    password = password + (hash[2] & 0x0f).ToString("x");
        }
            Console.WriteLine("Part A: Result is {0}.", password);
        }

        static void PartB()
        {
            MD5 md5 = MD5.Create();
            Dictionary<int, char> pwchars = new Dictionary<int, char>();
            int n = 0;
            for (; pwchars.Count < 8; n++)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input + n.ToString());
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                if ((hash[0] == 0) && (hash[1] == 0) && ((hash[2] & 0xf0) == 00))
                {
                    int pos = hash[2] & 0x0f;
                    if ((pos < 8) && !pwchars.ContainsKey(pos))
                        pwchars[pos] = hash[3].ToString("x2")[0];
                }
            }
            string password = "";
            for (int i = 0; i < 8; i++)
                password += pwchars[i];
            Console.WriteLine("Part B: Result is {0}.", password);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day05).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
