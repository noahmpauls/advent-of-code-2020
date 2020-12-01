using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class DayOne
    {
        private static IList<int> ProcessInputFile()
        {
            string line;
            IList<int> nums = new List<int>();

            string path = @"C:\Users\noahm\OfflineProjects\advent-of-code-2020\backend\Assets\day-one\input.txt";

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                nums.Add(int.Parse(line));
            }

            file.Close();
            return nums;
        }
        public static int DoPartOne()
        {
            IList<int> nums = ProcessInputFile();
            ISet<int> comboIndices = findComboIndices(2020, nums, new HashSet<int>(), 2, 0);
            int result = comboIndices.Select(i => nums[i]).Aggregate(1, (acc, v) => acc * v);
            return result;
        }

        public static int DoPartTwo()
        {
            IList<int> nums = ProcessInputFile();
            ISet<int> comboIndices = findComboIndices(2020, nums, new HashSet<int>(), 3, 0);
            int result = comboIndices.Select(i => nums[i]).Aggregate(1, (acc, v) => acc * v);
            return result;
        }

        private static ISet<int> findComboIndices(int target, IList<int> vals, ISet<int> soFar, int comboSize = 2, int depth = 0)
        {
            if (depth >= comboSize)
            {
                if (soFar.Select(i => vals[i]).Sum() == target)
                {
                    return soFar;
                }
                else
                {
                    return new HashSet<int>();
                }
            }
            else
            {
                for (int i=0; i<vals.Count; i++)
                {
                    if (soFar.Contains(i)) continue;
                    ISet<int> newSoFar = new HashSet<int>(soFar);
                    newSoFar.Add(i);
                    ISet<int> result = findComboIndices(target, vals, newSoFar, comboSize, depth + 1);
                    if (result.Count > 0) return result;
                }
                return new HashSet<int>();
            }
        }
    }
}
