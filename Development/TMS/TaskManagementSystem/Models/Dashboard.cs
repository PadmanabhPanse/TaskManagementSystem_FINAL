using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class Dashboard
    {
        public int TotalTasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public int InprogressTasksCount { get; set; }
        public int OverDueTasksCount { get; set; }
        public int ClosedTasksCount { get; set; }

        public List<Tuple<string, int>> DaywiseCompletedTasks { get; set; }
        public List<Tuple<string, int>> DaywiseInprogressTasks { get; set; }
        public List<Tuple<string, int>> DaywiseOverDueTasks { get; set; }
        public List<Tuple<string, int>> DaywiseClosedTasks { get; set; }

        public List<Task> OverdueTasks { get; set; }
        public List<Task> InprogressTasks { get; set; }
        public List<Task> CompletedTasks { get; set; }

        public List<Task> ClosedTasks { get; set; }
        public List<Subscription> subscriptions { get; set; }
    }
}
