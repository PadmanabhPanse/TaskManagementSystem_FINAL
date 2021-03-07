using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Services
{
    public class TaskManager
    {
        private readonly TMSDBContext _context;

        public TaskManager(TMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetTasks(TaskSearchModel model)
        {
            IQueryable<Models.Task> tasks = _context.Tasks;
            var SearchFieldMutators = new List<SearchFieldMutator<Models.Task, TaskSearchModel>>();
            string[] ongoing = new string[] { "AcceptedByAdmin", "Assigned", "InProgress", "SentForReview", "AcceptedInReview", "RejectedInReview", "AcceptedByStaff", "RejectedByStaff" };
            string[] closed = new string[] { "AcceptedClosed", "Closed" };
            if (model != null)
            {
                if (string.Compare(model.TaskStatusId, "OngoingCount", true) == 0)
                {
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.TaskStatusId), (list, c) => list.Where(item => ongoing.Contains(item.TaskStatusId))));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStartDate.HasValue && c.TaskStartDate.Value.Date != DateTime.MinValue.Date, (list, c) => list.Where(item => item.TaskStartDate.Date <= c.TaskStartDate.Value.Date && item.TaskEstimateTime.Value.Date > c.TaskStartDate.Value.Date)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(item => item.UserId == c.UserId)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskOwnerId.HasValue && c.TaskOwnerId.Value > 0, (list, c) => list.Where(item => item.TaskOwnerId.Value == c.TaskOwnerId.Value)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStaffId.HasValue && c.TaskStaffId.Value > 0, (list, c) => list.Where(item => item.TaskStaffId.Value == c.TaskStaffId.Value)));
                }
                else if (string.Compare(model.TaskStatusId, "OverDueCount", true) == 0)
                {
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.TaskStatusId), (list, c) => list.Where(item => string.Compare(item.TaskStatusId, "Pending") == 0)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStartDate.HasValue && c.TaskStartDate.Value.Date != DateTime.MinValue.Date, (list, c) => list.Where(item => item.TaskStartDate.Date < c.TaskStartDate.Value.Date)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(item => item.UserId == c.UserId)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskOwnerId.HasValue && c.TaskOwnerId.Value > 0, (list, c) => list.Where(item => item.TaskOwnerId.Value == c.TaskOwnerId.Value)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStaffId.HasValue && c.TaskStaffId.Value > 0, (list, c) => list.Where(item => item.TaskStaffId.Value == c.TaskStaffId.Value)));
                }
                else if (string.Compare(model.TaskStatusId, "Closed", true) == 0)
                {
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.TaskStatusId), (list, c) => list.Where(item => closed.Contains(item.TaskStatusId))));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStartDate.HasValue && c.TaskStartDate.Value.Date != DateTime.MinValue.Date, (list, c) => list.Where(item => item.TaskStartDate.Date < c.TaskStartDate.Value.Date)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(item => item.UserId == c.UserId)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskOwnerId.HasValue && c.TaskOwnerId.Value > 0, (list, c) => list.Where(item => item.TaskOwnerId.Value == c.TaskOwnerId.Value)));
                    SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => c.TaskStaffId.HasValue && c.TaskStaffId.Value > 0, (list, c) => list.Where(item => item.TaskStaffId.Value == c.TaskStaffId.Value)));
                }
                else
                {
                    SearchFieldMutators.AddRange(new SearchFieldMutator<Models.Task, TaskSearchModel>[] {
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=> c.TaskStartDate.HasValue && c.TaskStartDate.Value.Date!=DateTime.MinValue.Date,(list,c)=>list.Where(item=>item.TaskStartDate.Date==c.TaskStartDate.Value.Date)),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=> c.TaskEstimateTime.HasValue && c.TaskEstimateTime.Value.Date != DateTime.MinValue.Date ,(list,c)=>list.Where(item=>item.TaskEstimateTime.Value.Date==c.TaskEstimateTime.Value.Date)),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=> c.TaskThresholdDate.HasValue && c.TaskThresholdDate.Value.Date != DateTime.MinValue.Date ,(list,c)=>list.Where(item=>item.TaskThresholdDate.Value.Date==c.TaskThresholdDate.Value.Date)),

                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>!string.IsNullOrEmpty(c.TaskName),(list,c)=>list.Where(item => item.TaskName.Contains(c.TaskName))),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>!string.IsNullOrEmpty(c.TaskOwner),(list,c)=>list.Where(item => item.TaskOwner.Contains(c.TaskOwner))),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>c.TaskIsReopen.HasValue,(list,c)=>list.Where(item => item.TaskIsReopen.Value==c.TaskIsReopen.Value)),

                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>c.TaskClientId.HasValue && c.TaskClientId.Value>0,(list,c)=>list.Where(item =>  item.TaskClientId.Value==c.TaskClientId.Value)),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>c.TaskOwnerId.HasValue && c.TaskOwnerId.Value>0,(list,c)=>list.Where(item =>  item.TaskOwnerId.Value==c.TaskOwnerId.Value)),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>c.TaskStaffId.HasValue && c.TaskStaffId.Value>0,(list,c)=>list.Where(item =>  item.TaskStaffId.Value==c.TaskStaffId.Value)),
                        new SearchFieldMutator<Models.Task, TaskSearchModel>(c=>!string.IsNullOrEmpty(c.UserId),(list,c)=>list.Where(item =>  item.UserId==c.UserId)),
                   });
                    if (model != null && !string.IsNullOrEmpty(model.TaskStatusId))
                    {
                        if (string.Compare(model.TaskStatusId, "ongoing", true) == 0)
                        {
                            SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.TaskStatusId), (list, c) => list.Where(item => ongoing.Contains(item.TaskStatusId))));
                        }
                        else
                        {
                            SearchFieldMutators.Add(new SearchFieldMutator<Models.Task, TaskSearchModel>(c => !string.IsNullOrEmpty(c.TaskStatusId), (list, c) => list.Where(item => string.Compare(item.TaskStatusId, c.TaskStatusId) == 0)));
                        }
                    }

                }
                foreach (var filter in SearchFieldMutators)
                {
                    tasks = filter.Apply(model, tasks);
                }
            }
            return await tasks.OrderByDescending(o => o.TaskId).ToListAsync();
        }
    }
}
