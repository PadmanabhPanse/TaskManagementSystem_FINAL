using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public partial class TaskStaffComments
    {
        public long TaskCommentId { get; set; }
        public long? CommentTaskId { get; set; }
        public string TaskComment { get; set; }
        public DateTime? TaskCommentDate { get; set; }
        public Microsoft.EntityFrameworkCore.EntityState ChangeState { get; set; }
    }
}
