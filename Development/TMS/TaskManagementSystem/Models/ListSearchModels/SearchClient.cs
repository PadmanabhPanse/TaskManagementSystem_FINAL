using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
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
        //public long? ClientOrgId { get; set; }
        public bool? ClientIsLock { get; set; }
        //public long? ClinetInfoId { get; set; }
        //public long? ClientCreditId { get; set; }//
        //public long? ClientPaymentId { get; set; }//Payment Method used
        //public string ClientRemarkId { get; set; }
    }
}
