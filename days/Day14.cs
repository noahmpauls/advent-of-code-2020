using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day14
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static ulong Part1()
        {
            const string path = Helpers.inputPath + @"\day14\input.txt";
            IList<Tuple<Mask, IList<Tuple<ulong, ulong>>>> inputs = ProcessInputFile(path);

            IDictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            foreach(var opGroup in inputs)
            {
                Mask mask = opGroup.Item1;
                foreach (var op in opGroup.Item2)
                {
                    ulong newValue = op.Item2;
                    newValue &= mask.Zeros;
                    newValue |= mask.Ones;
                    memory[op.Item1] = newValue;
                }
            }

            //foreach (var k in memory)
            //{
            //    Console.WriteLine($" {k.Key}: {k.Value}");
            //}

            ulong sum = 0;
            foreach (var v in memory.Values)
            {
                sum += v;
            }
            return sum;
        }

        public static ulong Part2()
        {
            const string path = Helpers.inputPath + @"\day14\input.txt";
            IList<Tuple<Mask, IList<Tuple<ulong, ulong>>>> inputs = ProcessInputFile(path);

            IDictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            foreach (var opGroup in inputs)
            {
                Mask mask = opGroup.Item1;
                foreach (var op in opGroup.Item2)
                {
                    ulong newValue = op.Item2;
                    ulong nextAddress = op.Item1;
                    foreach(var addr in MaskAddress(mask, nextAddress))
                    {
                        memory[addr] = newValue;
                    }
                }
            }

            //foreach (var k in memory)
            //{
            //    Console.WriteLine($" {k.Key}: {k.Value}");
            //}

            ulong sum = 0;
            foreach (var v in memory.Values)
            {
                sum += v;
            }
            return sum;
        }

        //######################################################################
        // Methods
        //######################################################################

        private static IList<Tuple<Mask, IList<Tuple<ulong, ulong>>>> ProcessInputFile(string path)
        {
            string input = Helpers.GetFileAsString(path);

            Regex rx = new Regex(@"mask(.+\n)(mem.+\n)+");
            IList<Tuple<Mask, IList<Tuple<ulong, ulong>>>> opGroups = rx.Matches(input).Select(m => MaskAndMem(m.Value)).ToList();
            return opGroups;
        }

        private static Tuple<Mask, IList<Tuple<ulong, ulong>>> MaskAndMem(string lines)
        {
            Regex rxMask = new Regex("mask = (?<mask>[X10]+)");
            Regex rxMem = new Regex("mem\\[(?<address>[0-9]+)\\] = (?<value>[0-9]+)");

            Mask mask = new Mask(rxMask.Match(lines).Groups["mask"].Value);

            var memOps = new List<Tuple<ulong, ulong>>();
            foreach(Match match in rxMem.Matches(lines))
            {
                ulong address = ulong.Parse(match.Groups["address"].Value);
                ulong value = ulong.Parse(match.Groups["value"].Value);
                memOps.Add(new Tuple<ulong, ulong>(address, value));
            }
            return new Tuple<Mask, IList<Tuple<ulong, ulong>>>(mask, memOps);
        }

        private static IList<ulong> MaskAddress(Mask mask, ulong address)
        {
            ulong onesAddress = address | mask.Ones;

            IList<int> floatingIndices = mask.Raw.Reverse().Select((c, i) =>
            {
                if (c == 'X')
                    return i;
                else
                    return -1;
            }).Where(i => i >= 0).ToList();

            // iterative dynamic program
            var allAddresses = new HashSet<ulong>{ onesAddress };
            foreach (var i in floatingIndices)
            {
                var nextAddresses = new HashSet<ulong>();
                foreach (var addr in allAddresses)
                {
                    // add address with inverted bit
                    nextAddresses.Add(addr ^ ((ulong)1 << i));
                }
                allAddresses.UnionWith(nextAddresses);
            }

            return allAddresses.ToList();
        }

        //######################################################################
        // Inner Helper Classes
        //######################################################################

        public class Mask
        {
            public string Raw { get; private set; }
            public ulong Ones { get; private set; }
            public ulong Zeros { get; private set; }
            public Mask(string mask)
            {
                Raw = mask;

                ulong ones = 0;
                for (int i = 0; i < mask.Length; i++)
                {
                    int index = mask.Length - 1 - i;
                    if (mask[index] == '1')
                    {
                        ones |= ((ulong)1 << i);
                    }

                        
                }
                Ones = ones;

                ulong zeros = 0;
                for (int i = 0; i < mask.Length; i++)
                {
                    int index = mask.Length - 1 - i;
                    if (mask[index] == '0')
                    {
                        zeros |= ((ulong)1 << i);
                    }
                }
                Zeros = ~zeros;
            }
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}

// mask regex:  ?<mask>mask = (?<bits>[X10]+)
// mask group regex:  mask = (?<mask>[X01]+)\n(?<memlines>(?:mem.+\n)+)

