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
    public class TaskDescriptionsController : ControllerBase
    {
        private readonly TMSDBContext _context;

        public TaskDescriptionsController(TMSDBContext context)
        {
            _context = context;
        }

        // GET: api/TaskDescriptions
        [HttpGet("~/api/Tasks/{id}/Descriptions")]
        public IEnumerable<TaskDescription> GetTaskDesc([FromRoute] long id)
        {
            return _context.TaskDetails.Where(desc => desc.TaskId == id);
        }

        // GET: api/TaskDescriptions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskDescription([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskDescription = await _context.TaskDetails.FindAsync(id);

            if (taskDescription == null)
            {
                return NotFound();
            }

            return Ok(taskDescription);
        }

        // PUT: api/TaskDescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskDescription([FromRoute] long id, [FromBody] TaskDescription taskDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskDescription.TaskDescriptionId)
            {
                return BadRequest();
            }

            _context.Entry(taskDescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskDescriptionExists(id))
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

        // POST: api/TaskDescriptions
        [HttpPost]
        public async Task<IActionResult> PostTaskDescription([FromBody] TaskDescription taskDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TaskDetails.Add(taskDescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskDescription", new { id = taskDescription.TaskDescriptionId }, taskDescription);
        }

        // DELETE: api/TaskDescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskDescription([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskDescription = await _context.TaskDetails.FindAsync(id);
            if (taskDescription == null)
            {
                return NotFound();
            }

            _context.TaskDetails.Remove(taskDescription);
            await _context.SaveChangesAsync();

            return Ok(taskDescription);
        }

        private bool TaskDescriptionExists(long id)
        {
            return _context.TaskDetails.Any(e => e.TaskDescriptionId == id);
        }
    }
}