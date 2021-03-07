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
    [Route("taxtypes")]
    public class TaxTypesController:Controller
    {
        readonly TaxTypeManager _taxTypeManager;
        readonly IHostingEnvironment _environment;
        public TaxTypesController(TaxTypeManager taxTypeManager, IHostingEnvironment environment)
        {
            _taxTypeManager = taxTypeManager;
            _environment = environment;
        }
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _taxTypeManager.GetTaxTypes());
        }
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxType model)
        {
            if (ModelState.IsValid)
            {
                await _taxTypeManager.PostTaxType(model);
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

            var taxType = await _taxTypeManager.GetTaxType(id.Value);
            if (taxType == null)
            {
                return NotFound();
            }
            return View(taxType);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TaxType model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _taxTypeManager.PutTaxType(id, model);
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

            var menu = await _taxTypeManager.GetTaxType(id.Value);
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
            await _taxTypeManager.DeleteTaxType(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
