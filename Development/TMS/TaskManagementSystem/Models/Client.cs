using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public partial class Client
    {
        public long ClientId { get; set; }
        public string ClientCompanyName { get; set; }
        public string ClientContactPerson { get; set; }
        public DateTime ClientContactDate { get; set; }
        public string ClientLocation { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public DateTime? ClientSubscriptionStart { get; set; }
        public DateTime? ClientSubscriptionEnd { get; set; }
        public string ClientComment { get; set; }
        public bool? ClientIsOrg { get; set; }
        public long? ClientOrgId { get; set; }
        public bool? ClientIsLock { get; set; }
        public long? ClinetInfoId { get; set; }
        public long? ClientCreditId { get; set; }
        public long? ClientPaymentId { get; set; }
        public string ClientRemarkId { get; set; }
    }
}
