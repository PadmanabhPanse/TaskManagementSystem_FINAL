using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models.ResponseModels
{
    public class TaskNotificationViewModel : TaskNotification
    {
        public string Task { get; set; }
        public string Owner { get; set; }
        public string User { get; set; }
    }
}
