using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Helpers
    {
        // local path to inputs
        public const string inputPath = @"C:\Users\noahm\OfflineProjects\advent-of-code-2020\inputs";

        // take an input file and transform it to a list of desired objects
        public static IList<T> ProcessInputFile<T>(string path, Func<string, T> transformer)
        {
            IList<string> lines = GetFileAsLines(path);
            return ApplyLineTransformation(lines, transformer);
        }

        // apply a transformation to a list of strings
        public static IList<T> ApplyLineTransformation<T>(IList<string> lines, Func<string, T> transformer)
        {
            return lines.Select(line => transformer(line)).ToList();
        }

        // take an input file and transform it to a list of strings
        public static IList<string> GetFileAsLines(string path)
        {
            string line;
            IList<string> lines = new List<string>();

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();
            return lines;
        }

        public static string GetFileAsString(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line = file.ReadToEnd();
            file.Close();
            return line;
        }
    }
}
