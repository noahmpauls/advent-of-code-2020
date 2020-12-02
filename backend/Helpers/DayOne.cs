using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class DayOne
    {
        private static string path = @".\Assets\day-one\input.txt";
        private static IList<int> ProcessInputFile(string path)
        {
            string line;
            IList<int> nums = new List<int>();

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                nums.Add(int.Parse(line));
            }

            file.Close();
            return nums;
        }

        public static DayOneResult DoPartOne()
        {
            int target = 2020;
            int comboSize = 2;
            IList<int> nums = ProcessInputFile(path);
            return GetDayOneResult(target, comboSize, nums);
        }

        public static DayOneResult DoPartTwo()
        {
            int target = 2020;
            int comboSize = 3;
            IList<int> nums = ProcessInputFile(path);
            return GetDayOneResult(target, comboSize, nums);
        }

        public static DayOneResult GetDayOneResult(int target, int comboSize, IList<int> nums)
        {
            DayOneResult result = new DayOneResult();
            result.InputParams = new InputParams
            {
                TargetValue = target,
                ComboSize = comboSize
            };
            result.Combos = new List<Combo>();
            result.InputValues = nums;

            IList<ISet<int>> combosIndices = FindComboIndices(target, comboSize, nums);
            foreach (ISet<int> combo in combosIndices)
            {
                IList<int> indicesList = combo.ToList();
                IList<int> valuesList = indicesList.Select(i => nums[i]).ToList();
                int product = valuesList.Aggregate(1, (acc, val) => acc * val);
                result.Combos.Add(new Combo
                {
                    Indices = indicesList,
                    Values = valuesList,
                    Product = product
                });
            }

            return result;
        }

        public static IList<ISet<int>> FindComboIndices(int target, int comboSize, IList<int> vals)
        {
            IList<ISet<int>> combos = 
                FindComboIndicesRec(
                    target, 
                    comboSize, 
                    vals, 
                    new HashSet<int>(), 
                    vals.Count
            );

            return combos;

        }

        public static IList<ISet<int>> FindComboIndicesRec(int target, int comboSize, IList<int> vals, ISet<int> soFar, int indexBound)
        {
            if (soFar.Count >= comboSize)
            {
                IList<ISet<int>> newCombos = new List<ISet<int>>();
                if (soFar.Select(i => vals[i]).Sum() == target)
                {
                    newCombos.Add(new HashSet<int>(soFar));
                }
                return newCombos;
            }
            else
            {
                IList<ISet<int>> newCombos = new List<ISet<int>>();
                int startIndex = comboSize - soFar.Count - 1;
                for (int i = startIndex; i < indexBound; i++)
                {
                    ISet<int> newSoFar = new HashSet<int>(soFar);
                    newSoFar.Add(i);
                    newCombos = newCombos.Union(FindComboIndicesRec(target, comboSize, vals, newSoFar, i)).ToList();
                }
                return newCombos;
            }
        }
    }

    public class DayOneResult
    {
        public IList<int> InputValues { get; set; }
        public InputParams InputParams { get; set; }
        public IList<Combo> Combos { get; set; }
    }

    public class InputParams
    {
        public int ComboSize { get; set; }
        public int TargetValue { get; set; }
    }

    public class Combo
    {
        public IList<int> Indices { get; set; }
        public IList<int> Values { get; set; }
        public int Product { get; set; }
    }
}
