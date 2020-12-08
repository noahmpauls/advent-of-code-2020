using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace days
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            Console.WriteLine(Day07.Part1());
            Console.WriteLine(Day07.Part2());

            IList<(string, IList <(string, int)>)> input = new List<(string, IList<(string, int)>)>
            {
                ( "sg", new List<(string, int)> { ("do", 1), ("vp", 2) } ),
                ( "do", new List<(string, int)> { ("fb", 3), ("db", 4) } ),
                ( "vp", new List<(string, int)> { ("fb", 5), ("db", 6) } ),
                ( "db", new List<(string, int)>() ),
                ( "fb", new List<(string, int)>() )
            };

            BagTree bt = new BagTree(input);
            Console.WriteLine(bt.TotalBags("sg"));

            Console.WriteLine("End");
        }
    }
}
