using System;
using System.Collections.Generic;
using System.Linq;

namespace days
{
    public class Day03
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static Day03Result Part1()
        {
            const string path = Helpers.inputPath + @"\day03\input.txt";
            IList<IList<bool>> input = ProcessInputFile(path);

            IList<(int, int)> slopes = new List<(int, int)>
            {
                (3, 1)
            };

            IList<SlopeResult> slopeResults = slopes.Select(s => new SlopeResult
            {
                Dx = s.Item1,
                Dy = s.Item2,
                TreesHit = GetTreesHit(input, s.Item1, s.Item2)
            }).ToList();

            return new Day03Result
            {
                Answer = slopeResults.Aggregate((long) 1, (a, v) => a * v.TreesHit),
                Details = new Day03Details
                {
                    Height = input.Count,
                    Width = input[0].Count,
                    SlopeResults = slopeResults
                }
            };
        }
        
        public static Day03Result Part2()
        {
            const string path = Helpers.inputPath + @"\day03\input.txt";
            IList<IList<bool>> input = ProcessInputFile(path);

            IList<(int, int)> slopes = new List<(int, int)>
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            IList<SlopeResult> slopeResults = slopes.Select(s => new SlopeResult
            {
                Dx = s.Item1,
                Dy = s.Item2,
                TreesHit = GetTreesHit(input, s.Item1, s.Item2)
            }).ToList();

            return new Day03Result
            {
                Answer = slopeResults.Aggregate((long) 1, (a, v) => a * v.TreesHit),
                Details = new Day03Details
                {
                    Height = input.Count,
                    Width = input[0].Count,
                    SlopeResults = slopeResults
                }
            };
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<IList<bool>> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, GetTreeIndices);
        }

        public static IList<bool> GetTreeIndices(string line)
        {
            const char tree = '#';
            return line.ToCharArray().Select(c => (c == tree)).ToList();
        }

        public static long GetTreesHit(IList<IList<bool>> input, int dx, int dy)
        {
            int width = input[0].Count;
            int height = input.Count;

            int x = 0;
            int y = 0;
            long treesHit = 0;

            while (y < height)
            {
                if (input[y][x % width]) treesHit++;
                x += dx;
                y += dy;
            }

            return treesHit;
        }
    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    public class Day03Result : DayResult<long, Day03Details> { }

    public class Day03Details
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IList<SlopeResult> SlopeResults { get; set; }
    }

    public class SlopeResult
    {
        public int Dx { get; set; }
        public int Dy { get; set; }
        public long TreesHit { get; set;  }
    }

}
