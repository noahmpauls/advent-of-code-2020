using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class DayTwo
    {
        private static string path = @".\Assets\day-two\input.txt";
        private static IList<DayTwoInput> ProcessInputFile(string path)
        {
            string line;
            IList<string> lines = new List<string>();

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();
            IList<DayTwoInput> inputs = lines.Select(line =>
            {
                char[] delimiters = { '-', ' ', ':', '\n' };
                string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                return new DayTwoInput
                {
                    NumOne = int.Parse(parts[0]),
                    NumTwo = int.Parse(parts[1]),
                    Character = char.Parse(parts[2]),
                    Password = parts[3]
                };
            }).ToList();
            return inputs;
        }

        public static DayTwoResult DoPartOne()
        {
            IList<DayTwoInput> inputs = ProcessInputFile(path);

            Func<DayTwoInput, PasswordEval> charBoundedCounts = input =>
            {
                int charOccurrences = input.Password.Count(c => c == input.Character);
                int min = input.NumOne;
                int max = input.NumTwo;
                bool valid = min <= charOccurrences && charOccurrences <= max;;
                return new PasswordEval
                {
                    Input = input,
                    Valid = valid
                };
            };

            return GetDayTwoResult(inputs, charBoundedCounts);

        }

        public static DayTwoResult DoPartTwo()
        {
            IList<DayTwoInput> inputs = ProcessInputFile(path);

            Func<DayTwoInput, PasswordEval> charsAtIndices = input =>
            {
                int i1 = input.NumOne - 1;
                int i2 = input.NumTwo - 1;
                bool valid =
                    input.Password[i1] == input.Character ^
                    input.Password[i2] == input.Character;
                return new PasswordEval
                {
                    Input = input,
                    Valid = valid
                };
            };

            return GetDayTwoResult(inputs, charsAtIndices);

        }

        public static DayTwoResult GetDayTwoResult(IList<DayTwoInput> inputs, Func<DayTwoInput, PasswordEval> evaluator)
        {
            int validCount = 0;
            IList<PasswordEval> results = new List<PasswordEval>();

            foreach (DayTwoInput input in inputs)
            {
                PasswordEval eval = evaluator(input);
                if (eval.Valid) validCount++;
                results.Add(eval);
            }

            return new DayTwoResult
            {
                ValidCount = validCount,
                Results = results
            };
        }
    }

    public class DayTwoInput
    {
        public int NumOne { get; set; }
        public int NumTwo { get; set; }
        public char Character { get; set; }
        public string Password { get; set; }
    }

    public class DayTwoResult
    {
        public int ValidCount { get; set; }
        public IList<PasswordEval> Results { get; set; }
    }

    public class PasswordEval
    {
        public DayTwoInput Input { get; set; }
        public bool Valid { get; set; }
    }
}
