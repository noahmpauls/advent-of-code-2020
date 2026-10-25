using days;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace days_tests
{
    [TestClass]
    public class Day01Tests
    {
        // test whether finds all index combos
        // test whether all combox are of correct size
        // test whether all combox add up to 2020
        // in the case where a target product is known, test whether target product is present in results

        [TestMethod]
        public void TestComboTwo()
        {
            IList<int> vals = new List<int> { 1721, 979, 366, 299, 675, 1456 };
            int comboSize = 2;

            int target = 2020;
            int comboCount = 1;
            IList<int> products = new List<int> { 514579 };

            IList<ISet<int>> combosIndices = Day01.FindComboIndices(target, comboSize, vals);
            IList<Combo> combos = combosIndices.Select(c => new Combo(c, vals)).ToList();

            TestCombos(combos, comboSize, target, comboCount, products);
        }

        [TestMethod]
        public void TestComboThree()
        {
            IList<int> vals = new List<int> { 1721, 979, 366, 299, 675, 1456 };
            int comboSize = 3;

            int target = 2020;
            int comboCount = 1;
            IList<int> products = new List<int> { 241861950 };

            IList<ISet<int>> combosIndices = Day01.FindComboIndices(target, comboSize, vals);
            IList<Combo> combos = combosIndices.Select(c => new Combo(c, vals)).ToList();

            TestCombos(combos, comboSize, target, comboCount, products);
        }

        [TestMethod]
        public void TestComboFour()
        {
            IList<int> vals = new List<int> { 100, 3, 8, 2, 7, 1 };
            int comboSize = 4;

            int target = 116;
            int comboCount = 1;
            IList<int> products = new List<int> { 5600 };

            IList<ISet<int>> combosIndices = Day01.FindComboIndices(target, comboSize, vals);
            IList<Combo> combos = combosIndices.Select(c => new Combo(c, vals)).ToList();

            TestCombos(combos, comboSize, target, comboCount, products);
        }

        [TestMethod]
        public void TestMultipleCombos()
        {
            IList<int> vals = new List<int> { 1, 7, 2, 6, 3, 5, 4, 4 };
            int comboSize = 2;

            int target = 8;
            int comboCount = 4;
            IList<int> products = new List<int> { 7, 12, 15, 16 };

            IList<ISet<int>> combosIndices = Day01.FindComboIndices(target, comboSize, vals);
            IList<Combo> combos = combosIndices.Select(c => new Combo(c, vals)).ToList();

            TestCombos(combos, comboSize, target, comboCount, products);
        }

        //######################################################################
        // Test Helpers
        //######################################################################

        private void TestCombos(IList<Combo> combos, int comboSize, int target, int comboCount, IList<int> products)
        {
            // check that count of combos is correct
            Assert.AreEqual(comboCount, combos.Count, "Combo count does not match.");

            IList<int> productsRemaining = new List<int>(products);
            // for each combo:
            foreach (Combo combo in combos)
            {
                // verify combo size
                Assert.AreEqual(comboSize, combo.Size, "Combo has incorrect size");
                // verify combo sum
                Assert.AreEqual(target, combo.Sum, "Combo does not sum to target.");
                // remove product from expected products
                int i = productsRemaining.IndexOf(combo.Product);
                if (i >= 0) productsRemaining.RemoveAt(i);
            }

            // verify all products were found
            Assert.AreEqual(0, productsRemaining.Count, "Not all expected products found.");
        }
    }
}
