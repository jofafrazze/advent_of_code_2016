using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day14
{
    class Day14
    {
        static bool FindAnyChar3InARow(string s, out char c)
        {
            for (int i = 0; i < (s.Length - 2); i++)
            {
                if ((s[i] == s[i + 1]) && (s[i + 1] == s[i + 2]))
                {
                    c = s[i];
                    return true;
                }
            }
            c = ' ';
            return false;
        }

        static bool FindChar5InARow(string s, char c)
        {
            for (int i = 0; i < (s.Length - 4); i++)
            {
                if ((s[i] == c) && (s[i + 1] == c) && (s[i + 2] == c) && (s[i + 3] == c) && (s[i + 4] == c))
                    return true;
            }
            return false;
        }

        const string input = "ihaygndm";

        static void PartA()
        {
            MD5 md5 = MD5.Create();
            Dictionary<int, string> hashes = new Dictionary<int, string>();
            int nextHashToGenerate = 0;
            string GetHash(int x)
            {
                while (nextHashToGenerate <= x)
                {
                    const int batchSize = 1000;
                    for (int n = 0; n < batchSize; n++)
                    {
                        int currentHashIndex = nextHashToGenerate + n;
                        byte[] bytes = Encoding.UTF8.GetBytes(input + currentHashIndex.ToString());
                        byte[] hash = md5.ComputeHash(bytes);
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in hash)
                            sb.Append(b.ToString("x2"));
                        hashes[currentHashIndex] = sb.ToString();
                    }
                    nextHashToGenerate += batchSize;
                }
                return hashes[x];
            }
            int i = 0;
            int keys = 0;
            while (keys < 64)
            {
                string candidate = GetHash(i);
                if (FindAnyChar3InARow(candidate, out char c))
                {
                    for (int a = 0; a < 1000; a++)
                    {
                        string candidate5 = GetHash(i + 1 + a);
                        if (FindChar5InARow(candidate5, c))
                        {
                            keys++;
                            break;
                        }
                    }
                }
                i++;
            }
            Console.WriteLine("Part A: Result is {0}.", i - 1);
        }

        static void PartB()
        {
            MD5 md5 = MD5.Create();
            Dictionary<int, string> hashes = new Dictionary<int, string>();
            int nextHashToGenerate = 0;
            string GetHash(int x)
            {
                while (nextHashToGenerate <= x)
                {
                    const int batchSize = 1000;
                    for (int n = 0; n < batchSize; n++)
                    {
                        int currentHashIndex = nextHashToGenerate + n;
                        byte[] bytes = Encoding.UTF8.GetBytes(input + currentHashIndex.ToString());
                        byte[] hash = md5.ComputeHash(bytes);
                        for (int z = 0; z < 2016; z++)
                        {
                            StringBuilder sb2 = new StringBuilder();
                            foreach (byte b in hash)
                                sb2.Append(b.ToString("x2"));
                            hash = md5.ComputeHash(Encoding.UTF8.GetBytes(sb2.ToString()));
                        }
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in hash)
                            sb.Append(b.ToString("x2"));
                        hashes[currentHashIndex] = sb.ToString();
                    }
                    nextHashToGenerate += batchSize;
                }
                return hashes[x];
            }
            int i = 0;
            int keys = 0;
            while (keys < 64)
            {
                string candidate = GetHash(i);
                if (FindAnyChar3InARow(candidate, out char c))
                {
                    for (int a = 0; a < 1000; a++)
                    {
                        string candidate5 = GetHash(i + 1 + a);
                        if (FindChar5InARow(candidate5, c))
                        {
                            keys++;
                            break;
                        }
                    }
                }
                i++;
            }
            Console.WriteLine("Part B: Result is {0}.", i - 1);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day14).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
