using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{

    public class NotificationsController : Controller
    {
        private readonly NotificationManager _manager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        public NotificationsController(NotificationManager manager, IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _httpContextAccessor = httpContextAccessor;

        }
        [Route("/notifications/settings/")]
        [Route("/notifications/settings/index")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        [Route("/notifications/getnotificationsettings")]
        public async Task<IActionResult> GetNotificationSettings(SearchNotificationSettingModel model)
        {
            return Json(await _manager.GetNotificationSettings(model));
        }

        [Route("/notifications/editnotificationsetting")]
        [HttpGet]
        public async Task<IActionResult> EditNotificationSetting(long? id)
        {
            Models.TaskNotificationSetting setting = new Models.TaskNotificationSetting();
            if (id.HasValue)
            {
                setting = await _manager.GetNotificationSetting(id.Value);
                if (setting == null)
                {
                    return NotFound();
                }
            }
            return Json(setting);
        }
        [Route("/notifications/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.TaskNotificationSetting model)
        {
            if (model.SettingId == 0)
            {
                await _manager.PostNotificationSetting(model);
            }
            else
            {
                await _manager.PutNotificationSetting(model.SettingId, model);
            }
            return NoContent();
        }

        [Route("/notifications/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            Models.TaskNotificationSetting setting = new Models.TaskNotificationSetting();
            if (id.HasValue)
            {
                setting = await _manager.GetNotificationSetting(id.Value);
                if (setting == null)
                {
                    return NotFound();
                }
                await _manager.DeleteNotificationSetting(id.Value);
            }
            return NoContent();
        }
        [Route("/notifications/")]
        [Route("/notifications/index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/notifications/getnotifications")]
        public async Task<IActionResult> GetNotifications(SearchNotificationModel model)
        {
            model.IsPopup = false;
            model.UserId = Session.GetString("UserId");
            return Json(await _manager.GetNotifications(model));
        }
        [Route("/notifications/popups")]
        public IActionResult Popups()
        {
            return View();
        }
        [HttpPost]
        [Route("/notifications/getpopups")]
        public async Task<IActionResult> GetPopups(SearchNotificationModel model)
        {
            model.IsPopup = true;
            if (!User.IsInRole("Admin"))
            {
                model.UserId = Session.GetString("UserId");
            }

            return Json(await _manager.GetNotifications(model));
        }
        [Route("/notifications/editpopup")]
        [HttpGet]
        public async Task<IActionResult> EditPopup(long? id)
        {
            Models.TaskNotification popup = new Models.TaskNotification();
            if (id.HasValue)
            {
                popup = await _manager.GetNotification(id.Value);
                if (popup == null)
                {
                    return NotFound();
                }
            }
            return Json(popup);
        }
        [Route("/notifications/editnotification")]
        [HttpPost]
        public async Task<IActionResult> EditNotification(Models.TaskNotification model)
        {
            if (model.NotificationId == 0)
            {
                await _manager.PostNotification(model);
            }
            else
            {
                await _manager.PutNotification(model.NotificationId, model);
            }
            return NoContent();
        }

        [Route("/notifications/markread")]
        [HttpPost]
        public async Task<IActionResult> MarkNotificationRead(long id)
        {
            Models.TaskNotification notification = await _manager.GetNotification(id);
            notification.IsRead = true;
            await _manager.PutNotification(id, notification);
            return Ok();
        }

        [Route("/notifications/deletenotification")]
        [HttpGet]
        public async Task<IActionResult> Deletenotification(long? id)
        {
            if (id.HasValue)
            {
                var taskNotification = await _manager.GetNotification(id.Value);
                if (taskNotification == null)
                {
                    return NotFound();
                }
                await _manager.DeleteNotification(id.Value);
            }
            return NoContent();
        }
    }
}