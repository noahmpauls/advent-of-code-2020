using System;
using System.Collections.Generic;
using System.Linq;

namespace days
{
    public class Day02
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static Day02Result Part1()
        {
            const string path = Helpers.inputPath + @"\day02\input.txt";
            IList<PasswordInput> inputs = ProcessInputFile(path);

            Func<PasswordInput, bool> evaluator = CharCountBounded;
            IList<bool> evaluations = EvaluatePasswords(inputs, evaluator);

            int validCount = evaluations.Count(e => e);
            IList<PasswordEval> passwordEvals = new List<PasswordEval>();
            for (int i=0; i < inputs.Count; i++)
            {
                passwordEvals.Add(new PasswordEval
                {
                    Input = inputs[i],
                    Valid = evaluations[i]
                });
            }

            return new Day02Result
            {
                Answer = validCount,
                Details = new Day02Details
                {
                    ValidCount = validCount,
                    RuleDescription = "The password must have at least num1 and at most num2 of the given character.",
                    Evaluations = passwordEvals
                }
            };
        }

        public static Day02Result Part2()
        {
            const string path = Helpers.inputPath + @"\day02\input.txt";
            IList<PasswordInput> inputs = ProcessInputFile(path);

            Func<PasswordInput, bool> evaluator = CharXORsIndices;
            IList<bool> evaluations = EvaluatePasswords(inputs, evaluator);

            int validCount = evaluations.Count(e => e);
            IList<PasswordEval> passwordEvals = new List<PasswordEval>();
            for (int i = 0; i < inputs.Count; i++)
            {
                passwordEvals.Add(new PasswordEval
                {
                    Input = inputs[i],
                    Valid = evaluations[i]
                });
            }

            return new Day02Result
            {
                Answer = validCount,
                Details = new Day02Details
                {
                    ValidCount = validCount,
                    RuleDescription = "The character must occur at exactly one of index num1 or num2 in the password.",
                    Evaluations = passwordEvals
                }
            };
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<PasswordInput> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, StringToPasswordInput);
        }

        public static PasswordInput StringToPasswordInput(string s)
        {
            char[] delimiters = { '-', ' ', ':', '\n' };
            string[] parts = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return new PasswordInput
            {
                Num1 = int.Parse(parts[0]),
                Num2 = int.Parse(parts[1]),
                Character = char.Parse(parts[2]),
                Password = parts[3]
            };
        }

        public static bool CharCountBounded(PasswordInput input)
        {
            int charOccurrences = input.Password.Count(c => c == input.Character);
            int min = input.Num1;
            int max = input.Num2;
            return min <= charOccurrences && charOccurrences <= max;
        }

        public static bool CharXORsIndices(PasswordInput input)
        {
            char c = input.Character;
            int i1 = input.Num1 - 1;
            int i2 = input.Num2 - 1;
            return input.Password[i1] == c ^ input.Password[i2] == c;
        }

        public static IList<bool> EvaluatePasswords(IList<PasswordInput> inputs, Func<PasswordInput, bool> evaluator)
        {
            return inputs.Select(p => evaluator(p)).ToList();
        }
    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    public class Day02Result : DayResult<int, Day02Details> { }

    public class PasswordInput
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public char Character { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PasswordInput input &&
                   Num1 == input.Num1 &&
                   Num2 == input.Num2 &&
                   Character == input.Character &&
                   Password == input.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Num1, Num2, Character, Password);
        }

        public override string ToString()
        {
            return $"({Num1}-{Num2} {Character}: {Password}";
        }
    }

    public class PasswordEval
    {
        public PasswordInput Input { get; set; }
        public bool Valid { get; set; }
    }

    public class Day02Details
    {
        public int ValidCount { get; set; }
        public string RuleDescription { get; set; }
        public IList<PasswordEval> Evaluations { get; set; }
    }
}
