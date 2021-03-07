using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;

namespace TaxationQuerySystemAPI.Services
{
    public class DashboardManager
    {
        private readonly TaskManager _context;

        public DashboardManager(TaskManager context)
        {
            _context = context;
        }
        public async Task<Dashboard> DashboardData(TaskSearchModel model)
        {
            Random random = new Random();
            Dashboard dashboard = new Dashboard();
            DateTime currentDate = DateTime.Now;

            DayOfWeek day = currentDate.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            DateTime startDate = currentDate.AddDays(-days);
            DateTime endDate = startDate;

            dashboard.TotalTasksCount = (await _context.GetTasks(new TaskSearchModel { UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).Count();
            dashboard.CompletedTasks = (await _context.GetTasks(new TaskSearchModel { TaskStatusId = "Done", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).ToList();
            dashboard.InprogressTasks = (await _context.GetTasks(new TaskSearchModel { TaskStatusId = "OngoingCount", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).ToList();
            dashboard.OverdueTasks = (await _context.GetTasks(new TaskSearchModel { TaskStatusId = "OverDueCount", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).ToList();
            dashboard.ClosedTasks = (await _context.GetTasks(new TaskSearchModel { TaskStatusId = "Closed", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).ToList();

            dashboard.CompletedTasksCount = dashboard.CompletedTasks.Count();
            dashboard.InprogressTasksCount = dashboard.InprogressTasks.Count();
            dashboard.OverDueTasksCount = dashboard.OverdueTasks.Count();
            dashboard.ClosedTasksCount = dashboard.ClosedTasks.Count();

            dashboard.DaywiseCompletedTasks = new List<Tuple<string, int>>();
            dashboard.DaywiseInprogressTasks = new List<Tuple<string, int>>();
            dashboard.DaywiseOverDueTasks = new List<Tuple<string, int>>();
            dashboard.DaywiseClosedTasks = new List<Tuple<string, int>>();

            for (int i = 0; i < 7; i++)
            {
                string firstletter = endDate.DayOfWeek.ToString().Substring(0, 1);
                dashboard.DaywiseCompletedTasks.Add(new Tuple<string, int>(firstletter, (await _context.GetTasks(new TaskSearchModel { TaskEstimateTime = endDate, TaskStatusId = "Done", UserId = model.UserId ,TaskOwnerId=model.TaskOwnerId,TaskStaffId=model.TaskStaffId })).Count()));
                dashboard.DaywiseInprogressTasks.Add(new Tuple<string, int>(firstletter, (await _context.GetTasks(new TaskSearchModel { TaskStartDate = endDate, TaskStatusId = "OngoingCount", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).Count()));
                dashboard.DaywiseOverDueTasks.Add(new Tuple<string, int>(firstletter, (await _context.GetTasks(new TaskSearchModel { TaskStartDate = endDate, TaskStatusId = "OverDueCount", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).Count()));
                dashboard.DaywiseClosedTasks.Add(new Tuple<string, int>(firstletter, (await _context.GetTasks(new TaskSearchModel { TaskEstimateTime = endDate, TaskStatusId = "Closed", UserId = model.UserId, TaskOwnerId = model.TaskOwnerId, TaskStaffId = model.TaskStaffId })).Count()));
                endDate = endDate.AddDays(1);
            }

            return dashboard;
        }
    }
}
