using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        readonly ILogger<AccountController> logger;
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly TaskOwnerManager _taskOwner;
        readonly SubscriberManager _subscriberManager;
        readonly SubscriptionManager _subscriptionManager;
        readonly QuotationManager _quotationManager;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IHttpContextAccessor httpContextAccessor,
            TaskOwnerManager taskOwner,
            SubscriberManager subscriberManager,
            SubscriptionManager subscriptionManager,
            QuotationManager quotationManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _taskOwner = taskOwner;
            _quotationManager = quotationManager;
            _subscriberManager = subscriberManager;
            _subscriptionManager = subscriptionManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            bool.TryParse(_httpContextAccessor.HttpContext.Request.Query["FromQuotePage"], out bool FromQuotePage);
            TempData["FromQuotePage"] = FromQuotePage;

            long.TryParse(_httpContextAccessor.HttpContext.Request.Query["SubscriptionId"], out long SubscriptionId);
            TempData["SubscriptionId"] = SubscriptionId;

            if (!User.IsInRole("Admin") && (FromQuotePage || SubscriptionId > 0))
            {
                await signInManager.SignOutAsync();
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool.TryParse(TempData.ContainsKey("FromQuotePage") ? TempData["FromQuotePage"].ToString() : "false", out bool FromQuotePage);
                TempData.Keep("FromQuotePage");

                long.TryParse(TempData.ContainsKey("SubscriptionId") ? TempData["SubscriptionId"].ToString() : "0", out long SubscriptionId);
                TempData.Keep("SubscriptionId");
                // Copy data from RegisterViewModel to ApplicationUser
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = model.Email,
                    Profession=model.Profession,
                    EmailConfirmed = true
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    // var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //
                    // var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    //     new { userId = user.Id, token = token }, Request.Scheme);
                    // logger.Log(LogLevel.Warning, confirmationLink);
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("index", "users");
                    }
                    else
                    {
                        if (FromQuotePage)
                        {
                            await userManager.AddToRoleAsync(user, "QuoteUser");
                        }
                        else if (SubscriptionId > 0)
                        {
                            Subscription subscription = await _subscriptionManager.GetSubscription(SubscriptionId);
                            Subscriber subscriber = new Subscriber
                            {
                                SubscriberId = Guid.NewGuid().ToString(),
                                SubscriptionId = subscription.Id,
                                UserId = user.Id,
                                SubscriptionStartDate = DateTime.Now,
                                SubscriptionEndDate = DateTime.Now,
                                IsLocked = false,
                                TotalCost = subscription.TotalCost,
                                BalanceAmount = subscription.TotalCost
                            };
                            await _subscriberManager.PostSubscriber(subscriber);
                            await userManager.AddToRoleAsync(user, "User");
                        }
                        await signInManager.SignInAsync(user, isPersistent: false);
                        Session.SetString("UserId", user.Id);
                        if (FromQuotePage)
                        {
                            return RedirectToAction("index", "quotations");
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }

                    // ViewBag.ErrorTitle = "Registration successful";
                    // ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                    //         "email, by clicking on the confirmation link we have emailed you";
                    // return View("Error");
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

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return Redirect("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("Login");
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return Redirect("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.UserName);

                if (user != null && !user.EmailConfirmed &&
                            (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    Session.SetString("UserId", user.Id);
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("QuoteUser"))
                    {
                        return RedirectToAction("index", "quotations");
                    }
                    else if (roles.Contains("User"))
                    {
                        Subscriber subscriber = await _subscriberManager.GetActiveSubscriberByUserId(user.Id);
                        if (subscriber != null)
                        {
                            Session.SetString("SubscriberId", subscriber.SubscriberId);
                        }
                        //ApplicationUser Admin = (await userManager.GetUsersInRoleAsync("Admin"))?.FirstOrDefault();
                        //if (Admin == null)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Admin Role not found");
                        //}
                        //TaskOwner taskOwner = (await _taskOwner.GetTaskOwners(new Models.ListSearchModels.TaskOwnerSearchModel { UserId = Admin.Id }))?.FirstOrDefault();
                        //if (taskOwner == null)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Admin Role not found");
                        //}
                        //Session.SetString("TaskOwnerId", taskOwner.TaskOwnerId.ToString());
                        //Session.SetString("TaskOwner", "Admin");
                    }
                    else if (roles.Contains("TaskManager") || roles.Contains("Staff"))
                    {
                        TaskOwner taskOwner = (await _taskOwner.GetTaskOwners(new Models.ListSearchModels.TaskOwnerSearchModel { UserId = user.Id }))?.FirstOrDefault();
                        if (taskOwner == null)
                        {
                            ModelState.AddModelError(string.Empty, "TaskOwner Role not found");
                        }
                        Session.SetString("TaskOwnerId", taskOwner.TaskOwnerId.ToString());
                        Session.SetString("TaskOwner", taskOwner.TaskOwnerName);
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
                // If account is lockedout send the use to AccountLocked view
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token }, Request.Scheme);

                    // Log the password reset link
                    logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        // Upon successful password reset and if the account is lockedout, set
                        // the account lockout end date to current UTC date time, so the user
                        // can login with the new password
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }
    }
}