using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("subscribers")]
    public class SubscribersController : Controller
    {
        readonly SubscriptionManager _subscriptionManager;
         readonly SubscriberManager _manager;
        readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public SubscribersController(SubscriberManager manager, SubscriptionManager subscriptionManager,  IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _subscriptionManager = subscriptionManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("getsubscribers")]
        public async Task<IActionResult> getsubscribers(SearchSubscriber model)
        {
            return Json(await _manager.GetSubscribers(model));
        }

        [Route("editsubscriber")]
        [HttpGet]
        public async Task<IActionResult> EditSubscriber(string id)
        {
            Subscriber subscriber = new Subscriber();
            if (!string.IsNullOrEmpty(id))
            {
                subscriber = await _manager.GetSubscriber(id);
                if (subscriber == null)
                {
                    return NotFound();
                }
            }
            return Json(subscriber);
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Subscriber model)
        {
            if (string.IsNullOrEmpty(model.SubscriberId))
            {
                model.SubscriberId = Guid.NewGuid().ToString();
                await _manager.PostSubscriber(model);
            }
            else
            {
                await _manager.PutSubscriber(model.SubscriberId, model);
            }
            return NoContent();
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            Subscriber subscriber = new Models.Subscriber();
            if (!string.IsNullOrEmpty(id))
            {
                subscriber = await _manager.GetSubscriber(id);
                if (subscriber == null)
                {
                    return NotFound();
                }
                await _manager.DeleteSubscriber(id);
            }
            return NoContent();
        }

        [Route("renew")]
        [HttpPost]
        public async Task<IActionResult> Renew(long SubscriptionId)
        {
            Subscription subscription = await _subscriptionManager.GetSubscription(SubscriptionId);
            Subscriber subscriber = new Subscriber
            {
                SubscriberId = Guid.NewGuid().ToString(),
                SubscriptionId = subscription.Id,
                UserId = Session.GetString("UserId"),
                SubscriptionStartDate = DateTime.Now,
                SubscriptionEndDate = DateTime.Now,
                IsLocked = false,
                TotalCost = subscription.TotalCost,
                BalanceAmount = subscription.TotalCost
            };
            await _manager.PostSubscriber(subscriber);
            Session.SetString("SubscriberId", subscriber.SubscriberId);
            return Redirect("/");
        }

    }
}
