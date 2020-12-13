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

            Console.WriteLine("Part 1 ----------------------------------------");
            Console.WriteLine(Day13.Part1());
            Console.WriteLine("");
            Console.WriteLine("Part 2 ----------------------------------------");
            Console.WriteLine(Day13.Part2());

            Console.WriteLine("End");
        }
    }
}
