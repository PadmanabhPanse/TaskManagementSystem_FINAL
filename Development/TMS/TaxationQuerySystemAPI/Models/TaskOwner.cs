using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskOwner
    {
        public TaskOwner()
        {
            Task = new HashSet<Task>();
            TaskHistory = new HashSet<TaskHistory>();
        }

        public long TaskOwnerId { get; set; }
        public string TaskOwnerName { get; set; }
        public string TaskOwnerAddress { get; set; }
        public string TaskOwnerPhoneNo { get; set; }
        public string TaskOwnerEmail { get; set; }
        public DateTime TaskOwnerJoinDate { get; set; }
        public DateTime TaskOwnerDateOfBirth { get; set; }
        public string TaskOwnerAuthenticationModeFlag { get; set; }
        public string TaskOwnerMacId { get; set; }
        public string UserId { get; set; }
        public bool TaskOwnerIsLock { get; set; }

        public ICollection<Task> Task { get; set; }
        public ICollection<TaskHistory> TaskHistory { get; set; }
    }
}
