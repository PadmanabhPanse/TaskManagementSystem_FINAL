using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskNotification
    {
        public long NotificationId { get; set; }
        public bool IsRead { get; set; }
        public bool IsPopup { get; set; }
        public long SettingId { get; set; }
        public string Description { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime PopupDate { get; set; }
        public DateTime SmsTime { get; set; }
        public DateTime EmailTime { get; set; }
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
        public long OwnerId { get; set; }
        public string UserId { get; set; }
        
    }
}
