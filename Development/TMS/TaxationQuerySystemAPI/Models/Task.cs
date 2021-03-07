using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class Task
    {
        public string UserId { get; set; }
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEstimateTime { get; set; }
        public DateTime? TaskThresholdDate { get; set; }
        public string TaskOwner { get; set; }
        public string TaskAdminInstructions { get; set; }
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
        public List<TaskDescription> descriptions { get; set; }
        public TaskOwner TaskOwnerNavigation { get; set; }
        public List<Document> documents { get; set; }
        public List<TaskHistory> TaskHistory { get; set; }
        public List<TaskReviewComments> reviews { get; set; }
        public List<TaskStaffComments> comments { get; set; }
        public List<StaffIncentive>  incentives { get; set; }
    }
}
