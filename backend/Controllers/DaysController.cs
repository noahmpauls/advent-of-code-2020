using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysController : ControllerBase
    {
        // GET: api/days/:day
        [HttpGet("{day?}/{part?}")]
        public int Get(int? day, int? part)
        {
            if (day != null && part != null)
            {
                switch (part)
                {
                    case 1: 
                        return DayOne.DoPartOne();
                    case 2:
                        return DayOne.DoPartTwo();
                    default:
                        return -1;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
