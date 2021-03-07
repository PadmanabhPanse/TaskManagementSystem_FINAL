using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DashboardManager _manager;
        public HomeController(DashboardManager manager)
        {
            _manager = manager;
        }

        // GET: api/Tasks
        [HttpGet("dashboarddata")]
        public async Task<Models.Dashboard> DashboardData([FromQuery] TaskSearchModel model)
        {
            return await _manager.DashboardData(model);
        }
    }
}
