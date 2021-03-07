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
    [Route("sms")]
    public class SmsController : Controller
    {
        readonly SmsManager _smsManager;
        readonly IHostingEnvironment _environment;
        public SmsController(SmsManager smsManager, IHostingEnvironment environment)
        {
            _smsManager = smsManager;
            _environment = environment;
        }
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _smsManager.GetSmss());
        }
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sms model)
        {
            if (ModelState.IsValid)
            {
                await _smsManager.PostSms(model);
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

            var Sms = await _smsManager.GetSms(id.Value);
            if (Sms == null)
            {
                return NotFound();
            }
            return View(Sms);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Sms model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _smsManager.PutSms(id, model);
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

            var menu = await _smsManager.GetSms(id.Value);
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
            await _smsManager.DeleteSms(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
