using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Helpers;
using days;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysController : ControllerBase
    {
        // GET: api/days/1/:part
        [HttpGet("1/{part?}")]
        public IActionResult GetDay01(int? part)
        {
            if (part != null)
            {
                switch (part)
                {
                    case 1: 
                        return Ok(Day01.Part1());
                    case 2:
                        return Ok(Day01.Part2());
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
        public IActionResult GetDay02(int? part)
        {
            if (part != null)
            {
                switch (part)
                {
                    case 1:
                        return Ok(Day02.Part1());
                    case 2:
                        return Ok(Day02.Part2());
                    default:
                        return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/days/3/:part
        [HttpGet("3/{part?}")]
        public IActionResult GetDay03(int? part)
        {
            if (part != null)
            {
                switch (part)
                {
                    case 1:
                        return Ok(Day03.Part1());
                    case 2:
                        return Ok(Day03.Part2());
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
