using System;
using System.ComponentModel.DataAnnotations;

namespace TaxationQuerySystemAPI.Models.ResponseModels
{
    public partial class TaskStaffCommentsViewModel
    {
        [Key]
        public long TaskCommentId { get; set; }
        public long? CommentTaskId { get; set; }
        public string TaskComment { get; set; }
        public DateTime? TaskCommentDate { get; set; }
        public Task StaffCommentsTask { get; set; }

        public Microsoft.EntityFrameworkCore.EntityState ChangeState { get; set; }
    }
}
