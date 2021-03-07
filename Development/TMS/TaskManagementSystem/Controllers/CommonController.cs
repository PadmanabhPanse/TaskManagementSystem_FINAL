using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("common")]
    public class CommonController : Controller
    {
        readonly TaxTypeManager _taxTypeManager;
        readonly CommonManager _manager;
        readonly UserManager<ApplicationUser> _userManager;
        public CommonController(TaxTypeManager taxTypeManager,CommonManager manager, UserManager<ApplicationUser> userManager)
        {
            _manager = manager;
            _userManager = userManager;
            _taxTypeManager = taxTypeManager;
        }

        [HttpPost]
        [Route("gettaskowners")]
        public async Task<IActionResult> gettaskowners()
        {
            List<Models.TaskOwner> taskOwners = await _manager.GetTaskOwners();

            if (User.IsInRole("Admin"))
            {
                taskOwners = gettaskownersbyrole(taskOwners, "TaskManager");
            }
            else if (User.IsInRole("TaskManager"))
            {
                taskOwners = gettaskownersbyrole(taskOwners, "Staff");
            }

            return Json(taskOwners);
        }

        [HttpPost]
        [Route("gettasks")]
        public async Task<IActionResult> gettasks()
        {
            return Json(await _manager.GetTasks());
        }

        [HttpPost]
        [Route("getclients")]
        public async Task<IActionResult> getclients()
        {
            return Json(await _manager.GetClients());
        }

        [HttpPost]
        [Route("getusers")]
        public async Task<IActionResult> getusers(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return Json(users);
        }

        private List<Models.TaskOwner> gettaskownersbyrole(List<Models.TaskOwner> taskOwners, string roleName)
        {
            var users = _userManager.GetUsersInRoleAsync(roleName).Result;
            var userIds = users.Select(u => u.Id);
            return taskOwners.Where(tO => userIds.Contains(tO.UserId)).ToList();
        }

        [HttpGet]
        [Route("getcountries")]
        public async Task<IActionResult> getcountries()
        {
            return Json(await _manager.GetCountries());
        }

        [HttpGet]
        [Route("getcurrencies")]
        public async Task<IActionResult> getcurrencies()
        {
            return Json(await _manager.GetCurrencies());
        }

        [HttpGet]
        [Route("convertcurrency")]
        public async Task<IActionResult> convertcurrency(string fromCurrency, string toCurrency, double amount)
        {
            return Json(await _manager.ConvertCurrency(fromCurrency, toCurrency, amount));
        }

        [HttpGet]
        [Route("gettaxtypes")]
        public async Task<IActionResult> gettaxtypes()
        {
            return Json(await _taxTypeManager.GetTaxTypes());
        }
    }
}
