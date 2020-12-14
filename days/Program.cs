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
            Console.WriteLine(Day14.Part1());
            Console.WriteLine("");
            Console.WriteLine("Part 2 ----------------------------------------");
            Console.WriteLine(Day14.Part2());

            Console.WriteLine("");
            Console.WriteLine("Tests  ----------------------------------------");
            Day14.Mask mask = new Day14.Mask("X1X01X01X000110001011X010X0111X01001");
            Console.WriteLine(mask.Raw);
            Console.WriteLine(Convert.ToString((long)mask.Zeros, 2));
            Console.WriteLine(Convert.ToString((long)mask.Ones, 2));

            Console.WriteLine("End");
        }
    }
}
