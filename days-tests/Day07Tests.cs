using System;
using System.Collections.Generic;
using System.Text;

namespace days_tests
{
    class Day07Tests
    {
    }
}

/*
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
*/