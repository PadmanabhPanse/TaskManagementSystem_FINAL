using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("tasks")]
    public class TasksController : Controller
    {

        private readonly TaskManager _manager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public TasksController(TaskManager manager, IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("gettasks")]
        public async Task<IActionResult> gettasks(TaskSearchModel model)
        {
            if (!User.IsInRole("Admin"))
            {
                if (User.IsInRole("User"))
                {
                    model.UserId = Session.GetString("UserId");
                }
                else if (User.IsInRole("TaskManager"))
                {
                    model.TaskOwnerId = Convert.ToInt64(Session.GetString("TaskOwnerId"));
                }
                else if (User.IsInRole("Staff"))
                {
                    model.TaskStaffId = Convert.ToInt64(Session.GetString("TaskOwnerId"));
                }
            }

            return Json(await _manager.GetTasks(model));
        }

        [Route("all")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("completed")]
        public IActionResult CompletedTasks()
        {
            return View("Index", new TaskSearchModel { TaskStatusId = "Completed" });
        }

        [Route("ongoing")]
        public IActionResult OngoingTasks()
        {
            return View("Index", new TaskSearchModel { TaskStatusId = "Ongoing" });
        }

        [Route("rejected")]
        public IActionResult RejectedTasks()
        {
            return View("Index", new TaskSearchModel { TaskStatusId = "Rejected" });
        }

        [Route("pending")]
        public IActionResult PendingTasks()
        {
            return View("Index", new TaskSearchModel { TaskStatusId = "Pending" });
        }

        [Route("closed")]
        public IActionResult ClosedTasks()
        {
            return View("Index", new TaskSearchModel { TaskStatusId = "Closed" });
        }

        [Route("edittask")]
        [HttpGet]
        public async Task<IActionResult> EditTask(long? id)
        {
            Models.Task task = new Models.Task();
            if (id.HasValue)
            {
                task = await _manager.GetTask(id.Value);
                if (task == null)
                {
                    return NotFound();
                }
            }
            return Json(task);
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.Task model)
        {
            if (model.TaskId == 0)
            {
                await _manager.PostTask(model);
            }
            else
            {
                await _manager.PutTask(model.TaskId, model);
            }
            return NoContent();
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id.HasValue)
            {
                Models.Task task = await _manager.GetTask(id.Value);
                if (task == null)
                {
                    return NotFound();
                }
               
                await _manager.DeleteTask(id.Value);
            }
            return NoContent();
        }
        [Route("getmaxtaskpriority")]
        [HttpGet]
        public async Task<long> getMaxTaskPriority(long? id)
        {
            long maxTaskPriority = 0;
            var Tasks = await _manager.GetTasks(new TaskSearchModel { TaskOwnerId = id.Value });
            if (Tasks != null && Tasks.Count() > 0)
            {
                maxTaskPriority = Tasks.Max(t => t.TaskPriority.Value);
            }
            return maxTaskPriority + 1;
        }
        [Route("downloadfile")]
        [HttpGet]
        public async Task<IActionResult> DownloadFile(long? id,string filename)
        {
            string base64file = await _manager.DownloadFile(id.Value);
            return File(Convert.FromBase64String(base64file), System.Net.Mime.MediaTypeNames.Application.Octet,filename);
        }
    }
}
