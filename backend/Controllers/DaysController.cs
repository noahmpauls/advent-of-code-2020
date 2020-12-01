using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Helpers;

namespace backend.Controllers
{
    [ApiController]
    public class DaysController : ControllerBase
    {
        [Route("api/[controller]/{id?}")]
        public int Get(int? id)
        {
            if (id != null)
            {
                return DayOne.DoPartOne();
                //return (int)id;
            } else
            {
                return -1;
            }
        }
    }
}
