using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace days
{
    public class Day04
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day04\input.txt";
            string text = Helpers.GetFileAsString(path);

            IList<IDictionary<string, string>> passports = ParseInput(text);
            int validCount = passports.Sum(p =>
            {
                if (ValidateFieldsExist(p)) return 1;
                else return 0;
            });
            return validCount;
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day04\input.txt";
            string text = Helpers.GetFileAsString(path);

            IList<IDictionary<string, string>> passports = ParseInput(text);
            int validCount = passports.Sum(p =>
            {
                if (ValidateFieldsExist(p) && ValidateFieldsContent(p)) return 1;
                else return 0;
            });
            return validCount;
        }

        //######################################################################
        // Parsing
        //######################################################################

        public static IList<IDictionary<string, string>> ParseInput(string input)
        {

            // regex: (((\S+:\S+)[\n ])+)\n\n
            // key: \S+
            // value: \S+
            // key-value pair: key:value
            // passport: ((key:value)[\n ])+

            string rxKey = "\\S+";
            string rxValue = "\\S+";
            string rxPair = $"{rxKey}:{rxValue}";
            string rxPassport = $"(({rxPair})[\\n ])+";

            Regex rxp = new Regex(rxPassport);

            IList<string> passportsText = rxp.Matches(input).Select(m => m.Value).ToList();
            IList<IDictionary<string, string>> passports = passportsText.Select(p =>
            {
                Regex rxkv = new Regex($"(?<key>{rxKey}):(?<value>{rxValue})");
                MatchCollection matches = rxkv.Matches(p);
                IDictionary<string, string> pairs = new Dictionary<string, string>();
                foreach (Match m in matches)
                {
                    pairs.Add(m.Groups["key"].Value, m.Groups["value"].Value);
                }
                return pairs;
            }).ToList();

            return passports;
        }


        //######################################################################
        // Methods
        //######################################################################

        public static IList<IDictionary<string, string>> ProcessInputFile(string path)
        {
            // return Helpers.ProcessInputFile(path, /* transformer here */);
            throw new NotImplementedException();
        }

        public static void PrintPassport(IDictionary<string, string> passport)
        {
            Console.WriteLine("");
            foreach (string k in new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" })
            {
                if (passport.ContainsKey(k)) Console.WriteLine($"{k}: {passport[k]}");
                else Console.WriteLine($"{k}: -none-");
            }
        }

        public static int MakePassports(IList<string> lines)
        {
            int emptyCount = 0;
            foreach (string s in lines)
            {
                if (s.Equals("")) emptyCount++;
            }
            return emptyCount;
        }

        public static bool ValidateFieldsExist(IDictionary<string, string> passport)
        {
            return !((passport.Keys.Count < 8 && passport.ContainsKey("cid")) || passport.Keys.Count < 7);
        }

        public static bool ValidateFieldsContent(IDictionary<string, string> passport)
        {
            bool valid = true;

            if (passport.ContainsKey("byr"))
            {
                string byr = passport["byr"];
                Regex rxbyr = new Regex("^[0-9]{4}$");
                valid &= rxbyr.Matches(byr).Count > 0;
                int birthYear = int.Parse(byr);
                valid &= (birthYear >= 1920 && birthYear <= 2002);
            }

            if (passport.ContainsKey("iyr"))
            {
                string iyr = passport["iyr"];
                Regex rxiyr = new Regex("^[0-9]{4}$");
                valid &= rxiyr.Matches(iyr).Count > 0;
                int issueYear = int.Parse(iyr);
                valid &= (issueYear >= 2010 && issueYear <= 2020);
            }

            if (passport.ContainsKey("eyr"))
            {
                string eyr = passport["eyr"];
                Regex rxeyr = new Regex("^[0-9]{4}$");
                valid &= rxeyr.Matches(eyr).Count > 0;
                int expireYear = int.Parse(eyr);
                valid &= (expireYear >= 2020 && expireYear <= 2030);
            }

            if (passport.ContainsKey("hgt"))
            {
                string hgt = passport["hgt"];
                Regex rxhgt = new Regex("^(?<height>[0-9]+)(?<units>(cm)|(in))$");
                MatchCollection matches = rxhgt.Matches(hgt);
                if (matches.Count == 0)
                {
                    valid = false;
                } else
                {
                    Match match = matches[0];
                    int height = int.Parse(match.Groups["height"].Value);
                    if (match.Groups["units"].Value.Equals("in"))
                    {
                        valid &= (height >= 59 && height <= 76);
                    }
                    else if (match.Groups["units"].Value.Equals("cm"))
                    {
                        valid &= (height >= 150 && height <= 193);
                    }
                }
            }

            if (passport.ContainsKey("hcl"))
            {
                string hcl = passport["hcl"];
                Regex rxhcl = new Regex("^#[0-9a-f]{6}$");
                valid &= rxhcl.Matches(hcl).Count == 1;
            }

            if (passport.ContainsKey("ecl"))
            {
                string ecl = passport["ecl"];
                Regex rxecl = new Regex("^((amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth)){1}$");
                valid &= rxecl.Matches(ecl).Count == 1;
            }

            if (passport.ContainsKey("pid"))
            {
                string pid = passport["pid"];
                Regex rxpid = new Regex("^[0-9]{9}$");
                valid &= rxpid.Matches(pid).Count == 1;
            }

            return valid;
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}