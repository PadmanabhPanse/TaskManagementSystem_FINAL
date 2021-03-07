using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {

        readonly SubscriptionManager _subscriptionManager;
        private DashboardManager _manager;
        IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public HomeController(DashboardManager manager, IHttpContextAccessor httpContextAccessor, SubscriptionManager subscriptionManager)
        {
            _manager = manager;
            _httpContextAccessor = httpContextAccessor;
            _subscriptionManager = subscriptionManager;
        }

        [AllowAnonymous]
        [Route("landingpage")]
        public async Task<IActionResult> LandingPage()
        {
            var subscriptions = await _subscriptionManager.GetSubscriptions();
            return View(subscriptions);
        }

        [Route("")]
        [Route("~/")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            Models.Dashboard dashboard = null;
            if (User.IsInRole("Admin"))
            {
                dashboard = await _manager.GetDashboardData();
                dashboard.subscriptions = await _subscriptionManager.GetSubscriptions();
            }
            else if (User.IsInRole("TaskManager") || User.IsInRole("Staff"))
            {
                if (User.IsInRole("TaskManager"))
                {
                    dashboard = await _manager.GetDashboardData("TaskOwnerId",Convert.ToInt64(Session.GetString("TaskOwnerId")));
                }
                else if (User.IsInRole("Staff"))
                {
                    dashboard = await _manager.GetDashboardData("TaskStaffId", Convert.ToInt64(Session.GetString("TaskOwnerId")));
                }
            }
            else
            {
                dashboard = await _manager.GetDashboardData(Session.GetString("UserId"));
            }
            return View(dashboard);
        }
    }
}