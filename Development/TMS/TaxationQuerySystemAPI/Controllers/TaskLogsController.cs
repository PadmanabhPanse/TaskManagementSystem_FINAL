using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskLogsController : ControllerBase
    {
        private readonly TMSDBContext _context;

        public TaskLogsController(TMSDBContext context)
        {
            _context = context;
        }

        // GET: api/TaskLogs
        [HttpGet("~/api/Tasks/{id}/Logs")]
        public IEnumerable<TaskHistory> GetTaskLogs([FromRoute] long id)
        {
            return _context.TaskLogs.Where(log => log.TaskHistoryTaskId == id).OrderByDescending(o => o.TaskHistoryId);
        }

        // GET: api/TaskLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskHistory = await _context.TaskLogs.FindAsync(id);

            if (taskHistory == null)
            {
                return NotFound();
            }

            return Ok(taskHistory);
        }

        // PUT: api/TaskLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskLog([FromRoute] long id, [FromBody] TaskHistory taskHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskHistory.TaskHistoryId)
            {
                return BadRequest();
            }

            _context.Entry(taskHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskLogs
        [HttpPost]
        public async Task<IActionResult> PostTaskLog([FromBody] TaskHistory taskHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TaskLogs.Add(taskHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskLog", new { id = taskHistory.TaskHistoryId }, taskHistory);
        }

        // DELETE: api/TaskLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskHistory = await _context.TaskLogs.FindAsync(id);
            if (taskHistory == null)
            {
                return NotFound();
            }

            _context.TaskLogs.Remove(taskHistory);
            await _context.SaveChangesAsync();

            return Ok(taskHistory);
        }

        private bool TaskHistoryExists(long id)
        {
            return _context.TaskLogs.Any(e => e.TaskHistoryId == id);
        }
    }
}