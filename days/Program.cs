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

            Console.WriteLine(Day09.Part1());
            Console.WriteLine(Day09.Part2());

            Day09.Test();

            Console.WriteLine("End");
        }
    }
}
