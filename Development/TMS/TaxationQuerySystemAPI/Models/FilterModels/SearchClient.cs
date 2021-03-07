using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.FilterModels
{
    public class SearchClient
    {
        public string ClientCompanyName { get; set; }
        public string ClientContactPerson { get; set; }
        public DateTime ClientContactDate { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public DateTime? ClientSubscriptionStart { get; set; }
        public DateTime? ClientSubscriptionEnd { get; set; }
        public bool? ClientIsOrg { get; set; }
        public bool? ClientIsLock { get; set; }
    }
}
