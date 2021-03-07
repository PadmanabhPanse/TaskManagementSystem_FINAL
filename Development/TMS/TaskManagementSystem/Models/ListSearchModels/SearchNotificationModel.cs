using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public class SearchNotificationModel
    {
        public bool? IsPopup { get; set; }
        public bool? IsRead { get; set; }
        public long SettingId { get; set; }
        public string Description { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime PopupDate { get; set; }
        public DateTime SmsTime { get; set; }
        public DateTime EmailTime { get; set; }
        public string UserId { get; set; }
    }
}
