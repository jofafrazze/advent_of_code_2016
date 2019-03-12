using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day23
{
    class Day23
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
            public Dictionary<char, long> registers;
            public List<OpCode> opCodes;
            public Dictionary<string, int> instructionSet;
            public Executable()
            {
                program = new List<Instruction>();
                pc = 0;
                registers = new Dictionary<char, long>();
                opCodes = new List<OpCode>()
                {
                    new OpCode("cpy", delegate(string x, string y) { if (char.IsLetter(y[0])) { InitReg(y[0]); registers[y[0]] = GetValue(x); } return true; }),
                    new OpCode("inc", delegate(string x, string _) { if (char.IsLetter(x[0])) { InitReg(x[0]); registers[x[0]]++; } return true; }),
                    new OpCode("dec", delegate(string x, string _) { if (char.IsLetter(x[0])) { InitReg(x[0]); registers[x[0]]--; } return true; }),
                    new OpCode("jnz", delegate(string x, string y) { if (GetValue(x) != 0) pc += (int)(GetValue(y) - 1); return true; }),
                    new OpCode("tgl", delegate(string x, string _) 
                    {
                        int index = pc + (int)GetValue(x);
                        if (index >= 0 && index < program.Count)
                        {
                            Instruction i = program[index];
                            if (i.opCode.name == "inc") i.opCode = opCodes.Find(a => a.name == "dec");
                            else if (i.opCode.name == "dec") i.opCode = opCodes.Find(a => a.name == "inc");
                            else if (i.opCode.name == "tgl") i.opCode = opCodes.Find(a => a.name == "inc");
                            else if (i.opCode.name == "cpy") i.opCode = opCodes.Find(a => a.name == "jnz");
                            else if (i.opCode.name == "jnz") i.opCode = opCodes.Find(a => a.name == "cpy");
                            else throw new ArgumentOutOfRangeException();
                            program[index] = i;
                        }
                        return true;
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
                while ((pc >= 0) && (pc < program.Count) && keepOn)
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

        static void PartA()
        {
            Executable exe = new Executable();
            exe.program = ReadInput(exe);
            exe.registers['a'] = 7;
            exe.Execute(out int n);
            Console.WriteLine("Exe executed {0} instructions", n);
            Console.WriteLine("Part A: Result is {0}.", exe.registers['a']);
        }

        static void PartB()
        {
            Executable exe = new Executable();
            exe.program = ReadInput(exe);
            exe.registers['a'] = 12;
            exe.Execute(out int n);
            Console.WriteLine("Exe executed {0} instructions", n);
            Console.WriteLine("Part B: Result is {0}.", exe.registers['a']);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2016 - " + typeof(Day23).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
