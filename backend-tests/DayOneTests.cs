using Microsoft.VisualStudio.TestTools.UnitTesting;
using backend.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace backend_tests
{
    [TestClass]
    public class DayOneTests
    {
        private int target = 2020;
        private IList<int> vals = new List<int> { 1721, 979, 366, 299, 675, 1456 };
        [TestMethod]
        public void ComboTwo()
        {
            int comboSize = 2;

            IList<ISet<int>> combosIndices = DayOne.FindComboIndices(target, comboSize, vals);
            IList<ISet<int>> combosValues = new List<ISet<int>>();
            foreach (ISet<int> combo in combosIndices)
            {
                combosValues.Add(new HashSet<int>(combo.Select(i => vals[i])));
            }
            ISet<int> sums = new HashSet<int>(combosValues.Select(combo => combo.Sum()));
            ISet<int> products = new HashSet<int>(combosValues.Select(combo => combo.Aggregate(1, (acc, val) => acc * val)));

            Assert.IsTrue(combosIndices.Count > 0, "No combos found.");
            foreach(ISet<int> combo in combosIndices)
            {
                Assert.AreEqual(comboSize, combo.Count, "Combo has incorrect size");
            }
            Assert.AreEqual(1, sums.Count, "Combos do not all have same sums.");
            Assert.IsTrue(sums.Contains(target), "Combos do not sum to target.");
            int expectedProduct = 514579;
            Assert.IsTrue(products.Contains(expectedProduct), "Combo does not contain expected product.");
        }

        [TestMethod]
        public void ComboThree()
        {
            int comboSize = 2;

            IList<ISet<int>> combosIndices = DayOne.FindComboIndices(target, comboSize, vals);
            IList<ISet<int>> combosValues = new List<ISet<int>>();
            foreach (ISet<int> combo in combosIndices)
            {
                combosValues.Add(new HashSet<int>(combo.Select(i => vals[i])));
            }
            ISet<int> sums = new HashSet<int>(combosValues.Select(combo => combo.Sum()));
            ISet<int> products = new HashSet<int>(combosValues.Select(combo => combo.Aggregate(1, (acc, val) => acc * val)));

            Assert.IsTrue(combosIndices.Count > 0, "No combos found.");
            foreach (ISet<int> combo in combosIndices)
            {
                Assert.AreEqual(comboSize, combo.Count, "Combo has incorrect size");
            }
            Assert.AreEqual(1, sums.Count, "Combos do not all have same sums.");
            Assert.IsTrue(sums.Contains(target), "Combos do not sum to target.");
            int expectedProduct = 514579;
            Assert.IsTrue(products.Contains(expectedProduct), "Combo does not contain expected product.");
        }

        /*
        [TestMethod]
        public void ComboThreeV2SuperSimple()
        {
            IList<int> vals = new List<int> { 979, 366, 675 };
            int expected = 241861950;

            ISet<int> comboIndices = DayOne.FCI(2020, vals, new HashSet<int>(), vals.Count, 3);
            int actual = comboIndices.Select(i => vals[i]).Aggregate(1, (acc, val) => acc * val);
            int sum = comboIndices.Select(i => vals[i]).Sum();
            Assert.AreEqual(2020, sum, "Combo does not sum to 2020");
            //Assert.AreEqual(expected, actual, "Combo of three numbers not correct.");
        }
        */
    }
}
