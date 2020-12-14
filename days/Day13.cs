using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day13
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day13\input.txt";
            IList<string> inputs = Helpers.GetFileAsLines(path);

            int arriveTime = int.Parse(inputs[0]);
            Regex rx = new Regex("[0-9]+");
            IList<int> ids = rx.Matches(inputs[1]).Select(m => int.Parse(m.Value)).ToList();

            int minId = -1;
            int minWait = -1;

            foreach(int id in ids)
            {
                int intervalWait = id - (arriveTime % id);
                if (intervalWait < minWait || minWait < 0)
                {
                    minWait = intervalWait;
                    minId = id;
                }
            }

            return minId * minWait;
        }

        public static long Part2()
        {
            const string path = Helpers.inputPath + @"\day13\input.txt";
            IList<string> inputs = Helpers.GetFileAsLines(path);

            Regex rx = new Regex("[0-9]+|x");
            IList<int> ids = rx.Matches(inputs[1]).Select(m => {
                if (m.Value.Equals("x"))
                    return 1;
                else
                    return int.Parse(m.Value);
            }).ToList();

            int largestIndex = ids.IndexOf(ids.Max());
            long t = -largestIndex;
            long multipleToAdd = ids[largestIndex];
            while (t <= 0)
                t += multipleToAdd;

            long tDebug = 0;

            IList<int> idIndices = ids
                .Select((id, index) => { if (id > 1) return index; else return -1; })
                .Where(i => i >= 0)
                .ToList();

            // whether this index is considered in the multipleToAdd variable
            IDictionary<int, bool> lockedIn = new Dictionary<int, bool>();
            foreach (int i in idIndices)
                lockedIn.Add(i, false);
            lockedIn[largestIndex] = true;
            while (true)
            {
                foreach (int i in idIndices)
                {
                    if ((t + i) % ids[i] == 0 && !lockedIn[i])
                    {
                        // MULTIPLY THE NUMBER TO ADD BY ids[i] ONCE ids[i] WORKS WITH THE CURRENT t VALUE
                        multipleToAdd *= ids[i];
                        lockedIn[i] = true;
                        if (lockedIn.Values.Aggregate(true, (acc, val) => acc & val))
                        {
                            return t;
                        }
                    }
                }
                t += multipleToAdd;
            }
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<string> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, line => line);
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}

//while(true)
//{
//    if (t - tDebug > 1000000000000)
//    {
//        Console.WriteLine($"  {t}");
//        tDebug = t;
//    }
//    bool tFound = true;
//    for (int i = 0; i < ids.Count; i++)
//    {
//        if ((t + i) % ids[i] != 0)
//        {
//            tFound = false;
//            break;
//        }
//    }
//    if (tFound)
//        return t;
//    else
//        t += ids[largestIndex];
//}