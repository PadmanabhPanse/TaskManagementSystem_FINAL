using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskReviewComments
    {
        public long TaskReviewCommentId { get; set; }
        public long? ReviewTaskId { get; set; }
        public string TaskReviewComment { get; set; }
        public string TaskReviewActions { get; set; }
        public DateTime? TaskReviewDate { get; set; }
        public Task ReviewCommentsTask { get; set; }
    }
}
