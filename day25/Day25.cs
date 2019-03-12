using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day25
{
    class Day25
    {
        struct OpCode
        {
            public string name;
            public Func<string, string, bool> func;
            public OpCode(string n, Func<string, string, bool> f) { name = n; func = f; }
        };

        struct Instruction
        {
            public OpCode opCode;
            public string param1;
            public string param2;
            public bool Execute()
            {
                return opCode.func(param1, param2);
            }
        };

        class Executable
        {
            public List<Instruction> program;
            public int pc;
            public int maxExec;
            public Dictionary<char, long> registers;
            public Dictionary<char, long> lastRegisters;
            public List<OpCode> opCodes;
            public Dictionary<string, int> instructionSet;

            public Executable(int maxInstructionsExecuted = 0)
            {
                program = new List<Instruction>();
                pc = 0;
                maxExec = maxInstructionsExecuted;
                registers = new Dictionary<char, long>();
                lastRegisters = new Dictionary<char, long>();
                opCodes = new List<OpCode>()
                {
                    new OpCode("cpy", delegate(string x, string y) { InitReg(y[0]); registers[y[0]] = GetValue(x); return true; }),
                    new OpCode("inc", delegate(string x, string _) { InitReg(x[0]); registers[x[0]]++; return true; }),
                    new OpCode("dec", delegate(string x, string _) { InitReg(x[0]); registers[x[0]]--; return true; }),
                    new OpCode("jnz", delegate(string x, string y) { if (GetValue(x) != 0) pc += (int)(GetValue(y) - 1); return true; }),
                    new OpCode("out", delegate(string x, string _) 
                    {
                        InitReg(x[0]);
                        bool ok = !lastRegisters.ContainsKey(x[0]) || (registers[x[0]] != lastRegisters[x[0]]);
                        lastRegisters = new Dictionary<char, long>(registers);
                        return ok;
                    }),
                };
                instructionSet = opCodes.Select((x, i) => new { x, i }).ToDictionary(a => a.x.name, a => a.i);
            }

            public void InitReg(char c)
            {
                if (!registers.ContainsKey(c)) 
                    registers[c] = 0;
            }

            public long GetValue(char c) { return GetValue(c.ToString()); }
            public long GetValue(string s)
            {
                if (s.Length == 1 && Char.IsLetter(s[0]))
                {
                    InitReg(s[0]);
                    return registers[s[0]];
                }
                else
                    return int.Parse(s);
            }

            public bool Execute(out int instructionsExecuted)
            {
                instructionsExecuted = 0;
                bool keepOn = true;
                while ((pc >= 0) && (pc < program.Count) && keepOn && (instructionsExecuted < maxExec))
                {
                    instructionsExecuted++;
                    keepOn &= program[pc].Execute();
                    if (keepOn)
                        pc++;
                }
                return !keepOn;
            }
        };

        static List<Instruction> ReadInput(Executable exe)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Instruction> list = new List<Instruction>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Instruction i = new Instruction();
                string[] s = line.Split(' ').ToArray();
                i.opCode = exe.opCodes[exe.instructionSet[s[0]]];
                i.param1 = s[1];
                i.param2 = (s.Count() > 2) ? s[2] : "";
                list.Add(i);
            }
            return list;
        }

        static int RunExecutable(int a, int maxRowsExec)
        {
            Executable exe = new Executable(maxRowsExec);
            exe.program = ReadInput(exe);
            exe.registers['a'] = a;
            exe.Execute(out int n);
            return n;
        }

        static void PartA()
        {
            const int maxRowsExec = 1000000;
            int n = 0;
            int a = 0;
            for (a = 0; n < maxRowsExec; a++)
            {
                n = RunExecutable(a, maxRowsExec);
                Console.Write(".");
            }
            Console.WriteLine();
            Console.WriteLine("Part A: Result is {0}.", a - 1);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day25).Namespace + ":");
            PartA();
        }
    }
}
