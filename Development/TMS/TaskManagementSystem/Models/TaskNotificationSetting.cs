using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public partial class TaskNotificationSetting
    {
        public long SettingId { get; set; }
        public long TaskId { get; set; }
        public string Type { get; set; }
        public long ClientId { get; set; }
        public long OwnerId { get; set; }
        public string ManagerId { get; set; }
        public long DescriptionId { get; set; }
        public string TaskChange { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
        public bool Dashboard { get; set; }
        public bool Popup { get; set; }
    }
}
