using System;
using System.Collections.Generic;

namespace TaxationQuerySystemAPI.Models
{
    public partial class TaskNotificationSetting
    {
        public long SettingId { get; set; }
        public string Type { get; set; }
        public string TaskChange { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
        public bool Dashboard { get; set; }
        public bool Popup { get; set; }
    }
}
