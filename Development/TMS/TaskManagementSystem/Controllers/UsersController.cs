using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TaskOwnerManager _ownerManager;
        public UsersController(TaskOwnerManager ownerManager, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _ownerManager = ownerManager;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token }, Request.Scheme);
                    _logger.Log(LogLevel.Warning, confirmationLink);

                    return View("Index");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetRolesAsync returns the list of user Roles
            string userRole = string.Empty;
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    Value = role.Id,
                    Text = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole = role.Name;
                }

                userRoles.Add(userRolesViewModel);
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                userRoles = userRoles,
                UserRole = userRole,
                Profession=user.Profession
            };

            return View(model);
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Profession = model.Profession;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Update Failed");
                    return View(model);
                }

                var userCurrentRoles = await _userManager.GetRolesAsync(user);
                result = await _userManager.RemoveFromRolesAsync(user, userCurrentRoles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(model);
                }

                result = await _userManager.AddToRolesAsync(user, new string[] { model.UserRole });//new Role

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }
                if (result.Succeeded)
                {
                    if (!userCurrentRoles.Contains(model.UserRole) || !userCurrentRoles.Contains(model.UserRole))
                    {
                        if ((userCurrentRoles.Contains("Admin") || userCurrentRoles.Contains("TaskManager") || userCurrentRoles.Contains("Staff")) && (model.UserRole != "Admin" || model.UserRole != "TaskManager" || model.UserRole != "Staff"))
                        {
                            Models.TaskOwner taskOwner = (await _ownerManager.GetTaskOwners(new Models.ListSearchModels.TaskOwnerSearchModel { UserId = user.Id })).FirstOrDefault();
                            if (taskOwner != null)
                            {
                                await _ownerManager.DeleteTaskOwner(taskOwner.TaskOwnerId);
                            }
                        }
                        if ((!userCurrentRoles.Contains("Admin") || !userCurrentRoles.Contains("TaskManager") || !userCurrentRoles.Contains("Staff")) && (model.UserRole == "Admin" || model.UserRole == "TaskManager" || model.UserRole == "Staff"))
                        {
                            await _ownerManager.PostTaskOwner(new TaskOwner() { TaskOwnerIsLock = false, TaskOwnerName = $"{user.FirstName} {user.LastName}", TaskOwnerAddress = string.Empty, TaskOwnerPhoneNo = user.PhoneNumber, TaskOwnerEmail = user.Email, UserId = user.Id, TaskOwnerDateOfBirth = DateTime.Now, TaskOwnerJoinDate = DateTime.Now, });
                        }
                    }


                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                string userCurrentRole = (await _userManager.GetRolesAsync(user))?.FirstOrDefault();

                if (!string.IsNullOrEmpty(userCurrentRole) && (string.Compare(userCurrentRole, "Admin") == 0 || string.Compare(userCurrentRole, "TaskManager") == 0 || string.Compare(userCurrentRole, "Staff") == 0))
                {
                    TaskOwner taskOwner = (await _ownerManager.GetTaskOwners(new Models.ListSearchModels.TaskOwnerSearchModel { UserId = user.Id }))?.FirstOrDefault();
                    if (taskOwner != null)
                    {
                        await _ownerManager.DeleteTaskOwner(taskOwner.TaskOwnerId);
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Delete");
            }
        }
    }
}