using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        readonly TMSDBContext _context;


        public CommonController(TMSDBContext context)
        {
            _context = context;
        }

        [HttpGet("gettaskowners")]
        public async Task<IEnumerable<Models.TaskOwner>> GetTaskOwners()
        {
            IQueryable<Models.TaskOwner> taskOwners = _context.TaskOwners;
            return await taskOwners.ToListAsync();
        }

        [HttpGet("getclients")]
        public async Task<IEnumerable<Models.Client>> GetClients()
        {
            IQueryable<Models.Client> clients = _context.Clients;
            return await clients.ToListAsync();
        }

        [HttpGet("gettasks")]
        public async Task<IEnumerable<Models.Task>> GetTasks()
        {
            IQueryable<Models.Task> tasks = _context.Tasks;
            return await tasks.ToListAsync();
        }

    }
}
