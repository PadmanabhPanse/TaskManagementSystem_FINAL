using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskOwnersController : ControllerBase
    {
        private readonly TMSDBContext _context;

        public TaskOwnersController(TMSDBContext context)
        {
            _context = context;
        }

        // GET: api/TaskOwners
        [HttpPost("gettaskowners")]
        public async Task<IEnumerable<TaskOwner>> GetTaskOwner([FromBody] TaskOwnerSearchModel model)
        {
            IQueryable<Models.TaskOwner> owners = _context.TaskOwners;
            if (model != null)
            {
                var SearchFieldMutators = new List<SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>> {
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=> c.TaskOwnerJoinDate.HasValue && c.TaskOwnerJoinDate.Value.Date!=DateTime.MinValue.Date,(list,c)=>list.Where(item=>item.TaskOwnerJoinDate.Date==c.TaskOwnerJoinDate.Value.Date)),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=> c.TaskOwnerDateOfBirth.HasValue && c.TaskOwnerDateOfBirth.Value.Date != DateTime.MinValue.Date ,(list,c)=>list.Where(item=>item.TaskOwnerDateOfBirth.Date==c.TaskOwnerDateOfBirth.Value.Date)),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwnerName),(list,c)=>list.Where(item => item.TaskOwnerName.Contains(c.TaskOwnerName))),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwnerEmail),(list,c)=>list.Where(item => item.TaskOwnerEmail.Contains(c.TaskOwnerEmail))),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwnerPhoneNo),(list,c)=>list.Where(item => item.TaskOwnerPhoneNo.Contains(c.TaskOwnerPhoneNo))),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwnerAuthenticationModeFlag),(list,c)=>list.Where(item => item.TaskOwnerAuthenticationModeFlag.Contains(c.TaskOwnerAuthenticationModeFlag))),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwnerMacId),(list,c)=>list.Where(item => item.TaskOwnerMacId.Contains(c.TaskOwnerMacId))),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>!string.IsNullOrEmpty(c.UserId),(list,c)=>list.Where(item => string.Compare( item.UserId,c.UserId)==0)),
                   new SearchFieldMutator<Models.TaskOwner, TaskOwnerSearchModel>(c=>c.TaskOwnerIsLock,(list,c)=>list.Where(item => item.TaskOwnerIsLock==c.TaskOwnerIsLock)),
            };

                foreach (var filter in SearchFieldMutators)
                {
                    owners = filter.Apply(model, owners);
                }
            }
            return await owners.OrderByDescending(o => o.TaskOwnerId).ToListAsync();
        }

        // GET: api/TaskOwners/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskOwner([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskOwner = await _context.TaskOwners.FindAsync(id);

            if (taskOwner == null)
            {
                return NotFound();
            }

            return Ok(taskOwner);
        }

        // PUT: api/TaskOwners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskOwner([FromRoute] long id, [FromBody] TaskOwner taskOwner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskOwner.TaskOwnerId)
            {
                return BadRequest();
            }

            _context.Entry(taskOwner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskOwnerExists(id))
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

        // POST: api/TaskOwners
        [HttpPost]
        public async Task<IActionResult> PostTaskOwner([FromBody] TaskOwner taskOwner)
        {
            try { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TaskOwners.Add(taskOwner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskOwner", new { id = taskOwner.TaskOwnerId }, taskOwner);
            }
            catch(Exception ex)
            {
                throw ex;
            }    
        }

        // DELETE: api/TaskOwners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskOwner([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskOwner = await _context.TaskOwners.FindAsync(id);
            if (taskOwner == null)
            {
                return NotFound();
            }

            _context.TaskOwners.Remove(taskOwner);
            await _context.SaveChangesAsync();

            return Ok(taskOwner);
        }

        private bool TaskOwnerExists(long id)
        {
            return _context.TaskOwners.Any(e => e.TaskOwnerId == id);
        }
    }
}