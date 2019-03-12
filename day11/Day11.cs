using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode;

namespace day11
{
    class Day11
    {
        // My puzzle input:
        //
        // F4 
        // F3             CM    UM    RM    LM
        // F2          CG    UG    RG    LG
        // F1 E  PG PM 
        //
        // Brief rules: 
        // - Elevator must always transport 1 or 2 objects and moves one floor at a time.
        // - A Microchip must be paired with its generator if another generator is on the same floor.

        const int minFloor = 1;
        const int maxFloor = 4;
        // Index 0 is the elevator, then each index pair is a generator and its corresponding microchip
        static readonly int[] inputA = new[] { 1, 1, 1, 2, 3, 2, 3, 2, 3, 2, 3 };
        static readonly int[] inputB = new[] { 1, 1, 1, 2, 3, 2, 3, 2, 3, 2, 3, 1, 1, 1, 1 };

        static string GetConfigId(int[] state)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in state)
                sb.Append(i.ToString());
            return sb.ToString();
        }

        //static bool IsStateValid(int[] state)
        //{
        //    bool valid = true;
        //    for (int f = minFloor; (f <= maxFloor) && valid; f++)
        //    {
        //        // Even index --> generator, odd index --> microchip
        //        var floorObjects = state.Skip(1).Select((a, i) => new { a, i }).Where(x => x.a == f).Select(x => x.i);
        //        if (floorObjects.Where(x => x % 2 == 0).Count() > 0)
        //        {
        //            foreach (int i in floorObjects)
        //            {
        //                if ((i % 2 == 1) && !floorObjects.Contains(i - 1))
        //                    valid = false;
        //            }
        //        }
        //    }
        //    return valid;
        //}

        static bool IsStateValid(int[] state)
        {
            bool valid = true;
            for (int f = minFloor; (f <= maxFloor) && valid; f++)
            {
                // Odd index --> generator, even index --> microchip
                bool generator = false;
                for (int i = 1; (i < state.Length) && !generator; i += 2)
                    if (state[i] == f)
                        generator = true;
                if (generator)
                {
                    for (int i = 2; (i < state.Length) && valid; i += 2)
                        if ((state[i] == f) && (state[i - 1] != f))
                            valid = false;
                }
            }
            return valid;
        }

        static bool ChangeFloor(int[] state, List<int> move, bool up, out int[] newState)
        {
            newState = state.Clone() as int[];
            foreach (int i in move.Concat(new List<int>() { 0 }))
                newState[i] += (up ? 1 : -1);
            return IsStateValid(newState);
        }

        static int ReachEndState(int[] start, int[] target, out int tested)
        {
            HashSet<string> visited = new HashSet<string>();
            List<int[]> toCheck = new List<int[]>() { start };
            int iter = 0;
            string targetId = GetConfigId(target);
            while (!visited.Contains(targetId) && toCheck.Count > 0)
            {
                iter++;
                //Console.WriteLine("Iter {0}: Checking {1} new states...", iter, toCheck.Count);
                Console.Write(".");
                List<int[]> nextToCheck = new List<int[]>();
                void AddOkState(int[] state)
                {
                    string id = GetConfigId(state);
                    if (!visited.Contains(id))
                    {
                        visited.Add(id);
                        nextToCheck.Add(state);
                    }
                }
                foreach (int[] state in toCheck)
                {
                    List<int> candidates = state.Skip(1).Select((x, i) => new { x, i }).Where(a => a.x == state[0]).Select(a => a.i + 1).ToList();
                    List<List<int>> candidateCombos = Algorithms.GetCombinations(candidates, 2);
                    foreach (List<int> tryMove in candidateCombos)
                    {
                        if (state[0] < maxFloor)
                            if (ChangeFloor(state, tryMove, true, out int[] newState))
                                AddOkState(newState);
                        if (state[0] > minFloor)
                            if (ChangeFloor(state, tryMove, false, out int[] newState))
                                AddOkState(newState);
                    }
                }
                toCheck = nextToCheck;
            }
            Console.WriteLine();
            if (!visited.Contains(targetId))
                throw new InvalidOperationException();
            tested = visited.Count;
            return iter;
        }

        static void PartA()
        {
            int[] target = new[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
            int iter = ReachEndState(inputA, target, out int tested);
            Console.WriteLine("Part A: Result is {0} (tested {1} different states).", iter, tested);
        }

        static void PartB()
        {
            int[] target = new[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
            int iter = ReachEndState(inputB, target, out int tested);
            Console.WriteLine("Part B: Result is {0} (tested {1} different states).", iter, tested);
        }

        static void TestAlgos()
        {
            List<int> test = new List<int>() { 0, 1, 2, 3, 4 };

            var combos2 = test.Combinations(2).ToList();
            var combos1 = test.Combinations(1).ToList();
            var combos = combos1.Concat(combos2).ToList();
            Console.WriteLine("Combinations():");
            combos.ForEach(list => Console.WriteLine(String.Join(", ", list)));

            List<List<int>> test2 = Algorithms.GetCombinations(test.Take(3).ToList());
            Console.WriteLine("GetCombinations():");
            test2.ForEach(list => Console.WriteLine(String.Join(", ", list)));

            List<List<int>> test3 = Algorithms.GetCombinations(test, 3);
            Console.WriteLine("GetCombinations() using max length:");
            test3.ForEach(list => Console.WriteLine(String.Join(", ", list)));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day11).Namespace + ":");
            //TestAlgos();
            PartA();
            PartB();
        }
    }
}
