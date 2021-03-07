using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaxTypesController : ControllerBase
    {
        public TaxTypeManager _taxTypeManager { get; }
        public TaxTypesController(TaxTypeManager taxTypeManager)
        {
            _taxTypeManager = taxTypeManager;
        }

        #region "Subscription"

        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetTaxTypes()
        {
            try
            {
                _taxTypeManager.LoadXml();
                return Ok(_taxTypeManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<TaxType> GetTaxType(long id)
        {

            try
            {
                _taxTypeManager.LoadXml();
                var Subscription = _taxTypeManager.xmlObjects.SingleOrDefault(m => m.Id == id);

                if (Subscription == null)
                {
                    return NotFound(new { Error = $"Subscription with id {id} not found" });
                }

                return Ok(Subscription);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // PUT: api/Subscription/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutTaxType([FromRoute] long id, [FromBody] TaxType taxType)
        {
            if (id != taxType.Id)
            {
                return BadRequest();
            }
            try
            {
                _taxTypeManager.LoadXml();
                _taxTypeManager.EditTaxType(taxType);
                _taxTypeManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!TaxTypeExists(id))
                {
                    return NotFound(new { Error = $"TaxType with id {id} not found" });
                }
                else
                {
                    return NotFound(new { Error = ex.Message });
                }
            }

            return Ok();
        }

        // POST: api/Subscription
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Subscription> PostTaxType([FromBody] TaxType taxType)
        {
            try
            {
                _taxTypeManager.LoadXml();
                _taxTypeManager.AddTaxType(taxType);
                _taxTypeManager.SaveXml();

                return Ok(taxType);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Subscription/5
        [HttpDelete("{id}")]
        public ActionResult<Subscription> DeleteTaxType([FromRoute] long id)
        {
            try
            {
                _taxTypeManager.LoadXml();
                var taxType = _taxTypeManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (taxType == null)
                {
                    return NotFound(new { Error = $"TaxType with id {id} not found" });
                }

                _taxTypeManager.RemoveTaxType(taxType);
                _taxTypeManager.SaveXml();

                return Ok(taxType);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool TaxTypeExists(long id)
        {
            return _taxTypeManager.xmlObjects.Any(e => e.Id == id);
        }
        #endregion
    }
}
