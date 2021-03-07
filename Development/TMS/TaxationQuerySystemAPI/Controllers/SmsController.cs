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
    public class SmsController : ControllerBase
    {
        public SmsManager _SmsManager { get; }
        public SmsController(SmsManager SmsManager)
        {
            _SmsManager = SmsManager;
        }

        #region "Sms"

        [HttpGet]
        public ActionResult<IEnumerable<Sms>> GetSms()
        {
            try
            {
                _SmsManager.LoadXml();
                return Ok(_SmsManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Sms> GetSms(long id)
        {

            try
            {
                _SmsManager.LoadXml();
                var Sms = _SmsManager.xmlObjects.FirstOrDefault(m => m.Id == id);

                if (Sms == null)
                {
                    return NotFound(new { Error = $"Sms with id {id} not found" });
                }

                return Ok(Sms);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/Sms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutSms([FromRoute] long id, [FromBody] Sms Sms)
        {
            if (id != Sms.Id)
            {
                return BadRequest();
            }
            try
            {
                _SmsManager.LoadXml();
                _SmsManager.EditSms(Sms);
                _SmsManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!SmsExists(id))
                {
                    return NotFound(new { Error = $"Sms with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/Sms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Sms> PostSms([FromBody] Sms Sms)
        {
            try
            {
                _SmsManager.LoadXml();
                _SmsManager.AddSms(Sms);
                _SmsManager.SaveXml();

                return Ok(Sms);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Sms/5
        [HttpDelete("{id}")]
        public ActionResult<Sms> DeleteSms([FromRoute] long id)
        {
            try
            {
                _SmsManager.LoadXml();
                var Sms = _SmsManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (Sms == null)
                {
                    return NotFound(new { Error = $"Sms with id {id} not found" });
                }

                _SmsManager.RemoveSms(Sms);
                _SmsManager.SaveXml();

                return Ok(Sms);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool SmsExists(long id)
        {
            return _SmsManager.xmlObjects.Any(e => e.Id == id);
        }
        #endregion
    }
}
