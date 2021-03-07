using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskHistory
    {
        public long TaskHistoryTaskId { get; set; }
        public long? TaskHistoryOwnerId { get; set; }
        public DateTime TaskHistoryTaskAssignDate { get; set; }
        public long? TaskHistoryDocumentId { get; set; }
        public string TaskHistoryKeywords { get; set; }
        public string TaskHistoryComments { get; set; }
        public long TaskHistoryId { get; set; }

        public TaskOwner TaskHistoryOwner { get; set; }
        public Task TaskHistoryTask { get; set; }
    }
}
