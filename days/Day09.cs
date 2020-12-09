using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day09
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static long Part1()
        {
            const string path = Helpers.inputPath + @"\day09\input.txt";
            IList<long> inputs = ProcessInputFile(path);
            int invalidIndex = InvalidIndices(inputs, 25)[0];
            return inputs[invalidIndex];
        }

        public static long Part2()
        {
            const string path = Helpers.inputPath + @"\day09\input.txt";
            IList<long> inputs = ProcessInputFile(path);
            int invalidIndex = InvalidIndices(inputs, 25)[0];
            long invalid = inputs[invalidIndex];

            (int, int) range = ContiguousSumRanges(inputs, invalid)[0];
            IList<long> vals = inputs.ToList().GetRange(range.Item1, range.Item2);
            return vals.Min() + vals.Max();
        }

        public static void Test()
        {
            const string path = Helpers.inputPath + @"\day09\test1.txt";
            IList<long> inputs = ProcessInputFile(path);
            int invalidIndex = InvalidIndices(inputs, 5)[0];
            Console.WriteLine(inputs[invalidIndex]);
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<long> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, line => long.Parse(line));
        }

        /* Main idea: provide a method for quickly referencing active sums,
         *  removing old sums, and adding new ones.
         *  
         * The `sumsAfter` dictionary has an index as a key, and a list of all
         *  the sums it creates with the indices that come after it. This way,
         *  each sum is only ever tracked once. A key only exists if the sums it
         *  creates are under active consideration; once it falls out of range,
         *  it is removed. A new index gets its own new empty entry, and new
         *  sums are added to any preceding active values that create sums with
         *  it.
         *  
         * When a sum is added or removed from `sumsAfter`, it is also added to 
         *  `sumCounts`. This keeps track of how many of a given sum are
         *  currently active. Keys are sums, and values are their counts. A key
         *  only exists if its value is > 0. When we check if the next value in
         *  the list has a sum in the previous values, we can do a quick check
         *  to see if it exists in `sumCounts`'s keys.
         * 
         * TODO: make DRYer
         * TODO: make more general, allowing for an arbitrary combo size
         * TODO: make `sumCounts` its own ADT to track counts of things
         */
        public static IList<int> InvalidIndices(IList<long> sequence, int preamble)
        {
            // initialize preamble
            IDictionary<long, int> sumCounts = new Dictionary<long, int>();
            IDictionary<int, IList<long>> sumsAfter = new Dictionary<int, IList<long>>();
            for (int i = 0; i < preamble; i++)
            {
                sumsAfter[i] = new List<long>();
                for (int j = i+1; j < preamble; j++)
                {
                    if (sequence[i] != sequence[j])
                    {
                        long sum = sequence[i] + sequence[j];
                        sumsAfter[i].Add(sum);
                        if (!sumCounts.ContainsKey(sum)) sumCounts[sum] = 0;
                        sumCounts[sum]++;
                    }
                }
            }

            IList<int> invalidIndices = new List<int>();

            // check next values
            for (int i = preamble; i < sequence.Count; i++)
            {
                long test = sequence[i];
                if (!sumCounts.ContainsKey(test)) invalidIndices.Add(i);

                int tailIndex = i - preamble;
                foreach (long oldSum in sumsAfter[tailIndex])
                {
                    if (--sumCounts[oldSum] == 0) sumCounts.Remove(oldSum);
                }
                sumsAfter.Remove(tailIndex);

                sumsAfter[i] = new List<long>();
                for (int j = tailIndex+1; j < i; j++)
                {
                    if (sequence[i] != sequence[j])
                    {
                        long newSum = sequence[i] + sequence[j];
                        sumsAfter[j].Add(newSum);
                        if (!sumCounts.ContainsKey(newSum)) sumCounts[newSum] = 0;
                        sumCounts[newSum]++;
                    }
                }
            }

            return invalidIndices;
        }

        // use a two finger approach:
        //  starting from the beginning, add one to the end if the sum is too small
        //  remove one from the beginning if sum is too large
        // TODO: use fewer conditionals, make DRYer
        public static IList<(int, int)> ContiguousSumRanges(IList<long> sequence, long target)
        {
            IList<(int, int)> ranges = new List<(int, int)>();
            int start = 0;
            int end = 1;
            long sum = sequence[start] + sequence[end];
            while (end < sequence.Count)
            {
                if (sum < target)
                {
                    end++;
                    if (end >= sequence.Count) break;
                    sum += sequence[end];
                }
                else if (sum > target)
                {
                    if (end == start + 1)
                    {
                        end++;
                        if (end >= sequence.Count) break;
                        sum += sequence[end];
                    }
                    else
                    {
                        sum -= sequence[start++];
                    }
                }
                else
                {
                    ranges.Add((start, end-start+1));
                    if (end == start + 1)
                    {
                        end++;
                        if (end >= sequence.Count) break;
                        sum += sequence[end];
                    }
                    else
                    {
                        sum -= sequence[start++];
                    }
                }
            }
            return ranges;
        }
    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}
