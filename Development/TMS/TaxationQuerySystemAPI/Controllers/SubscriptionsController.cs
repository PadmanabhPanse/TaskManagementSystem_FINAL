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
    public class SubscriptionsController : ControllerBase
    {
        public SubscriptionManager _subscriptionManager { get; }
        public SubscriptionsController(SubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        #region "Subscription"

        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetSubscription()
        {
            try
            {
                _subscriptionManager.LoadXml();
                return Ok(_subscriptionManager.xmlObjects.OrderByDescending(o => o.Id));
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Subscription> GetSubscription(long id)
        {

            try
            {
                _subscriptionManager.LoadXml();
                var Subscription = _subscriptionManager.xmlObjects.SingleOrDefault(m => m.Id == id);

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
        public IActionResult PutSubscription([FromRoute] long id, [FromBody] Subscription Subscription)
        {
            if (id != Subscription.Id)
            {
                return BadRequest();
            }
            try
            {
                _subscriptionManager.LoadXml();
                _subscriptionManager.EditSubscription(Subscription);
                _subscriptionManager.SaveXml();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    return NotFound(new { Error = ex.Message });
                }
                if (!SubscriptionExists(id))
                {
                    return NotFound(new { Error = $"Subscription with id {id} not found" });
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
        public ActionResult<Subscription> PostSubscription([FromBody] Subscription Subscription)
        {
            try
            {
                _subscriptionManager.LoadXml();
                _subscriptionManager.AddSubscription(Subscription);
                _subscriptionManager.SaveXml();

                return Ok(Subscription);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // DELETE: api/Subscription/5
        [HttpDelete("{id}")]
        public ActionResult<Subscription> DeleteSubscription([FromRoute] long id)
        {
            try
            {
                _subscriptionManager.LoadXml();
                var Subscription = _subscriptionManager.xmlObjects.FirstOrDefault(m => m.Id == id);
                if (Subscription == null)
                {
                    return NotFound(new { Error = $"Subscription with id {id} not found" });
                }

                _subscriptionManager.RemoveSubscription(Subscription);
                _subscriptionManager.SaveXml();

                return Ok(Subscription);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        private bool SubscriptionExists(long id)
        {
            return _subscriptionManager.xmlObjects.Any(e => e.Id == id);
        }
        #endregion
    }
}