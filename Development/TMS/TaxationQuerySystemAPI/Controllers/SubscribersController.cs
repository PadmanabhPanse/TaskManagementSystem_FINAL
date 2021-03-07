using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Models.ResponseModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        readonly TMSDBContext _context;
        readonly SubscriptionManager _subscriptionManager;
        readonly SubscriberManager _manager;
        readonly UserRoleManager _userRoleManager;
        readonly UserManager<ApplicationUser> _userManager;
        readonly NotificationManager _notifier;
        public SubscribersController(
            TMSDBContext context,
            SubscriberManager manager,
            UserManager<ApplicationUser> userManager,
            UserRoleManager userRoleManager,
            SubscriptionManager subscriptionManager,
            NotificationManager notifier
            )
        {
            _context = context;
            _manager = manager;
            _userManager = userManager;
            _userRoleManager = userRoleManager;
            _subscriptionManager = subscriptionManager;
            _notifier = notifier;
        }

        [HttpPost("getsubscribers")]
        public async Task<IEnumerable<Models.ResponseModels.SubscriberViewModel>> GetSubscribers([FromBody] SearchSubscriber model)
        {
            var subscribers = await _manager.GetSubscribers(model);

            List<SubscriberViewModel> subscriberViewModels = subscribers.Select(s => this.GetSubscriberVMObject(s)).ToList();
            return subscriberViewModels;
        }
        private SubscriberViewModel GetSubscriberVMObject(Subscriber subscriber)
        {
            if (subscriber != null)
            {
                ApplicationUser user = _userManager.FindByIdAsync(subscriber.UserId).Result;
                _subscriptionManager.LoadXml();
                var subscription = _subscriptionManager.xmlObjects.SingleOrDefault(m => m.Id == subscriber.SubscriptionId);
                return new SubscriberViewModel
                {
                    BalanceAmount = subscriber.BalanceAmount,
                    ThresholdPrice = subscriber.ThresholdPrice,
                    IsLocked = subscriber.IsLocked,
                    SubscriberId = subscriber.SubscriberId,
                    SubscriptionEndDate = subscriber.SubscriptionEndDate,
                    SubscriptionId = subscriber.SubscriptionId,
                    SubscriptionStartDate = subscriber.SubscriptionStartDate,
                    TotalCost = subscriber.TotalCost,
                    User = $"{user.FirstName} {user.LastName} ({user.UserName})",
                    UserId = user.Id,
                    CostPerCredit = subscription.CostPerCredit,
                    CostPerQuery = subscription.CostPerQuery,
                    Credits = subscription.Credits,
                    QueryRange = subscription.queryRange,
                    QueryType = subscription.querytype.ToString(),
                    SubscriptionName = subscription.Name,
                    TaxType = subscription.taxType.ToString()
                };
            }
            else
            {
                return null;
            }
        }
        [HttpGet("getactivesubscriberbyuser")]
        public async Task<SubscriberViewModel> GetActiveSubscriberByUser([FromQuery] string UserId)
        {
            return GetSubscriberVMObject(await _manager.GetActiveSubscriberByUser(UserId));
        }
        [HttpGet("getlastsubscriberbyuser")]
        public async Task<SubscriberViewModel> GetLastSubscriberByUser([FromQuery] string UserId)
        {
            return GetSubscriberVMObject(await _manager.GetLastSubscriberByUser(UserId));
        }
        [HttpGet("issubscriber")]
        public async Task<bool> IsSubscriber([FromQuery] string UserId)
        {
            return await _manager.IsSubscriber(UserId);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriber([FromRoute] string id)
        {
            try
            {
                return Ok(GetSubscriberVMObject(await _manager.GetSubscriber(id)));
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriber([FromRoute] string id, [FromBody] Models.Subscriber subscriber)
        {
            return Ok(await _manager.PutSubscriber(id, subscriber));
        }

        [HttpPost]
        public async Task<IActionResult> PostSubscriber([FromBody] Models.Subscriber subscriber)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _manager.PostSubscriber(subscriber);
                    User admin = _userRoleManager.getAdmin();
                    ApplicationUser user = await _userManager.FindByIdAsync(subscriber.UserId);

                    TaskNotificationSetting setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "NewSubscriberRegistration") == 0);
                    var newNotification = new TaskNotification
                    {
                        SettingId = setting.SettingId,
                        Description = setting.TaskChange.Replace("{Subscriber}", user.FirstName),
                        NotificationDate = DateTime.Now,
                        SmsTime = DateTime.Now,
                        EmailTime = DateTime.Now,
                        PopupDate = DateTime.Now,
                        IsRead = !setting.Dashboard,
                        ObjectId = subscriber.SubscriberId,
                        ObjectType = "Subscriber",
                        UserId = admin.UserId,
                        OwnerId = admin.OwnerId
                    };
                    await _notifier.PostNotification(newNotification);

                    await _notifier.SendNotification(
                        setting,
                        newNotification,
                        string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                        new Tuple<string, string>(user.UserName, user.Email),
                        new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) });
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriber([FromRoute] string id)
        {
            return Ok(await _manager.DeleteSubscriber(id));
        }
    }
}
