using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day01
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static Day01Result Part1()
        {
            const string path = Helpers.inputPath + @"\day01\input.txt";
            IList<int> inputs = ProcessInputFile(path);

            const int targetSum = 2020;
            const int comboSize = 2;
            IList<ISet<int>> comboIndices = FindComboIndices(targetSum, comboSize, inputs);
            IList<Combo> combos = comboIndices.Select(c => new Combo(c, inputs)).ToList();

            return new Day01Result
            {
                Answer = combos.Select(c => c.Product).ToList(),
                Details = new Day01Details
                {
                    Combos = combos,
                    ComboSize = comboSize,
                    TargetSum = targetSum
                }
            };
        }

        public static Day01Result Part2()
        {
            const string path = Helpers.inputPath + @"\day01\input.txt";
            IList<int> inputs = ProcessInputFile(path);

            const int targetSum = 2020;
            const int comboSize = 3;
            IList<ISet<int>> comboIndices = FindComboIndices(targetSum, comboSize, inputs);
            IList<Combo> combos = comboIndices.Select(c => new Combo(c, inputs)).ToList();

            return new Day01Result
            {
                Answer = combos.Select(c => c.Product).ToList(),
                Details = new Day01Details
                {
                    Combos = combos,
                    ComboSize = comboSize,
                    TargetSum = targetSum
                }
            };
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<int> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, line => int.Parse(line));
        }

        public static int ComboProduct(IList<int> vals, IEnumerable<int> indices)
        {
            return indices.Select(i => vals[i]).Aggregate(1, (acc, val) => acc * val);
        }

        public static IList<ISet<int>> FindComboIndices(int targetSum, int comboSize, IList<int> vals)
        {
            IList<ISet<int>> combos =
                FindComboIndicesRec(
                    targetSum,
                    comboSize,
                    vals,
                    new HashSet<int>(),
                    vals.Count
            );
            return combos;

        }

        public static IList<ISet<int>> FindComboIndicesRec(int targetSum, int comboSize, IList<int> vals, ISet<int> soFar, int indexBound)
        {
            if (soFar.Count >= comboSize)
            {
                IList<ISet<int>> newCombos = new List<ISet<int>>();
                if (soFar.Select(i => vals[i]).Sum() == targetSum)
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
                    newCombos = newCombos.Union(FindComboIndicesRec(targetSum, comboSize, vals, newSoFar, i)).ToList();
                }
                return newCombos;
            }
        }
    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    public class Day01Input
    {
        public string Text { get; set; }
        public int TargetSum { get; set; }
        public int ComboSize { get; set; }
    }

    public class Day01Result : DayResult<IList<int>, Day01Details> { }

    // information about a found combo
    public class Combo
    {
        // combo values ordered by index
        public IList<int> Vals { get; }
        // ordered list of combo indices
        public IList<int> Indices { get; }
        // size of combo
        public int Size
        { 
            get { return Indices.Count; }
        }
        // sum of combo
        public int Sum
        {
            get { return Vals.Sum(); }
        }
        // product of combo
        public int Product
        {
            get { return Vals.Aggregate(1, (a, v) => a * v); }
        }

        public Combo(IEnumerable<int> indices, IList<int> allVals)
        {
            Indices = indices.OrderBy(i => i).ToList();
            Vals = Indices.Select(i => allVals[i]).ToList();
        }
    }

    // JSON-able format for all result info
    public class Day01Details
    {
        public int ComboSize { get; set; }
        public int TargetSum { get; set; }
        public IList<Combo> Combos { get; set; }
    }
}
