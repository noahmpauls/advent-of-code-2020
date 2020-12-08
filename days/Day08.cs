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
            // TODO: make length a method on IProgram objects rather than a visitor
            var lv = new LengthVisitor();
            input.Accept(lv);
            RunVisitor v = new RunVisitor(lv.Length);
            input.Accept(v);
            return v.Accumulator;
        }

        public static int Part2()
        {
            // TODO: make this more visitor-ey
            const string path = Helpers.inputPath + @"\day08\input.txt";
            IProgram input = ProcessInputFile(path);
            var lv = new LengthVisitor();
            input.Accept(lv);
            int programLength = lv.Length;

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
                RunVisitor rv = new RunVisitor(programLength);
                sequence.Accept(rv);
                if (rv.Terminated)
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

    // public class Day/* day */Result : DayResult<void, void> { }

    public interface IProgram
    {
        public void Accept(IProgramVisitor visitor);
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

        public void Accept(IProgramVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class NOP : IProgram
    {
        public int Argument { get; set; } = 0;

        public void Accept(IProgramVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ACC : IProgram
    {
        public int Argument { get; set; } = 0;

        public void Accept(IProgramVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class JMP : IProgram
    {
        public int Argument { get; set; } = 0;

        public void Accept(IProgramVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // TODO: make return types generic
    public interface IProgramVisitor
    {
        public void Visit(Sequence program);
        public void Visit(NOP nop);
        public void Visit(ACC acc);
        public void Visit(JMP jmp);
    }

    public class LengthVisitor : IProgramVisitor
    {
        public int Length { get; set; } = 0;
        public void Visit(Sequence program)
        {
            foreach (var instruction in program.Instructions)
            {
                instruction.Accept(this);
            }
        }

        public void Visit(NOP nop)
        {
            Length++;
        }

        public void Visit(ACC acc)
        {
            Length++;
        }

        public void Visit(JMP jmp)
        {
            Length++;
        }
    }

    public class RunVisitor : IProgramVisitor
    {
        public int ProgramCounter { get; set; } = 0;
        public int Accumulator { get; set; } = 0;
        public int ProgramLength { get; set; }
        public ISet<int> IndicesRun { get; set; } = new HashSet<int>();
        public bool Looped { get; set; } = false;
        public bool Terminated { get; set; } = false;
        public bool DebugDisplay { get; set; } = false;

        public RunVisitor(int programLength)
        {
            ProgramLength = programLength;
        }

        public RunVisitor(int programLength, bool debugDisplay)
        {
            ProgramLength = programLength;
            DebugDisplay = debugDisplay;
        }

        public void Visit(Sequence program)
        {
            while (!(Looped || Terminated))
            {
                program.Instructions[ProgramCounter].Accept(this);
            }
        }
        public void Visit(NOP nop)
        {
            Debug("NOP: ");
            UpdateState(1, 0);
        }
        public void Visit(ACC acc)
        {
            Debug("ACC: ");
            UpdateState(1, acc.Argument);
        }
        public void Visit(JMP jmp)
        {
            Debug("JMP " + jmp.Argument + ": ");
            UpdateState(jmp.Argument, 0);
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

    public class ReplaceVisitor : IProgramVisitor
    {
        public int ProgramCounter { get; set; } = 0;
        public int Accumulator { get; set; } = 0;
        public int ProgramLength { get; set; }
        public ISet<int> IndicesRun { get; set; } = new HashSet<int>();
        public bool Looped { get; set; } = false;
        public bool Terminated { get; set; } = false;
        public int IndexAltered { get; set; } = -1;
        public bool DebugDisplay { get; set; } = false;

        public void Visit(Sequence program)
        {
            while (!(Looped || Terminated || IndexAltered >= 0))
            {
                program.Instructions[ProgramCounter].Accept(this);
            }
        }
        public void Visit(ACC acc)
        {
            Debug("ACC: ");
            UpdateState(1, acc.Argument);
        }
        public void Visit(NOP nop)
        {
            if (IndexAltered < 0)
            {
                // stop here and try two new alternate realities
                ReplaceVisitor rvSame = new ReplaceVisitor
                {
                    ProgramCounter = this.ProgramCounter,
                    Accumulator = this.Accumulator,
                    ProgramLength = this.ProgramLength,
                    IndicesRun = this.IndicesRun,
                    Looped = this.Looped,
                    Terminated = this.Terminated,
                    IndexAltered = this.IndexAltered,
                    DebugDisplay = this.DebugDisplay
                };
            }
            if (IndexAltered == ProgramCounter)
            Debug("NOP: ");
            UpdateState(1, 0);
        }
        public void Visit(JMP jmp)
        {
            Debug("JMP " + jmp.Argument + ": ");
            UpdateState(jmp.Argument, 0);
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
                }
                else if (ProgramCounter >= ProgramLength)
                {
                    Terminated = true;
                    Debug("terminated: ");
                    Debug(ProgramCounter + " : " + Accumulator + "\n");
                }
                IndicesRun.Add(ProgramCounter);
            }
        }

        private void SetState(int pc, int acc)
        {
            ProgramCounter = pc;
            Accumulator = acc;
            if (ProgramCounter >= ProgramLength)
            {
                Terminated = true;
                Debug("terminated: ");
                Debug(ProgramCounter + " : " + Accumulator + "\n");
            }
        }

        private void Debug(string text)
        {
            if (DebugDisplay) Console.Write(text);
        }
    }

}