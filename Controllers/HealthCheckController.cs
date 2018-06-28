using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace financial_api.Controllers
{
    [Route("api/healthchecks")]
    public class HealthCheckController : Controller
    {
        [HttpGet("ping")]
        public string Get(int id)
        {
            return "financial_api";
        }
    }
}
