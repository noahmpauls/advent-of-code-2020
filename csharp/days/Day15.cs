using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day15
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day15\input.txt";
            IList<int> inputs = ProcessInputFile(path);

            const int turns = 2020;
            return MemoryGame(inputs, turns);
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day15\input.txt";
            IList<int> inputs = ProcessInputFile(path);

            const int turns = 30000000;
            return MemoryGame(inputs, turns);
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<int> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, line => int.Parse(line));
        }

        public static int MemoryGame(IList<int> start, int turns)
        {
            IDictionary<int, int> recentIndices = new Dictionary<int, int>();
            for (int i = 0; i < start.Count - 1; i++)
            {
                recentIndices.Add(start[i], i);
            }

            int mostRecent = start.Last();
            int mostRecentIndex = start.Count - 1;

            while (mostRecentIndex < turns-1)
            {
                int newRecent = 0;
                if (recentIndices.ContainsKey(mostRecent))
                {
                    newRecent = mostRecentIndex - recentIndices[mostRecent];
                }
                recentIndices[mostRecent] = mostRecentIndex;
                mostRecent = newRecent;
                mostRecentIndex++;
            }

            return mostRecent;
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}

