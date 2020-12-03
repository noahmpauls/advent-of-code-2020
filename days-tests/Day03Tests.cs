using System;
using days;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace days_tests
{
    [TestClass]
    public class Day03Tests {

        // test GetTreeIndices

        [TestMethod]
        public void TestParse()
        {
            string input = "#.##..###...";
            IList<bool> expected = new List<bool> { true, false, true, true, false, false, true, true, true, false, false, false };

            IList<bool> actual = Day03.GetTreeIndices(input);

            Assert.IsTrue(expected.SequenceEqual(actual), "Expected indices not found.");
        }

        // test GetTreesHit

        [TestMethod]
        public void TestTreesHit()
        {
            IList<string> rawInput = new List<string>
            {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#"
            };
            IList<IList<bool>> input = Helpers.ApplyTransformation(rawInput, Day03.GetTreeIndices);
            const int dx = 3;
            const int dy = 1;
            const long expected = 7;

            long actual = Day03.GetTreesHit(input, dx, dy);

            Assert.AreEqual(expected, actual, "Number of trees hit does not equal expected.");
        }

        //######################################################################
        // Test Helpers
        //######################################################################

    }
}