using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class SubscriptionManager:XmlFileHandler<List<Subscription>>
    {
        public SubscriptionManager(IHostingEnvironment environment) : base(environment, "ArrayOfSubscription", "subscription.xml")
        {

        }

        public void AddSubscription(Subscription newSubscription)
        {
            xmlObjects.Add(newSubscription);
        }

        public void EditSubscription(Subscription updatedSubscription)
        {
            int index = xmlObjects.FindIndex(m => m.Id == updatedSubscription.Id);
            Subscription[] subscriptionArr = xmlObjects.ToArray();
            subscriptionArr[index] = updatedSubscription;
            xmlObjects = subscriptionArr.ToList();
        }

        public void RemoveSubscription(Subscription subscription)
        {
            xmlObjects.Remove(subscription);
        }
    }
}
