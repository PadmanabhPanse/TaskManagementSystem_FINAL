using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public partial class TaskDescription
    {
        public long TaskId { get; set; }
        public long TaskDescriptionId { get; set; }
        [MaxLength(3800)]
        public string TaskDescLine1 { get; set; }
        [MaxLength(3800)]
        public string TaskDescLine2 { get; set; }
        [MaxLength(3800)]
        public string TaskDescLine3 { get; set; }
        [MaxLength(3800)]
        public string TaskDescLine4 { get; set; }
        public long? TaskQueryTypeId { get; set; }
        public long? TaskKeywordId { get; set; }
    }
}
