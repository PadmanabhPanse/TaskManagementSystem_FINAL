using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class NotificationDesc
    {
        public long DescId { get; set; }
        public long NotificationId { get; set; }
        public string Description { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime SmsTime { get; set; }
        public DateTime EmailTime { get; set; }
    }
}
