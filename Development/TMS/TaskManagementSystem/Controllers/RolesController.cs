using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("roles")]
    public class RolesController : Controller
    {
        private readonly SiteMapManager _siteMapManager;
        private readonly TaskOwnerManager _ownerManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(SiteMapManager siteMapManager, TaskOwnerManager ownerManager, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _siteMapManager = siteMapManager;
            _ownerManager = ownerManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Route("accesscontrol")]
        public async Task<IActionResult> AccessControl()
        {
            List<string> roles = new List<string>();
            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }

            List<MenuItem> menuItems = await _siteMapManager.GetMenuItems();
            AccessControlViewModel model = new AccessControlViewModel
            {
                menuItems = menuItems,
                roles = roles
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoles(List<MenuItem> menuItems)
        {
            if (ModelState.IsValid && menuItems != null)
            {
                await _siteMapManager.AssignRoles(menuItems);
                return RedirectToAction("AccessControl");
            }

            return View(User);
        }

        #region Role CRUD
        [Route("")]
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roles = new List<IdentityRole>();
            await _roleManager.Roles.ForEachAsync(role => { roles.Add(role); });
            return View(roles);
        }
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Find the role by Role ID
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach (var user in _userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        [Route("edit")]
        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [Route("EditUsersInRole")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var users = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                users.Add(userRoleViewModel);
            }

            EditUsersInRole model = new EditUsersInRole
            {
                RoleId = roleId,
                RoleName = role.Name,
                users = users
            };

            return View(model);
        }
        [Route("EditUsersInRole")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(EditUsersInRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.RoleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.users.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model.users[i].UserId);

                IdentityResult result = null;

                if (model.users[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    var userCurrentRoles = await _userManager.GetRolesAsync(user);
                    result = await _userManager.RemoveFromRolesAsync(user, userCurrentRoles.Except(new string[] { role.Name }));

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot remove user existing roles");
                        return View(model);
                    }
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        if (string.Compare(role.Name, "Admin") == 0 || string.Compare(role.Name, "TaskManager") == 0 || string.Compare(role.Name, "Staff") == 0)
                        {
                            await _ownerManager.PostTaskOwner(new TaskOwner() { TaskOwnerIsLock = false, TaskOwnerName = $"{user.FirstName} {user.LastName}", TaskOwnerAddress = string.Empty, TaskOwnerPhoneNo = user.PhoneNumber, TaskOwnerEmail = user.Email, UserId = user.Id, TaskOwnerDateOfBirth = DateTime.Now, TaskOwnerJoinDate = DateTime.Now, });
                        }
                    }
                }
                else if (!model.users[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        if (string.Compare(role.Name, "Admin") == 0 || string.Compare(role.Name, "TaskManager") == 0 || string.Compare(role.Name, "Staff") == 0)
                        {
                            Models.TaskOwner taskOwner = (await _ownerManager.GetTaskOwners(new Models.ListSearchModels.TaskOwnerSearchModel { UserId = user.Id })).FirstOrDefault();
                            if (taskOwner != null)
                            {
                                await _ownerManager.DeleteTaskOwner(taskOwner.TaskOwnerId);
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.users.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { Id = model.RoleId });
                }
            }

            return RedirectToAction("Edit", new { Id = model.RoleId });
        }
        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);

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
        #endregion 
    }

}
