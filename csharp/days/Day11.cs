using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day11
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day11\input.txt";
            IList<IList<char>> input = ProcessInputFile(path);

            Seating<char> seating = new Seating<char>(input, '.', 'L', '#', 4);
            seating.StepShortToStasis();
            Console.WriteLine(seating.ToString());
            return seating.OccupiedCount();
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day11\input.txt";
            IList<IList<char>> input = ProcessInputFile(path);

            Seating<char> seating = new Seating<char>(input, '.', 'L', '#', 5);
            seating.StepLongToStasis();
            Console.WriteLine(seating.ToString());
            return seating.OccupiedCount();
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<IList<char>> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, ToCharList);
            throw new NotImplementedException();
        }

        public static IList<char> ToCharList(string line)
        {
            return line.ToCharArray().ToList();
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

    public class Seating<T>
    {
        private IList<IList<T>> chart;
        private T floor;
        private T empty;
        private T occupied;
        private int occupiedThreshold;

        public Seating(IList<IList<T>> chart, T floor, T empty, T occupied, int occupiedThreshold)
        {
            this.chart = chart;
            this.floor = floor;
            this.empty = empty;
            this.occupied = occupied;
            this.occupiedThreshold = occupiedThreshold;
        }

        public bool StepShort()
        {
            IList<IList<T>> nextRows = new List<IList<T>>();
            foreach (var row in chart)
            {
                nextRows.Add(new List<T>(row));
            }

            for (int row = 0; row < chart.Count; row++)
            {
                for (int col = 0; col < chart[0].Count; col++)
                {
                    if (chart[row][col].Equals(empty) && OccupiedAdjacentTo((col, row)) == 0)
                    {
                        nextRows[row][col] = occupied;
                    }
                    else if (chart[row][col].Equals(occupied) && OccupiedAdjacentTo((col, row)) >= occupiedThreshold)
                    {
                        nextRows[row][col] = empty;
                    }
                }
            }

            bool result = IsSameSeating(chart, nextRows);
            chart = nextRows;
            return result;
        }

        public void StepShortToStasis()
        {
            while (!StepShort()) ;
        }

        public bool StepLong()
        {
            IList<IList<T>> nextRows = new List<IList<T>>();
            foreach (var row in chart)
            {
                nextRows.Add(new List<T>(row));
            }

            for (int row = 0; row < chart.Count; row++)
            {
                for (int col = 0; col < chart[0].Count; col++)
                {
                    if (chart[row][col].Equals(empty) && OccupiedInSight((col, row)) == 0)
                    {
                        nextRows[row][col] = occupied;
                    }
                    else if (chart[row][col].Equals(occupied) && OccupiedInSight((col, row)) >= occupiedThreshold)
                    {
                        nextRows[row][col] = empty;
                    }
                }
            }

            bool result = IsSameSeating(chart, nextRows);
            chart = nextRows;
            return result;
        }

        public void StepLongToStasis()
        {
            while (!StepLong()) ;
        }

        public int OccupiedCount()
        {
            int occupiedCount = 0;

            for (int row = 0; row < chart.Count; row++)
            {
                for (int col = 0; col < chart[0].Count; col++)
                {
                    if (chart[row][col].Equals(occupied))
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount;
        }

        private int OccupiedAdjacentTo((int, int) seat)
        {
            int occupiedCount = 0;

            foreach (int dr in new int[] { -1, 0, 1 }) {
                foreach (int dc in new int[] { -1, 0, 1 }) {
                    if (dc == 0 && dr == 0) continue;
                    int row = seat.Item2 + dr;
                    int col = seat.Item1 + dc;
                    if (IsInbounds(row, col) && chart[row][col].Equals(occupied))
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount;
        }
        private int OccupiedInSight((int, int) seat)
        {
            int occupiedCount = 0;

            // iterate out in each direction until seat is found
            foreach (int dr in new int[] { -1, 0, 1 })
            {
                foreach (int dc in new int[] { -1, 0, 1 })
                {
                    if (dc == 0 && dr == 0) continue;
                    int row = seat.Item2 + dr;
                    int col = seat.Item1 + dc;
                    while(IsInbounds(row, col))
                    {
                        if (chart[row][col].Equals(empty)) break;
                        else if (chart[row][col].Equals(occupied))
                        {
                            occupiedCount++;
                            break;
                        }
                        row += dr;
                        col += dc;
                    }
                }
            }

            return occupiedCount;
        }

        private bool IsInbounds(int row, int col)
        {
            int rowCount = chart.Count;
            int colCount = chart[0].Count;

            return
                col >= 0 && col < colCount &&
                row >= 0 && row < rowCount;
        }

        private static bool IsSameSeating(IList<IList<T>> oldSeating, IList<IList<T>> newSeating)
        {
            int rowCount = oldSeating.Count;
            int colCount = oldSeating[0].Count;

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (!oldSeating[row][col].Equals(newSeating[row][col])) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string result = "";

            int rowCount = chart.Count;
            int colCount = chart[0].Count;

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result += chart[row][col];
                }
                result += '\n';
            }

            return result;
        }

    }

}

