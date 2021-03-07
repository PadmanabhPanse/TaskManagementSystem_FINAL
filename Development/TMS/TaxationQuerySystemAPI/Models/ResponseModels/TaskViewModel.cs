using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.ResponseModels
{
    public class TaskViewModel
    {
       
        [System.ComponentModel.DataAnnotations.Key]
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEstimateTime { get; set; }
        public DateTime? TaskThresholdDate { get; set; }
        public string Staff { get; set; }
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
        public List<DocumentViewModel> documents { get; set; }
        public TaskOwner TaskOwnerNavigation { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<TaskHistory> TaskHistory { get; set; }

        public List<TaskReviewCommentsViewModel> reviews { get; set; }
        public List<TaskStaffCommentsViewModel> comments { get; set; }
        public List<StaffIncentiveViewModel> incentives { get; set; }
        public string UserId { get; set; } 
        public string User { get; set; }
    }
}
