using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;

namespace TaxationQuerySystemAPI.Services
{
    public class SubscriberManager
    {
        private readonly TMSDBContext _context;

        public SubscriberManager(TMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Subscriber>> GetSubscribers(SearchSubscriber model)
        {
            IQueryable<Subscriber> Subscribers = _context.Subscribers;

            var SearchFieldMutators = new List<SearchFieldMutator<Models.Subscriber, SearchSubscriber>>();

            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.SubscriptionId > 0, (list, c) => list.Where(o => o.SubscriptionId == c.SubscriptionId)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(o => string.Compare(o.UserId, c.UserId) == 0)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.TotalCost.HasValue && c.TotalCost.Value > 0, (list, c) => list.Where(o => o.TotalCost == c.TotalCost.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.BalanceAmount.HasValue && c.BalanceAmount.Value > 0, (list, c) => list.Where(o => o.BalanceAmount == c.BalanceAmount.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.ThresholdPrice.HasValue && c.ThresholdPrice.Value > 0, (list, c) => list.Where(o => o.ThresholdPrice == c.ThresholdPrice.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.IsLocked.HasValue, (list, c) => list.Where(o => o.IsLocked == c.IsLocked.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.SubscriptionStartDate.HasValue && c.SubscriptionStartDate != DateTime.MinValue, (list, c) => list.Where(o => o.SubscriptionStartDate.Date == c.SubscriptionStartDate.Value.Date)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.Subscriber, SearchSubscriber>(c => c.SubscriptionEndDate.HasValue && c.SubscriptionEndDate != DateTime.MinValue, (list, c) => list.Where(o => o.SubscriptionEndDate.Date == c.SubscriptionEndDate.Value.Date)));


            foreach (var item in SearchFieldMutators)
            {
                Subscribers = item.Apply(model, Subscribers);
            }


            return await Subscribers.OrderByDescending(o => o.SubscriptionStartDate).ToListAsync();
        }
        private async Task<Models.Subscriber> GetSubscriberByUserId(string UserId, bool? IsLocked)
        {
            try
            {
                if (IsLocked.HasValue)
                {
                    return await _context.Subscribers.OrderByDescending(o => o.SubscriptionStartDate).FirstOrDefaultAsync(s => String.Compare(s.UserId, UserId) == 0 && s.IsLocked == IsLocked);
                }
                else
                {
                    return await _context.Subscribers.OrderByDescending(o => o.SubscriptionStartDate).FirstOrDefaultAsync(s => String.Compare(s.UserId, UserId) == 0);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<Models.Subscriber> GetActiveSubscriberByUser(string UserId)
        {
            return await this.GetSubscriberByUserId(UserId, false);
        }
        public async Task<Models.Subscriber> GetLastSubscriberByUser(string UserId)
        {
            return await this.GetSubscriberByUserId(UserId, null);
        }
        public async Task<bool> IsSubscriber(string UserId)
        {
            return (await this.GetSubscriberByUserId(UserId, null)) != null;
        }
        public async Task<Models.Subscriber> GetSubscriber(string id)
        {
            try
            {
                var subscriber = await _context.Subscribers.FindAsync(id);
                if (subscriber == null)
                {
                    throw new Exception("Not Found");
                }
                return subscriber;
            }
            catch (Exception ex) { throw ex; }
        }

        // PUT: api/Tasks/5
        [HttpPut]
        public async Task<int> PutSubscriber(string id, Models.Subscriber subscriber)
        {
            if (string.Compare(subscriber.SubscriberId, id) != 0)
            {
                throw new Exception("Not Found");
            }
            try
            {
                _context.Entry(subscriber).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!SubscriberExists(id))
                {
                    throw new Exception("Not Found");
                }
                else
                {
                    throw ex;
                }
            }

        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<int> PostSubscriber(Models.Subscriber subscriber)
        {
            try
            {
                _context.Subscribers.Add(subscriber);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/Tasks/5
        [HttpDelete]
        public async Task<int> DeleteSubscriber(string id)
        {
            try
            {
                var subscriber = await _context.Subscribers.FindAsync(id);
                if (subscriber == null)
                {
                    throw new Exception("Not Found");
                }
                _context.Subscribers.Remove(subscriber);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SubscriberExists(string id)
        {
            return _context.Subscribers.Any(e => string.Compare(e.SubscriberId, id) == 0);
        }


    }
}
