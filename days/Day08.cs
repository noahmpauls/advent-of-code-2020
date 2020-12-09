using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace days
{
    public class Day08
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day08\input.txt";
            IProgram input = ProcessInputFile(path);
            int programLength = input.Accept(new TotalLength());
            Run v = new Run(programLength);
            input.Accept(v);
            return v.Accumulator;
        }

        public static int Part2()
        {
            // TODO: make this more visitor-ey
            const string path = Helpers.inputPath + @"\day08\input.txt";
            IProgram input = ProcessInputFile(path);
            int programLength = input.Accept(new TotalLength());

            IList<(string, int)> instructions = Helpers.ProcessInputFile(path, StringToInstruction);
            for (int i=0; i < instructions.Count; i++)
            {
                IList<IProgram> newInstructions = instructions.Select((inst, j) =>
                {
                    switch (inst.Item1)
                    {
                        case "acc": return IProgram.MakeACC(inst.Item2);
                        case "nop":
                            {
                                if (i == j) return IProgram.MakeJMP(inst.Item2);
                                else return IProgram.MakeNOP(inst.Item2);
                            }

                        case "jmp":
                            {
                                if (i == j) return IProgram.MakeNOP(inst.Item2);
                                else return IProgram.MakeJMP(inst.Item2);
                            }
                        default: throw new ArgumentException("Invalid opcode.");
                    }
                }).ToList();

                IProgram sequence = IProgram.MakeSequence(newInstructions);
                Run rv = new Run(programLength);
                if (sequence.Accept(rv))
                {
                    Console.WriteLine("Index changed: " + i);
                    return rv.Accumulator;
                }
            }
            throw new Exception("Value not found");
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IProgram ProcessInputFile(string path)
        {
            return IProgram.MakeSequence(Helpers.ProcessInputFile(path, StringToProgram));
        }

        public static IProgram StringToProgram(string line)
        {
            Regex rx = new Regex("(?<op>.+) (?<arg>[+-][0-9]+)");
            Match match = rx.Match(line);
            string operation = match.Groups["op"].Value;
            int argument = int.Parse(match.Groups["arg"].Value);
            return IProgram.MakeInstruction(operation, argument);
        }
        public static (string, int) StringToInstruction(string line)
        {
            Regex rx = new Regex("(?<op>.+) (?<arg>[+-][0-9]+)");
            Match match = rx.Match(line);
            string operation = match.Groups["op"].Value;
            int argument = int.Parse(match.Groups["arg"].Value);
            return (operation, argument);
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // TODO: make a generalized visitor that takes a set of string inputs and 
    // logs them during visiting, essentially allowing a custom visitor.

    // public class Day/* day */Result : DayResult<void, void> { }

    public interface IProgram
    {
        public interface IVisitor<T>
        {
            public T Visit(Sequence program);
            public T Visit(NOP nop);
            public T Visit(ACC acc);
            public T Visit(JMP jmp);
        }
        public T Accept<T>(IVisitor<T> visitor);
        public static IProgram MakeSequence(IList<IProgram> instructions)
        {
            return new Sequence { Instructions = instructions };
        }
        public static IProgram MakeInstruction(string operation, int argument)
        {
            const string nop = "nop";
            const string acc = "acc";
            const string jmp = "jmp";

            switch (operation)
            {
                case nop:
                    return MakeNOP(argument);

                case acc:
                    return MakeACC(argument);

                case jmp:
                    return MakeJMP(argument);

                default:
                    throw new ArgumentException("Invalid operation type.");
            }
        }

        public static IProgram MakeNOP(int argument)
        {
            return new NOP { Argument = argument };
        }

        public static IProgram MakeACC(int argument)
        {
            return new ACC { Argument = argument };
        }

        public static IProgram MakeJMP(int argument)
        {
            return new JMP { Argument = argument };
        }
    }

    public class Sequence : IProgram
    {
        public IList<IProgram> Instructions { get; set; }

        public T Accept<T>(IProgram.IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class NOP : IProgram
    {
        public int Argument { get; set; } = 0;

        public T Accept<T>(IProgram.IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ACC : IProgram
    {
        public int Argument { get; set; } = 0;

        public T Accept<T>(IProgram.IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class JMP : IProgram
    {
        public int Argument { get; set; } = 0;

        public T Accept<T>(IProgram.IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class TotalLength : IProgram.IVisitor<int>
    {
        public int Length { get; set; } = 0;
        public int Visit(Sequence program)
        {
            int length = 0;
            foreach (var instruction in program.Instructions)
            {
                length += instruction.Accept(this);
            }
            return length;
        }

        public int Visit(NOP nop)
        {
            return 1;
        }

        public int Visit(ACC acc)
        {
            return 1;
        }

        public int Visit(JMP jmp)
        {
            return 1;
        }
    }

    public class InstructionList : IProgram.IVisitor<IList<IProgram>>
    {
        public IList<IProgram> Visit(Sequence program)
        {
            IList<IProgram> instructions = new List<IProgram>();
            foreach (IProgram p in program.Instructions)
            {
                instructions = instructions.Concat(p.Accept(this)).ToList();
            }
            return instructions;
        }

        public IList<IProgram> Visit(NOP nop)
        {
            return new List<IProgram> { nop };
        }

        public IList<IProgram> Visit(ACC acc)
        {
            return new List<IProgram> { acc };
        }

        public IList<IProgram> Visit(JMP jmp)
        {
            return new List<IProgram> { jmp };
        }
    }

    public class Flatten : IProgram.IVisitor<IProgram>
    {
        public IProgram Visit(Sequence program)
        {
            IList<IProgram> instructions = program.Accept(new InstructionList());
            return IProgram.MakeSequence(instructions);
        }

        public IProgram Visit(NOP nop)
        {
            return nop;
        }

        public IProgram Visit(ACC acc)
        {
            return acc;
        }

        public IProgram Visit(JMP jmp)
        {
            return jmp;
        }
    }

    public class Run : IProgram.IVisitor<bool>
    {
        public int ProgramCounter { get; set; } = 0;
        public int Accumulator { get; set; } = 0;
        public int ProgramLength { get; set; }
        public ISet<int> IndicesRun { get; set; } = new HashSet<int>();
        public bool Looped { get; set; } = false;
        public bool Terminated { get; set; } = false;
        public bool DebugDisplay { get; set; } = false;

        public Run(int programLength)
        {
            ProgramLength = programLength;
        }

        public Run(int programLength, bool debugDisplay)
        {
            ProgramLength = programLength;
            DebugDisplay = debugDisplay;
        }

        public bool Visit(Sequence program)
        {
            while (!(Looped || Terminated))
            {
                program.Instructions[ProgramCounter].Accept(this);
            }
            return Terminated;
        }
        public bool Visit(NOP nop)
        {
            Debug("NOP: ");
            UpdateState(1, 0);
            return Terminated;
        }
        public bool Visit(ACC acc)
        {
            Debug("ACC: ");
            UpdateState(1, acc.Argument);
            return Terminated;
        }
        public bool Visit(JMP jmp)
        {
            Debug("JMP " + jmp.Argument + ": ");
            UpdateState(jmp.Argument, 0);
            return Terminated;
        }

        private void UpdateState(int pcDelta, int accDelta)
        {
            if (!(Looped || Terminated))
            {
                Debug(ProgramCounter + " : " + Accumulator + "\n");
                ProgramCounter += pcDelta;
                Accumulator += accDelta;
                if (IndicesRun.Contains(ProgramCounter))
                {
                    Looped = true;
                    Debug("looped: ");
                    Debug(ProgramCounter + " : " + Accumulator + "\n");
                } else if (ProgramCounter >= ProgramLength) {
                    Terminated = true;
                    Debug("terminated: ");
                    Debug(ProgramCounter + " : " + Accumulator + "\n");
                }
                IndicesRun.Add(ProgramCounter);
            }
        }

        private void Debug(string text)
        {
            if (DebugDisplay) Console.Write(text);
        }
    }

    public class ReplaceAndRun : IProgram.IVisitor<IList<Run>>
    {
        private IProgram sourceProgram;
        private int sourceLength;
        public ReplaceAndRun(IProgram program)
        {
            sourceProgram = program.Accept(new Flatten());
            sourceLength = program.Accept(new TotalLength());
        }

        IList<Run> IProgram.IVisitor<IList<Run>>.Visit(Sequence program)
        {
            IList<Run> terminated = new List<Run>();
            // TODO
            return terminated;
        }

        IList<Run> IProgram.IVisitor<IList<Run>>.Visit(NOP nop)
        {
            IList<Run> terminated = new List<Run>();
            Run nopRun = new Run(1);
            if (nop.Accept(nopRun))
            {
                terminated.Add(nopRun);
            }

            IProgram jmp = IProgram.MakeNOP(nop.Argument);
            Run jmpRun = new Run(1);
            if (jmp.Accept(jmpRun))
            {
                terminated.Add(jmpRun);
            }

            return terminated;
        }

        IList<Run> IProgram.IVisitor<IList<Run>>.Visit(ACC acc)
        {
            IList<Run> terminated = new List<Run>();
            Run accRun = new Run(1);
            if (acc.Accept(accRun))
            {
                terminated.Add(accRun);
            }

            return terminated;
        }

        IList<Run> IProgram.IVisitor<IList<Run>>.Visit(JMP jmp)
        {
            IList<Run> terminated = new List<Run>();
            Run jmpRun = new Run(1);
            if (jmp.Accept(jmpRun))
            {
                terminated.Add(jmpRun);
            }

            IProgram nop = IProgram.MakeNOP(jmp.Argument);
            Run nopRun = new Run(1);
            if (nop.Accept(nopRun))
            {
                terminated.Add(nopRun);
            }

            return terminated;
        }
    }
}