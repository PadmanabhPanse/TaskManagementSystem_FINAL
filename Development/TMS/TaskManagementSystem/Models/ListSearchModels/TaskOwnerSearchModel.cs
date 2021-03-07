using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public class TaskOwnerSearchModel
    {
        public string TaskOwnerName { get; set; }
        public string TaskOwnerPhoneNo { get; set; }
        public string TaskOwnerEmail { get; set; }
        public DateTime? TaskOwnerJoinDate { get; set; }
        public DateTime? TaskOwnerDateOfBirth { get; set; }
        public string TaskOwnerAuthenticationModeFlag { get; set; }
        public string TaskOwnerMacId { get; set; }
        public string UserId { get; set; }
        public bool TaskOwnerIsLock { get; set; }
    }
}
