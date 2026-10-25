using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day06
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day06\input.txt";
            string text = Helpers.GetFileAsString(path);

            IList<IList<char>> groupAnswers = ParseGroups(text).Select(g => GetUniqueChars(g)).ToList();
            return groupAnswers.Select(a => a.Count).Sum();
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day06\input.txt";
            string text = Helpers.GetFileAsString(path);

            IList<IList<char>> groupAnswers = ParseGroups(text).Select(g => GetCharsIntersect(g)).ToList();
            return groupAnswers.Select(a => a.Count).Sum();
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<char> GetCharsIntersect(string group)
        {
            Regex rx = new Regex("[a-z]+");
            IList<string> members = rx.Matches(group).Select(m => m.Value).ToList();
            IList<HashSet<char>> memberQuestions =
                members.Select(m => m.Distinct().ToHashSet()).ToList();
            List<char> intersect = memberQuestions.Aggregate((acc, q) => acc.Intersect(q).ToHashSet()).ToList();
            intersect.Sort();
            return intersect;
        }

        public static IList<char> GetUniqueChars(string group)
        {
            // TODO: use distinct method?

            ISet<char> chars = new HashSet<char>();
            foreach (char c in group)
            {
                if (!char.IsWhiteSpace(c))
                {
                    chars.Add(c);
                }
            }
            List<char> charList = chars.ToList();
            charList.Sort();
            return charList;
        }

        public static IList<string> ParseGroups(string text)
        {
            Regex rx = new Regex("([a-z]+\\n?)+");
            return rx.Matches(text).Select(m => m.Value).ToList();
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}