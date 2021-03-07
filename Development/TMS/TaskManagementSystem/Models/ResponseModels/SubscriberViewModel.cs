using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ResponseModels
{
    public class SubscriberViewModel : Subscriber
    {
        public string User { get; set; }
        public string SubscriptionName { get; set; }
        public string TaxType { get; set; }
        public string QueryType { get; set; }
        public QueryRange QueryRange { get; set; }
        public decimal CostPerQuery { get; set; }
        public int Credits { get; set; }
        public decimal CostPerCredit { get; set; }
    }
}
