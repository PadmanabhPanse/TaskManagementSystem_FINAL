using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.FilterModels
{
    public class SearchSubscriber
    {
        public long SubscriptionId { get; set; }
        public string UserId { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? BalanceAmount { get; set; } 
        public decimal? ThresholdPrice { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool? IsLocked { get; set; }
    }
}
