using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("taskowners")]
    public class TaskOwnersController : Controller
    {
        private readonly TaskOwnerManager _manager;
        public TaskOwnersController(TaskOwnerManager manager)
        {
            _manager = manager;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("gettaskowners")]
        public async Task<IActionResult> gettaskowners(TaskOwnerSearchModel model)
        {
            return Json(await _manager.GetTaskOwners(model));
        }

        [Route("edittaskowner")]
        [HttpGet]
        public async Task<IActionResult> EditTaskOwner(long? id)
        {
            Models.TaskOwner owner = new Models.TaskOwner();
            if (id.HasValue)
            {
                owner = await _manager.GetTaskOwner(id.Value);
                if (owner == null)
                {
                    return NotFound();
                }
            }
            return Json(owner);
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.TaskOwner model)
        {
            if (model.TaskOwnerId == 0)
            {
                await _manager.PostTaskOwner(model);
            }
            else
            {
                await _manager.PutTaskOwner(model.TaskOwnerId, model);
            }
            return NoContent();
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            Models.TaskOwner owner = new Models.TaskOwner();
            if (id.HasValue)
            {
                owner = await _manager.GetTaskOwner(id.Value);
                if (owner == null)
                {
                    return NotFound();
                }
                await _manager.DeleteTaskOwner(id.Value);
            }
            return NoContent();
        }
    }
}
