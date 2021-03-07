using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Models.ResponseModels;
using TaxationQuerySystemAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        readonly string DocumentFolderPath = string.Empty;
        readonly IHostingEnvironment _environment;
        readonly TMSDBContext _context;
        readonly TaskManager _manager;
        readonly NotificationManager _notifier;
        readonly UserManager<ApplicationUser> _userManager;
        readonly UserRoleManager _userRoleManager;

        public TasksController(
            TMSDBContext context,
            TaskManager manager,
            IHostingEnvironment environment,
            NotificationManager notifier,
            UserManager<ApplicationUser> userManager,
            UserRoleManager userRoleManager
            )
        {
            _userRoleManager = userRoleManager;
            _userManager = userManager;
            _context = context;
            _manager = manager;
            _notifier = notifier;
            _environment = environment;
            DocumentFolderPath = Path.Combine(_environment.WebRootPath, "TaskDocuments");
            if (!Directory.Exists(DocumentFolderPath))
            {
                Directory.CreateDirectory(DocumentFolderPath);
            }
        }

        [HttpGet("geturl")]
        public string GetUrl()
        {
            return $"API URI:[{Request.Scheme}://{Request.Host}:{Request.Path}]";
        }
        // GET: api/Tasks
        [HttpPost("gettasks")]
        public async Task<IEnumerable<TaskViewModel>> GetTask([FromBody] TaskSearchModel model)
        {
            return (await _manager.GetTasks(model)).Select(q => GetTaskVMObject(q));
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            task.descriptions = _context.TaskDetails.Where(o => o.TaskId == id)?.ToList();
            task.reviews = _context.ReviewComments.Where(o => o.ReviewTaskId == id)?.ToList();
            task.comments = _context.StaffComments.Where(o => o.CommentTaskId == id)?.ToList();
            task.documents = _context.Documents.Where(o => o.DocumentTaskId == id)?.ToList();
            task.incentives = _context.StaffIncentives.Where(o => o.TaskId == id)?.ToList();
            return Ok(GetTaskVMObject(task));
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> PutTask([FromRoute] long id, [FromBody] Models.ResponseModels.TaskViewModel taskVM)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var currenttask = await _context.Tasks.FindAsync(id);
                    if (currenttask == null)
                    {
                        return NotFound();
                    }
                    _context.Entry(currenttask).State = EntityState.Detached;
                    EntityState taskDetailsentityState = _context.TaskDetails.Where(o => o.TaskId == id).Any() ? EntityState.Modified : EntityState.Added;

                    Models.Task task = this.GetTaskObject(taskVM);
                    if (id != task.TaskId)
                    {
                        return BadRequest();
                    }

                    _context.Entry(task).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    if (taskVM.descriptions != null && taskVM.descriptions.Count > 0)
                    {
                        foreach (var description in taskVM.descriptions)
                        {
                            TaskDescription taskDescription = description;
                            _context.Entry(taskDescription).State = taskDetailsentityState;
                        }
                        await _context.SaveChangesAsync();
                    }

                    if (taskVM.reviews != null && taskVM.reviews.Any(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached))
                    {
                        await this.UpdateReviews(taskVM.reviews.Where(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached).ToList(), task);
                    }
                    if (taskVM.comments != null && taskVM.comments.Any(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached))
                    {
                        await this.UpdateComments(taskVM.comments.Where(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached).ToList(), task);
                    }
                    if (taskVM.documents != null && taskVM.documents.Any(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached))
                    {
                        await this.UpdateDocuments(taskVM.documents.Where(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached).ToList(), task);
                    }
                    if (taskVM.incentives != null && taskVM.incentives.Any(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached))
                    {
                        await this.UpdateIncentives(taskVM.incentives.Where(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached).ToList(), task);
                    }
                    TaskNotificationSetting setting = null;
                    TaskNotification newNotification = null;
                    Tuple<string, string> sender = null;

                    Dictionary<string, string[]> rolesWithStatus = new Dictionary<string, string[]>
                    {
                        { "Admin", new string[] { "Pending", "Done" } },
                        { "TaskManager", new string[] { "AcceptedByAdmin", "SentForReview", "AcceptedByStaff", "RejectedByStaff" } },
                        { "Staff", new string[] { "Assigned", "RejectedInReview" } },
                        { "User", new string[] { "Waiting", "Closed", "RejectedByAdmin" } }
                    };

                    if (string.Compare(currenttask.TaskStatusId, taskVM.TaskStatusId) != 0)
                    {
                        setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "taskstatuschanged") == 0);
                        //Admin
                        string desc = setting.TaskChange.Replace("{TaskName}", taskVM.TaskName).Replace("{TaskStatusId}", taskVM.TaskStatusId);
                        if (rolesWithStatus["Admin"].Contains(task.TaskStatusId))
                        {
                            desc = $"Task {task.TaskName} is waiting for you action.";
                        }

                        User admin = _userRoleManager.getAdmin();
                        sender = new Tuple<string, string>(admin.UserName, admin.Email);
                        newNotification = new TaskNotification
                        {
                            SettingId = setting.SettingId,
                            Description = desc,
                            NotificationDate = DateTime.Now,
                            SmsTime = DateTime.Now,
                            EmailTime = DateTime.Now,
                            PopupDate = DateTime.Now,
                            IsRead = !setting.Dashboard,
                            ObjectId = task.TaskId.ToString(),
                            ObjectType = "Task",
                            UserId = admin.UserId,
                            OwnerId = task.TaskOwnerId.Value
                        };
                        await _notifier.PostNotification(newNotification);
                        await _notifier.SendNotification(
                             setting,
                             newNotification,
                             string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                             sender,
                             new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) }
                             );
                        //Owner
                        if (task.TaskOwnerId.HasValue && rolesWithStatus["TaskManager"].Contains(task.TaskStatusId))
                        {
                            desc = $"Task {task.TaskName} is assigned to you";
                            User Owner = _userRoleManager.getUsersByOwner("TaskManager", task.TaskOwnerId.Value);
                            if (Owner != null)
                            {
                                newNotification = new TaskNotification
                                {
                                    SettingId = setting.SettingId,
                                    Description = desc,
                                    NotificationDate = DateTime.Now,
                                    SmsTime = DateTime.Now,
                                    EmailTime = DateTime.Now,
                                    PopupDate = DateTime.Now,
                                    IsRead = !setting.Dashboard,
                                    ObjectId = task.TaskId.ToString(),
                                    ObjectType = "Task",
                                    UserId = Owner.UserId,
                                    OwnerId = task.TaskOwnerId.Value
                                };
                                await _notifier.PostNotification(newNotification);
                                await _notifier.SendNotification(
                             setting,
                             newNotification,
                             string.IsNullOrEmpty(Owner.PhoneNumber) ? null : new List<string> { Owner.PhoneNumber },
                             sender,
                             new List<Tuple<string, string>> { new Tuple<string, string>(Owner.UserName, Owner.Email) }
                             );
                            }
                        }
                        //Staff
                        if (task.TaskStaffId.HasValue && rolesWithStatus["Staff"].Contains(task.TaskStatusId))
                        {
                            desc = $"Task {task.TaskName} is assigned to you";
                            User Staff = _userRoleManager.getUsersByOwner("Staff", task.TaskStaffId.Value);
                            if (Staff != null)
                            {
                                newNotification = new TaskNotification
                                {
                                    SettingId = setting.SettingId,
                                    Description = desc,
                                    NotificationDate = DateTime.Now,
                                    SmsTime = DateTime.Now,
                                    EmailTime = DateTime.Now,
                                    PopupDate = DateTime.Now,
                                    IsRead = !setting.Dashboard,
                                    ObjectId = task.TaskId.ToString(),
                                    ObjectType = "Task",
                                    UserId = Staff.UserId,
                                    OwnerId = task.TaskOwnerId.Value
                                };
                                await _notifier.PostNotification(newNotification);
                                await _notifier.SendNotification(
                             setting,
                             newNotification,
                             string.IsNullOrEmpty(Staff.PhoneNumber) ? null : new List<string> { Staff.PhoneNumber },
                             sender,
                             new List<Tuple<string, string>> { new Tuple<string, string>(Staff.UserName, Staff.Email) }
                             );
                            }
                        }
                        //User
                        if (rolesWithStatus["User"].Contains(task.TaskStatusId))
                        {
                            desc = $"Task {task.TaskName} is waiting for you action.";
                        }
                        else
                        {
                            desc = setting.TaskChange.Replace("{TaskName}", taskVM.TaskName).Replace("{TaskStatusId}", taskVM.TaskStatusId);
                        }

                        ApplicationUser user = await _userManager.FindByIdAsync(task.UserId);
                        newNotification = new TaskNotification
                        {
                            SettingId = setting.SettingId,
                            Description = desc,
                            NotificationDate = DateTime.Now,
                            SmsTime = DateTime.Now,
                            EmailTime = DateTime.Now,
                            PopupDate = DateTime.Now,
                            IsRead = !setting.Dashboard,
                            ObjectId = task.TaskId.ToString(),
                            ObjectType = "Task",
                            UserId = task.UserId,
                            OwnerId = task.TaskOwnerId.Value
                        };
                        await _notifier.PostNotification(newNotification);
                        await _notifier.SendNotification(
                             setting,
                             newNotification,
                            string.IsNullOrEmpty(user.PhoneNumber) ? null : new List<string> { user.PhoneNumber },
                             sender,
                             new List<Tuple<string, string>> { new Tuple<string, string>(user.UserName, user.Email) }
                             );
                    }
                    else
                    {
                        setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "taskupdated") == 0);
                        newNotification = new TaskNotification
                        {
                            SettingId = setting.SettingId,
                            Description = setting.TaskChange,
                            NotificationDate = DateTime.Now,
                            SmsTime = DateTime.Now,
                            EmailTime = DateTime.Now,
                            PopupDate = DateTime.Now,
                            IsRead = !setting.Dashboard,
                            ObjectId = task.TaskId.ToString(),
                            ObjectType = "Task",
                            UserId = task.UserId,
                            OwnerId = task.TaskOwnerId.Value
                        };
                        await _notifier.PostNotification(newNotification);
                    }

                    transaction.Commit();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (!TaskExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

        }

        // POST: api/Tasks
        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> PostTask([FromBody] Models.ResponseModels.TaskViewModel taskVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Models.Task task = this.GetTaskObject(taskVM);
                    _context.Tasks.Add(task);
                    await _context.SaveChangesAsync();

                    if (taskVM.descriptions != null && taskVM.descriptions.Count > 0)
                    {
                        foreach (var description in taskVM.descriptions)
                        {
                            TaskDescription taskDescription = new TaskDescription
                            {
                                TaskId = task.TaskId,
                                TaskDescLine1 = description.TaskDescLine1,
                                TaskDescLine2 = description.TaskDescLine2,
                                TaskDescLine3 = description.TaskDescLine3,
                                TaskDescLine4 = description.TaskDescLine4,
                                TaskKeywordId = description.TaskKeywordId,
                                TaskQueryTypeId = description.TaskQueryTypeId
                            };
                            _context.Entry(taskDescription).State = EntityState.Added;
                        }
                        await _context.SaveChangesAsync();
                    }

                    if (taskVM.reviews != null && taskVM.reviews.Any(t => t.ChangeState == EntityState.Added))
                    {
                        await this.UpdateReviews(taskVM.reviews.Where(t => t.ChangeState == EntityState.Added).ToList(), task);
                    }
                    if (taskVM.comments != null && taskVM.comments.Any(t => t.ChangeState == EntityState.Added))
                    {
                        await this.UpdateComments(taskVM.comments.Where(t => t.ChangeState == EntityState.Added).ToList(), task);
                    }
                    if (taskVM.documents != null && taskVM.documents.Any(t => t.ChangeState == EntityState.Added))
                    {
                        await this.UpdateDocuments(taskVM.documents.Where(t => t.ChangeState == EntityState.Added).ToList(), task);
                    }
                    if (taskVM.incentives != null && taskVM.incentives.Any(t => t.ChangeState == EntityState.Added))
                    {
                        await this.UpdateIncentives(taskVM.incentives.Where(t => t.ChangeState == EntityState.Added).ToList(), task);
                    }
                    TaskNotificationSetting setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "taskadded") == 0);
                    User admin = _userRoleManager.getAdmin();
                    var newNotification = new TaskNotification
                    {
                        SettingId = setting.SettingId,
                        Description = setting.TaskChange.Replace("{TaskName}", taskVM.TaskName),
                        NotificationDate = DateTime.Now,
                        SmsTime = DateTime.Now,
                        EmailTime = DateTime.Now,
                        PopupDate = DateTime.Now,
                        IsRead = !setting.Dashboard,
                        ObjectId = task.TaskId.ToString(),
                        ObjectType = "Task",
                        UserId = admin.UserId,
                        OwnerId = task.TaskOwnerId.Value
                    };
                    await _notifier.PostNotification(newNotification);

                    ApplicationUser user = await _userManager.FindByIdAsync(task.UserId);

                    newNotification = new TaskNotification
                    {
                        SettingId = setting.SettingId,
                        Description = setting.TaskChange.Replace("{TaskName}", taskVM.TaskName),
                        NotificationDate = DateTime.Now,
                        SmsTime = DateTime.Now,
                        EmailTime = DateTime.Now,
                        PopupDate = DateTime.Now,
                        IsRead = !setting.Dashboard,
                        ObjectId = task.TaskId.ToString(),
                        ObjectType = "Task",
                        UserId = task.UserId,
                        OwnerId = task.TaskOwnerId.Value
                    };
                    await _notifier.PostNotification(newNotification);
                    await _notifier.SendNotification(
                        setting,
                        newNotification,
                        string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                        new Tuple<string, string>(user.UserName, user.Email),
                        new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) });

                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var task = await _context.Tasks.FindAsync(id);
                    if (task == null)
                    {
                        return NotFound();
                    }
                    string TaskFolder = Path.Combine(DocumentFolderPath, id.ToString());
                    if (Directory.Exists(TaskFolder))
                    {
                        Directory.Delete(TaskFolder, true);
                    }
                    task.comments = _context.StaffComments.Where(d => d.CommentTaskId == id).ToList();
                    if (task.comments != null && task.comments.Any())
                    {
                        foreach (var comment in task.comments)
                        {
                            TaskStaffComments staffComments = comment;
                            _context.StaffComments.Remove(staffComments);
                        }
                    }

                    task.reviews = _context.ReviewComments.Where(d => d.ReviewTaskId == id).ToList();
                    if (task.reviews != null && task.reviews.Any())
                    {
                        foreach (var review in task.reviews)
                        {
                            TaskReviewComments reviewComments = review;
                            _context.ReviewComments.Remove(reviewComments);
                        }
                    }

                    task.descriptions = _context.TaskDetails.Where(d => d.TaskId == id).ToList();
                    foreach (TaskDescription description in task.descriptions)
                    {
                        TaskDescription taskDescription = description;
                        _context.TaskDetails.Remove(taskDescription);
                    }
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return Ok(task);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        [NonAction]
        private bool TaskExists(long id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
        [NonAction]
        private Models.Task GetTaskObject(TaskViewModel task)
        {
            return new Models.Task
            {
                TaskClientId = task.TaskClientId,
                descriptions = null,
                TaskDescriptionId = task.TaskDescriptionId,
                TaskDocumentId = task.TaskDocumentId,
                TaskEstimateTime = task.TaskEstimateTime,
                TaskId = task.TaskId,
                TaskHistory = null,
                TaskIsReopen = task.TaskIsReopen,
                TaskKeywordId = task.TaskKeywordId,
                TaskName = task.TaskName,
                TaskNotificationId = task.TaskNotificationId,
                TaskOwner = task.TaskOwner,
                TaskAdminInstructions = task.TaskAdminInstructions,
                TaskOwnerId = task.TaskOwnerId,
                TaskOwnerNavigation = null,
                TaskPriority = task.TaskPriority,
                TaskQueryTypeId = task.TaskQueryTypeId,
                TaskStaffId = task.TaskStaffId,
                TaskStartDate = task.TaskStartDate,
                TaskStatusId = task.TaskStatusId,
                TaskThresholdDate = task.TaskThresholdDate,
                TaskTitle = task.TaskTitle,
                UserId = task.UserId,
                comments = null,
                reviews = null,
                documents = null,
                incentives = null
            };
        }
        [NonAction]
        private Models.ResponseModels.TaskViewModel GetTaskVMObject(Models.Task task)
        {
            ApplicationUser user = _userManager.FindByIdAsync(task.UserId).Result;
            TaskOwner Staff = null;
            if (task.TaskStaffId.HasValue)
            {
                Staff = _context.TaskOwners.SingleOrDefault(s => s.TaskOwnerId == task.TaskStaffId.Value);
            }
            return new Models.ResponseModels.TaskViewModel
            {
                Staff = Staff?.TaskOwnerName ?? "TBD",
                TaskClientId = task.TaskClientId,
                descriptions = task.descriptions,
                TaskDescriptionId = task.TaskDescriptionId,
                TaskDocumentId = task.TaskDocumentId,
                TaskEstimateTime = task.TaskEstimateTime,
                TaskId = task.TaskId,
                TaskHistory = null,
                TaskIsReopen = task.TaskIsReopen,
                TaskKeywordId = task.TaskKeywordId,
                TaskName = task.TaskName,
                TaskNotificationId = task.TaskNotificationId,
                TaskOwner = task.TaskOwner,
                TaskAdminInstructions = task.TaskAdminInstructions,
                TaskOwnerId = task.TaskOwnerId,
                TaskPriority = task.TaskPriority,
                TaskQueryTypeId = task.TaskQueryTypeId,
                TaskStaffId = task.TaskStaffId,
                TaskStartDate = task.TaskStartDate,
                TaskStatusId = task.TaskStatusId,
                TaskThresholdDate = task.TaskThresholdDate,
                TaskTitle = task.TaskTitle,
                UserId = task.UserId,
                User = $"{user.FirstName} {user.LastName} ({user.UserName})",
                comments = task.comments != null && task.comments.Count() > 0 ? this.getCommentVMs(task.comments.ToList(), task.TaskId, EntityState.Unchanged) : null,
                reviews = task.reviews != null && task.reviews.Count() > 0 ? this.getReviewVMs(task.reviews.ToList(), task.TaskId, EntityState.Unchanged) : null,
                documents = task.documents != null && task.documents.Count() > 0 ? this.getDocumentVMs(task.documents.ToList(), task.TaskId, EntityState.Unchanged) : null,
                incentives = task.incentives != null && task.incentives.Count() > 0 ? this.getIncentiveVMs(task.incentives.ToList(), task.TaskId, EntityState.Unchanged) : null,
                TaskOwnerNavigation = null
            };
        }
        [NonAction]
        private List<TaskReviewCommentsViewModel> getReviewVMs(List<TaskReviewComments> reviews, long id, EntityState entityState)
        {
            if (reviews != null && reviews.Any())
            {
                List<TaskReviewCommentsViewModel> reviewVMs = new List<TaskReviewCommentsViewModel>();
                foreach (var review in reviews)
                {
                    TaskReviewCommentsViewModel taskReview = new TaskReviewCommentsViewModel
                    {
                        ChangeState = entityState,
                        ReviewTaskId = id,
                        TaskReviewActions = review.TaskReviewActions,
                        TaskReviewComment = review.TaskReviewComment,
                        TaskReviewCommentId = review.TaskReviewCommentId,
                        TaskReviewDate = review.TaskReviewDate
                    };
                    reviewVMs.Add(taskReview);
                }

                return reviewVMs;
            }
            return new List<TaskReviewCommentsViewModel>();
        }
        [NonAction]
        private async Task<IActionResult> UpdateReviews(List<Models.ResponseModels.TaskReviewCommentsViewModel> taskReviews, Models.Task task)
        {
            try
            {
                if (taskReviews != null && taskReviews.Any())
                {
                    foreach (var taskReviewCommentViewModel in taskReviews.Where(list => list.ChangeState != EntityState.Unchanged || list.ChangeState != EntityState.Detached))
                    {
                        TaskReviewComments taskReviewComment = new TaskReviewComments
                        {
                            TaskReviewCommentId = taskReviewCommentViewModel.TaskReviewCommentId,
                            ReviewTaskId = task.TaskId,
                            TaskReviewComment = taskReviewCommentViewModel.TaskReviewComment,
                            TaskReviewActions = taskReviewCommentViewModel.TaskReviewActions,
                            TaskReviewDate = taskReviewCommentViewModel.TaskReviewDate,
                            ReviewCommentsTask = task,
                        };
                        _context.Entry(taskReviewComment).State = taskReviewCommentViewModel.ChangeState;
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        private List<TaskStaffCommentsViewModel> getCommentVMs(List<TaskStaffComments> comments, long id, EntityState entityState)
        {
            if (comments != null && comments.Any())
            {
                List<TaskStaffCommentsViewModel> commentVMs = new List<TaskStaffCommentsViewModel>();
                foreach (var comment in comments)
                {
                    TaskStaffCommentsViewModel taskReview = new TaskStaffCommentsViewModel
                    {
                        ChangeState = entityState,
                        CommentTaskId = id,
                        TaskComment = comment.TaskComment,
                        TaskCommentId = comment.TaskCommentId,
                        TaskCommentDate = comment.TaskCommentDate
                    };
                    commentVMs.Add(taskReview);
                }

                return commentVMs;
            }
            return new List<TaskStaffCommentsViewModel>();
        }
        [NonAction]
        private async Task<IActionResult> UpdateComments(List<Models.ResponseModels.TaskStaffCommentsViewModel> taskComments, Models.Task task)
        {
            try
            {
                if (taskComments != null && taskComments.Any())
                {
                    foreach (var taskStaffCommentViewModel in taskComments.Where(docs => docs.ChangeState != EntityState.Unchanged || docs.ChangeState != EntityState.Detached))
                    {
                        TaskStaffComments taskStaffComments = new TaskStaffComments
                        {
                            TaskCommentId = taskStaffCommentViewModel.TaskCommentId,
                            CommentTaskId = task.TaskId,
                            TaskComment = taskStaffCommentViewModel.TaskComment,
                            TaskCommentDate = taskStaffCommentViewModel.TaskCommentDate,
                            StaffCommentsTask = task,
                        };
                        _context.Entry(taskStaffComments).State = taskStaffCommentViewModel.ChangeState;
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        private List<DocumentViewModel> getDocumentVMs(List<Document> documents, long id, EntityState entityState)
        {
            if (documents != null && documents.Any())
            {
                List<DocumentViewModel> documentVMs = new List<DocumentViewModel>();
                foreach (var document in documents)
                {
                    DocumentViewModel taskDocument = new DocumentViewModel
                    {
                        DocumentComments = document.DocumentComments,
                        DocumentDate = document.DocumentDate,
                        DocumentId = document.DocumentId,
                        DocumentKeywordId = document.DocumentKeywordId,
                        DocumentOwnerId = document.DocumentOwnerId,
                        DocumentPhysicalPath = document.DocumentPhysicalPath,
                        DocumentTaskId = id,
                        ChangeState = entityState,
                        filename = null
                    };
                    documentVMs.Add(taskDocument);
                }

                return documentVMs;
            }
            return new List<DocumentViewModel>();
        }
        [NonAction]
        private async Task<IActionResult> UpdateDocuments(List<Models.ResponseModels.DocumentViewModel> taskDocuments, Models.Task task)
        {
            try
            {
                if (taskDocuments != null && taskDocuments.Any())
                {
                    string TaskFolder = Path.Combine(DocumentFolderPath, task.TaskId.ToString());
                    if (taskDocuments.All(d => d.ChangeState == Microsoft.EntityFrameworkCore.EntityState.Deleted))
                    {
                        if (Directory.Exists(TaskFolder))
                        {
                            Directory.Delete(TaskFolder, true);
                        }
                    }
                    if (taskDocuments.Any(d => d.ChangeState == Microsoft.EntityFrameworkCore.EntityState.Added))
                    {
                        if (!Directory.Exists(TaskFolder))
                        {
                            Directory.CreateDirectory(TaskFolder);
                        }
                    }
                    foreach (var documentViewModel in taskDocuments.Where(docs => docs.ChangeState != EntityState.Unchanged || docs.ChangeState != EntityState.Detached))
                    {
                        if (!string.IsNullOrEmpty(documentViewModel.filename)
                            && !string.IsNullOrEmpty(documentViewModel.file)
                            && (documentViewModel.ChangeState == EntityState.Added || documentViewModel.ChangeState == EntityState.Modified))
                        {
                            if (documentViewModel.ChangeState == Microsoft.EntityFrameworkCore.EntityState.Modified)
                            {
                                if (System.IO.File.Exists(documentViewModel.DocumentPhysicalPath))
                                {
                                    System.IO.File.Delete(documentViewModel.DocumentPhysicalPath);
                                }
                            }
                            documentViewModel.DocumentPhysicalPath = Path.Combine(TaskFolder, documentViewModel.filename);

                            await System.IO.File.WriteAllBytesAsync(documentViewModel.DocumentPhysicalPath, Convert.FromBase64String(documentViewModel.file));
                        }
                        else if (documentViewModel.ChangeState == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                        {
                            if (System.IO.File.Exists(documentViewModel.DocumentPhysicalPath))
                            {
                                System.IO.File.Delete(documentViewModel.DocumentPhysicalPath);
                            }
                        }

                        Models.Document document = new Models.Document
                        {
                            DocumentId = documentViewModel.DocumentId,
                            DocumentComments = documentViewModel.DocumentComments,
                            DocumentDate = documentViewModel.DocumentDate,
                            DocumentKeywordId = documentViewModel.DocumentKeywordId,
                            DocumentOwnerId = documentViewModel.DocumentOwnerId,
                            DocumentPhysicalPath = documentViewModel.DocumentPhysicalPath,
                            DocumentTaskId = task.TaskId
                        };
                        _context.Entry(document).State = documentViewModel.ChangeState;
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [NonAction]
        private List<StaffIncentiveViewModel> getIncentiveVMs(List<StaffIncentive> incentives, long id, EntityState entityState)
        {
            if (incentives != null && incentives.Any())
            {
                List<StaffIncentiveViewModel> incentiveVMs = new List<StaffIncentiveViewModel>();
                foreach (var incentive in incentives)
                {
                    StaffIncentiveViewModel taskReview = new StaffIncentiveViewModel
                    {
                        ChangeState = entityState,
                        IncetiveId = incentive.IncetiveId,
                        StaffUserId = incentive.StaffUserId,
                        AssignedBy = incentive.AssignedBy,
                        AssignedDate = incentive.AssignedDate,
                        TaskId = id,
                        IncentivesDecided = incentive.IncentivesDecided,
                        IncentivesPaid = incentive.IncentivesPaid,
                        IncentivesPaidDate = incentive.IncentivesPaidDate
                    };
                    incentiveVMs.Add(taskReview);
                }

                return incentiveVMs;
            }
            return new List<StaffIncentiveViewModel>();
        }
        [NonAction]
        private async Task<IActionResult> UpdateIncentives(List<Models.ResponseModels.StaffIncentiveViewModel> incentiveViewModels, Models.Task task)
        {
            try
            {
                if (incentiveViewModels != null && incentiveViewModels.Any())
                {
                    foreach (var incentiveViewModel in incentiveViewModels.Where(docs => docs.ChangeState != EntityState.Unchanged || docs.ChangeState != EntityState.Detached))
                    {
                        StaffIncentive staffIncentive = new StaffIncentive
                        {
                            IncetiveId = incentiveViewModel.IncetiveId,
                            StaffUserId = incentiveViewModel.StaffUserId,
                            AssignedBy = incentiveViewModel.AssignedBy,
                            AssignedDate = incentiveViewModel.AssignedDate,
                            TaskId = task.TaskId,
                            IncentivesDecided = incentiveViewModel.IncentivesDecided,
                            IncentivesPaid = incentiveViewModel.IncentivesPaid,
                            IncentivesPaidDate = incentiveViewModel.IncentivesPaidDate
                        };
                        _context.Entry(staffIncentive).State = incentiveViewModel.ChangeState;
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("downloadfile")]
        public async System.Threading.Tasks.Task<IActionResult> DownloadFile([FromQuery] long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Document document = await _context.Documents.FindAsync(id);

            if (document == null 
                || (document != null && string.IsNullOrEmpty(document.DocumentPhysicalPath))
                || (document != null && !string.IsNullOrEmpty(document.DocumentPhysicalPath) && !System.IO.File.Exists(document.DocumentPhysicalPath)))
            {
                return NotFound();
            }
            Byte[] bytes = await System.IO.File.ReadAllBytesAsync(document.DocumentPhysicalPath);
            return Ok(Convert.ToBase64String(bytes));
        }

    }
}