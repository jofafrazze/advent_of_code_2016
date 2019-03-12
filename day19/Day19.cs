using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;

namespace day19
{
    class Day19
    {
        const int input = 3017957;

        class Elve
        {
            public int number;
            public int presents;
        }

        static void PartA1()
        {
            Elve[] elves = new Elve[input];
            for (int i = 0; i < input; i++)
                elves[i] = new Elve() { number = i + 1, presents = 1 };
            while (elves.Length > 1)
            {
                for (int i = 0; i < elves.Length; i += 2)
                {
                    int stealIndex = (i + 1) % elves.Length;
                    elves[i].presents += elves[stealIndex].presents;
                    elves[stealIndex].presents = 0;
                }
                elves = elves.Where(x => x.presents > 0).ToArray();
            }
            Console.WriteLine("Part A1: Result is {0}.", elves.First().number);
        }

        static void PartA2()
        {
            LinkedList<Elve> elves = new LinkedList<Elve>();
            for (int i = 0; i < input; i++)
                elves.AddLast(new Elve() { number = i + 1, presents = 1 });
            LinkedListNode<Elve> pos = elves.First;
            while (elves.Count > 1)
            {
                LinkedListNode<Elve> steal = pos.NextOrFirst();
                pos.Value.presents += steal.Value.presents;
                elves.Remove(steal);
                pos = pos.NextOrFirst();
            }
            Console.WriteLine("Part A2: Result is {0}.", elves.First().number);
        }

        static void PartB()
        {
            LinkedList<Elve> elves = new LinkedList<Elve>();
            for (int i = 0; i < input; i++)
                elves.AddLast(new Elve() { number = i + 1, presents = 1 });
            LinkedListNode<Elve> currentPos = elves.First;
            int stealOffs = elves.Count / 2;
            LinkedListNode<Elve> stealPos = currentPos;
            for (int i = 0; i < stealOffs; i++)
                stealPos = stealPos.NextOrFirst();
            while (elves.Count > 1)
            {
                currentPos.Value.presents += stealPos.Value.presents;
                var tempPos = stealPos;
                stealPos = stealPos.NextOrFirst();
                elves.Remove(tempPos);
                while (stealOffs > elves.Count / 2)
                {
                    stealOffs--;
                    stealPos = stealPos.PreviousOrLast();
                }
                currentPos = currentPos.NextOrFirst();
                stealPos = stealPos.NextOrFirst();
            }
            Console.WriteLine("Part B: Result is {0}.", elves.First().number);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day19).Namespace + ":");
            PartA1();
            PartA2();
            PartB();
        }
    }
}
