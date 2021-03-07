using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("menu")]
    public class MenuController : Controller
    {
        private readonly SiteMapManager _siteMapManager;
        public MenuController(SiteMapManager siteMapManager)
        {
            _siteMapManager = siteMapManager;
        }
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _siteMapManager.GetMenus());
        }

        [Route("details")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _siteMapManager.GetMenu(id.Value);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                await _siteMapManager.PostMenu(menu);
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _siteMapManager.GetMenu(id.Value);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _siteMapManager.PutMenu(id, menu);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _siteMapManager.GetMenu(id.Value);
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
            await _siteMapManager.DeleteMenu(id);
            return RedirectToAction(nameof(Index));
        }

        [Route("createitem")]
        public async Task<IActionResult> CreateItem(long parentMenuId)
        {
            Menu menu = await _siteMapManager.GetMenu(parentMenuId);
            @ViewBag.ParentMenuName = menu.Name;
            return View(new MenuItem { ParentMenuId = parentMenuId });
        }

        // POST: MenuItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("createitem")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(long parentMenuId, [Bind("Id,ParentMenuId,Name,ActionName,ControllerName,Url")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                Menu menu = await _siteMapManager.GetMenu(parentMenuId);
                menuItem.ParentMenuId = parentMenuId;
                await _siteMapManager.PostMenuItem(parentMenuId, menuItem);
                return RedirectToAction("Details", "Menu", new { id = parentMenuId });
            }
            return View(menuItem);
        }

        [Route("edititem")]
        public async Task<IActionResult> EditItem(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _siteMapManager.GetMenuItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("edititem")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(long parentMenuId, [Bind("Id,ParentMenuId,Name,ActionName,ControllerName,Url,Roles")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _siteMapManager.PutMenuItem(menuItem.Id, parentMenuId, menuItem);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Details", "Menu", new { id = parentMenuId });
            }
            return View(menuItem);
        }

        [Route("deleteitem")]
        public async Task<IActionResult> DeleteItem(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _siteMapManager.GetMenuItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [Route("deleteitem")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(long parentMenuId, long id)
        {
            var menuItem = await _siteMapManager.GetMenuItem(id);
            await _siteMapManager.DeleteMenuItem(id, parentMenuId);
            return RedirectToAction("Details", "Menu", new { id = parentMenuId });
        }
    }
}
