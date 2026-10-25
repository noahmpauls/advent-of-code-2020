using System;
using days;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace days_tests
{
    [TestClass]
    public class Day02Tests
    {
        // test whether input parser is correct

        [TestMethod]
        public void TestParserSimple()
        {
            const string input = "1-3 a: abcde";
            PasswordInput expected = new PasswordInput
            {
                Num1 = 1,
                Num2 = 3,
                Character = 'a',
                Password = "abcde"
            };

            PasswordInput actual = Day02.StringToPasswordInput(input);

            Assert.AreEqual(expected, actual, "Parsed output does not match expected.");

        }

        [TestMethod]
        public void TestParserLarger()
        {
            const string input = "5000-5000000 b: 1234567890";
            PasswordInput expected = new PasswordInput
            {
                Num1 = 5000,
                Num2 = 5000000,
                Character = 'b',
                Password = "1234567890"
            };

            PasswordInput actual = Day02.StringToPasswordInput(input);

            Assert.AreEqual(expected, actual, "Parsed output does not match expected.");
        }

        /* Any negative inputs to num1 and num2 should be rejected.
         * 
        [TestMethod]
        public void TestParserNegatives()
        {
            const string input = "-1--45 b: b";
            PasswordInput expected = new PasswordInput
            {
                Num1 = -1,
                Num2 = -45,
                Character = 'b',
                Password = "b"
            };

            PasswordInput actual = Day02.StringToPasswordInput(input);

            Assert.AreEqual(expected, actual, "Parsed output does not match expected.");
        }
        */

        [TestMethod]
        public void TestParserSymbols()
        {
            const string input = "1-20 %: This :s poss!ble.";
            PasswordInput expected = new PasswordInput
            {
                Num1 = 1,
                Num2 = 20,
                Character = '%',
                Password = "This :is poss!ble."
            };

            PasswordInput actual = Day02.StringToPasswordInput(input);

            Assert.AreEqual(expected, actual, "Parsed output does not match expected.");
        }


        // test whether correct passwords are marked as valid/invalid

        [TestMethod]
        public void TestCountBoundRule()
        {
            IList<string> rawInputs = new List<string>
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };
            IList<PasswordInput> inputs = rawInputs.Select(s => Day02.StringToPasswordInput(s)).ToList();
            IList<bool> expected = new List<bool> { true, false, true };

            IList<bool> actual = Day02.EvaluatePasswords(inputs, Day02.CharCountBounded);

            Assert.IsTrue(expected.SequenceEqual(actual), "Password evaluation does not match expected.");
        }

        [TestMethod]
        public void TestIndexXORRule()
        {
            IList<string> rawInputs = new List<string>
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };
            IList<PasswordInput> inputs = rawInputs.Select(s => Day02.StringToPasswordInput(s)).ToList();
            IList<bool> expected = new List<bool> { true, false, false };

            IList<bool> actual = Day02.EvaluatePasswords(inputs, Day02.CharXORsIndices);

            Assert.IsTrue(expected.SequenceEqual(actual), "Password evaluation does not match expected.");
        }

        //######################################################################
        // Test Helpers
        //######################################################################

    }
}
