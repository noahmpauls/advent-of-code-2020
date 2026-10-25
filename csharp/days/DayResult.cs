using System;
using System.Collections.Generic;
using System.Text;

namespace days
{
    public class DayResult<TAnswer, TDetails>
    {
        // usually a single number or string
        public TAnswer Answer { get; set; }
        // usually some collection of details about input parsing and evaluation
        public TDetails Details { get; set; }
    }
}
