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
    public class ClientsController : ControllerBase
    {
        private readonly TMSDBContext _context;

        public ClientsController(TMSDBContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpPost("getclients")]
        public async Task<IEnumerable<Client>> GetClientMaster([FromBody] SearchClient model)
        {
            IQueryable<Models.Client> clients = _context.Clients;
            if (model != null)
            {
                var SearchFieldMutators = new List<SearchFieldMutator<Models.Client, SearchClient>> {  
                   new SearchFieldMutator<Models.Client, SearchClient>(c=> c.ClientContactDate!=null && c.ClientContactDate.Date!=DateTime.MinValue.Date,(list,c)=>list.Where(item=>item.ClientContactDate.Date==c.ClientContactDate.Date)),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=> c.ClientSubscriptionStart.HasValue && c.ClientSubscriptionStart.Value.Date!=DateTime.MinValue.Date,(list,c)=>list.Where(item=>item.ClientSubscriptionStart.Value.Date==c.ClientSubscriptionStart.Value.Date)),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=> c.ClientSubscriptionEnd.HasValue && c.ClientSubscriptionEnd.Value.Date != DateTime.MinValue.Date ,(list,c)=>list.Where(item=>item.ClientSubscriptionEnd.Value.Date==c.ClientSubscriptionEnd.Value.Date)),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=>!string.IsNullOrEmpty(c.ClientCompanyName),(list,c)=>list.Where(item => item.ClientCompanyName.Contains(c.ClientCompanyName))),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=>!string.IsNullOrEmpty(c.ClientEmail),(list,c)=>list.Where(item => item.ClientEmail.Contains(c.ClientEmail))),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=>!string.IsNullOrEmpty(c.ClientPhone),(list,c)=>list.Where(item => item.ClientPhone.Contains(c.ClientPhone))),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=>!string.IsNullOrEmpty(c.ClientContactPerson),(list,c)=>list.Where(item => item.ClientContactPerson.Contains(c.ClientContactPerson))),
                   new SearchFieldMutator<Models.Client, SearchClient>(c=>c.ClientIsLock.HasValue,(list,c)=>list.Where(item => item.ClientIsLock.Value==c.ClientIsLock.Value)),
            };

                foreach (var filter in SearchFieldMutators)
                {
                    clients = filter.Apply(model, clients);
                }
            }
            return await clients.OrderByDescending(o=>o.ClientId).ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient([FromRoute] long id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        private bool ClientExists(long id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}