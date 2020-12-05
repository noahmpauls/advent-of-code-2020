using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day05
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static long Part1()
        {
            const string path = Helpers.inputPath + @"\day05\input.txt";
            IList<(int, int)> inputs = ProcessInputFile(path);
            IList<long> ids = inputs.Select(inp => (long)(inp.Item1 * 8 + inp.Item2)).ToList();
            return ids.Max();
        }

        public static long Part2()
        {
            const string path = Helpers.inputPath + @"\day05\input.txt";
            IList<(int, int)> inputs = ProcessInputFile(path);
            ISet<long> ids = new HashSet<long>(inputs.Select(inp => (long)(inp.Item1 * 8 + inp.Item2)));
            long minId = ids.Min();
            long maxId = ids.Max();
            for (long i=minId+1; i<maxId; i++)
            {
                if (!ids.Contains(i) && ids.Contains(i - 1) && ids.Contains(i + 1)) return i;
            }
            return -1;
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<(int, int)> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, SeatToRowCol);
        }

        public static (int, int) SeatToRowCol(string seat)
        {
            string binary = seat;
            binary = binary
                .Replace('F', '0')
                .Replace('B', '1')
                .Replace('L', '0')
                .Replace('R', '1');
            int row = Convert.ToInt32(binary.Substring(0, 7), 2);
            int col = Convert.ToInt32(binary.Substring(7, 3), 2);
            return (row, col);
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}

// Treat B as 1, F as 0, convert to binary number
// Treat R as 1, L as 0, convert to binary number
// Convert.ToInt32("1000110", 2)
