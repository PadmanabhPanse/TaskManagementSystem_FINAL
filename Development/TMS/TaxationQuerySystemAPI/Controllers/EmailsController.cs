using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        public EmailManager _EmailManager { get; }
        public EmailsController(EmailManager EmailManager)
        {
            _EmailManager = EmailManager;
        }

        #region "Email"

        [HttpGet]
        public ActionResult<IEnumerable<Email>> GetEmail()
        {
            try
            {
                _EmailManager.LoadXml();
                return Ok(_EmailManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Email> GetEmail(long id)
        {

            try
            {
                _EmailManager.LoadXml();
                var Email = _EmailManager.xmlObjects.FirstOrDefault(m => m.Id == id);

                if (Email == null)
                {
                    return NotFound(new { Error = $"Email with id {id} not found" });
                }

                return Ok(Email);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/Email/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutEmail([FromRoute] long id, [FromBody] Email Email)
        {
            if (id != Email.Id)
            {
                return BadRequest();
            }
            try
            {
                _EmailManager.LoadXml();
                _EmailManager.EditEmail(Email);
                _EmailManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!EmailExists(id))
                {
                    return NotFound(new { Error = $"Email with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/Email
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Email> PostEmail([FromBody] Email Email)
        {
            try
            {
                _EmailManager.LoadXml();
                _EmailManager.AddEmail(Email);
                _EmailManager.SaveXml();

                return Ok(Email);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Email/5
        [HttpDelete("{id}")]
        public ActionResult<Email> DeleteEmail([FromRoute] long id)
        {
            try
            {
                _EmailManager.LoadXml();
                var Email = _EmailManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (Email == null)
                {
                    return NotFound(new { Error = $"Email with id {id} not found" });
                }

                _EmailManager.RemoveEmail(Email);
                _EmailManager.SaveXml();

                return Ok(Email);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool EmailExists(long id)
        {
            return _EmailManager.xmlObjects.Any(e => e.Id == id);
        }
        #endregion
    }
}
