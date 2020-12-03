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

            var result1 = Day03.Part1();
            Console.WriteLine(result1.Answer);

            var result2 = Day03.Part2();
            Console.WriteLine(result2.Answer);

            Console.WriteLine("End");
        }
    }
}
