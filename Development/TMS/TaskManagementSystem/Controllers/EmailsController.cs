using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;
namespace TaskManagementSystem.Controllers
{
    [Route("emails")]
    public class EmailsController : Controller
    {
        readonly EmailManager _emailManager;
        readonly IHostingEnvironment _environment;
        public EmailsController(EmailManager EmailManager, IHostingEnvironment environment)
        {
            _emailManager = EmailManager;
            _environment = environment;
        }
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _emailManager.GetEmails());
        }
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Email model)
        {
            if (ModelState.IsValid)
            {
                await _emailManager.PostEmail(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Menu/Edit/5
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Email = await _emailManager.GetEmail(id.Value);
            if (Email == null)
            {
                return NotFound();
            }
            return View(Email);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Email model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _emailManager.PutEmail(id, model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _emailManager.GetEmail(id.Value);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _emailManager.DeleteEmail(id);
            return RedirectToAction(nameof(Index));
        }
        [Route("view")]
        [HttpGet]
        public async Task<IActionResult> ViewEmails()
        {
            var Emails = await _emailManager.GetEmails();
            return View(Emails);
        }
    }
}
