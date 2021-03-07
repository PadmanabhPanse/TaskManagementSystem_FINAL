using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskStaffComments
    {
        public long TaskCommentId { get; set; }
        public long? CommentTaskId { get; set; }
        public string TaskComment { get; set; }
        public DateTime? TaskCommentDate { get; set; }

        public Task StaffCommentsTask { get; set; }
    }
}
