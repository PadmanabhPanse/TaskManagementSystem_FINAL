using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ListSearchModels
{
    public class SearchNotificationSettingModel
    {
        public long TaskId { get; set; }
        public string Type { get; set; }
        public long ClinetId { get; set; }
        public long OwnerId { get; set; }
        public string ManagerId { get; set; }
        public string TaskChange { get; set; }
        public bool? Popup { get; set; }
    }
}
