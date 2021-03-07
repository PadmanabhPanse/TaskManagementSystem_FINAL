using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public class TaskSearchModel
    {
        public string UserId { get; set; }
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEstimateTime { get; set; } 
        public DateTime? TaskThresholdDate { get; set; }
        public string TaskOwner { get; set; }
        public long? TaskClientId { get; set; }
        public long? TaskOwnerId { get; set; }
        public long? TaskStaffId { get; set; }
        public string TaskStatusId { get; set; }
        public bool? TaskIsReopen { get; set; }
        public long? TaskDocumentId { get; set; }
        public string TaskKeywordId { get; set; }
        public long? TaskDescriptionId { get; set; }
        public long? TaskNotificationId { get; set; }
        public string TaskQueryTypeId { get; set; }
        public long? TaskPriority { get; set; }
      

    }
}
