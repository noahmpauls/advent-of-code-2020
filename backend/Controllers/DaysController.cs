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
        // GET: api/days/1/:part
        [HttpGet("1/{part?}")]
        public IActionResult GetDayOne(int? part)
        {
            if (part != null)
            {
                switch (part)
                {
                    case 1: 
                        return Ok(DayOne.DoPartOne());
                    case 2:
                        return Ok(DayOne.DoPartTwo());
                    default:
                        return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/days/2/:part
        [HttpGet("2/{part?}")]
        public IActionResult GetDayTwo(int? part)
        {
            if (part != null)
            {
                switch (part)
                {
                    case 1:
                        return Ok(DayTwo.DoPartOne());
                    case 2:
                        return Ok(DayTwo.DoPartTwo());
                    default:
                        return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
