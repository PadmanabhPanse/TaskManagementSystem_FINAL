using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("clients")]
    public class ClientsController : Controller
    {
        private readonly ClientManager _manager;
        public ClientsController(ClientManager manager)
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
        [Route("getclients")]
        public async Task<IActionResult> getclients(SearchClient model)
        {
            return Json(await _manager.GetClients(model));
        }

        [Route("editclient")]
        [HttpGet]
        public async Task<IActionResult> EditClient(long? id)
        {
            Models.Client owner = new Models.Client();
            if (id.HasValue)
            {
                owner = await _manager.GetClient(id.Value);
                if (owner == null)
                {
                    return NotFound();
                }
            }
            return Json(owner);
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.Client model)
        {
            if (model.ClientId == 0)
            {
                await _manager.PostClient(model);
            }
            else
            {
                await _manager.PutClient(model.ClientId, model);
            }
            return NoContent();
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            Models.Client owner = new Models.Client();
            if (id.HasValue)
            {
                owner = await _manager.GetClient(id.Value);
                if (owner == null)
                {
                    return NotFound();
                }
                await _manager.DeleteClient(id.Value);
            }
            return NoContent();
        }
    }
}
