using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ResponseModels
{
    public class NotificationSettingResponse : TaskNotificationSetting
    {
        public string Task { get; set; }
        public string Client { get; set; }
        public string Owner { get; set; }
        public string Manager { get; set; }
    }
}
