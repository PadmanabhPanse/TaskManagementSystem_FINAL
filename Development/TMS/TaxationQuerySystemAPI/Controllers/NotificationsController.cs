using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Models.ResponseModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TMSDBContext _context;
        private readonly NotificationManager _manager;
        public NotificationsController(TMSDBContext context, NotificationManager manager)
        {
            _context = context;
            _manager = manager;
        }
        #region Settings
        [HttpGet("~/api/NotificationSettings")]
        public async Task<IEnumerable<Models.TaskNotificationSetting>> GetSettings([FromQuery] SearchNotificationSettingModel model)
        {
            IQueryable<TaskNotificationSetting> settings = _context.NotificationSettings;

            var SearchFieldMutators = new List<SearchFieldMutator<Models.TaskNotificationSetting, SearchNotificationSettingModel>>();

            //SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotificationSetting, SearchNotificationSettingModel>(c => c.TaskId > 0, (list, c) => list.Where(o => o.TaskId == c.TaskId)));
            //SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotificationSetting, SearchNotificationSettingModel>(c => c.OwnerId > 0, (list, c) => list.Where(o => o.OwnerId == c.OwnerId)));
            //SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotificationSetting, SearchNotificationSettingModel>(c => c.ClientId > 0, (list, c) => list.Where(o => o.ClientId == c.ClientId)));
            //SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotificationSetting, SearchNotificationSettingModel>(c => !string.IsNullOrEmpty(c.ManagerId), (list, c) => list.Where(o => o.ManagerId == c.ManagerId)));
            SearchFieldMutators.Add(new SearchFieldMutator<TaskNotificationSetting, SearchNotificationSettingModel>(c => !string.IsNullOrEmpty(c.TaskChange), (list, c) => list.Where(o => string.Compare(o.TaskChange, c.TaskChange) == 0)));
            SearchFieldMutators.Add(new SearchFieldMutator<TaskNotificationSetting, SearchNotificationSettingModel>(c => !string.IsNullOrEmpty(c.Type), (list, c) => list.Where(o => string.Compare(o.Type, c.Type) == 0)));
            SearchFieldMutators.Add(new SearchFieldMutator<TaskNotificationSetting, SearchNotificationSettingModel>(c => c.Popup.HasValue, (list, c) => list.Where(o => o.Popup == c.Popup.Value)));

            foreach (var item in SearchFieldMutators)
            {
                settings = item.Apply(model, settings);
            }

            return await settings.ToListAsync();
        }
        //var responses = from setting in settings
        //                join task in _context.Tasks on setting.TaskId equals task.TaskId
        //                join owner in _context.TaskOwners on setting.OwnerId equals owner.TaskOwnerId
        //                join client in _context.Clients on setting.ClientId equals client.ClientId
        //                join user in _context.TaskOwners on setting.ManagerId equals user.UserId
        //                select new Models.ResponseModels.NotificationSettingResponse
        //                {
        //                    Client = client.ClientContactPerson + " (" + client.ClientCompanyName + ")",
        //                    Owner = owner.TaskOwnerName,
        //                    Task = task.TaskName,
        //                    SettingId = setting.SettingId,
        //                    Type = setting.Type,
        //                    TaskChange = setting.TaskChange,
        //                    Dashboard = setting.Dashboard,
        //                    Email = setting.Email,
        //                    Sms = setting.Sms,
        //                    OwnerId = setting.OwnerId,
        //                    ClientId = setting.ClientId,
        //                    ManagerId = setting.ManagerId,
        //                    TaskId = setting.SettingId,
        //                    Manager = owner.TaskOwnerName
        //                };

        //  return await responses.OrderByDescending(o => o.SettingId).ToListAsync();
        // }

        [HttpGet("~/api/NotificationSettings/{id}")]
        public async Task<IActionResult> GetSetting([FromRoute] long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var setting = await _context.NotificationSettings.FindAsync(id);
                if (setting == null)
                {
                    return NotFound();
                }
                return Ok(setting);
            }
            catch (Exception ex) { throw ex; }
        }

        // PUT: api/Tasks/5
        [HttpPut("~/api/NotificationSettings/{id}")]
        public async Task<IActionResult> PutNotificationSetting([FromRoute] long id, [FromBody] Models.TaskNotificationSetting setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != setting.SettingId)
            {
                return BadRequest();
            }
            try
            {
                _context.Entry(setting).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!NotificationSettingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        [HttpPost("~/api/NotificationSettings")]
        public async Task<IActionResult> PostNotificationSetting([FromBody] Models.TaskNotificationSetting setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.NotificationSettings.Add(setting);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CreatedAtAction("GetNofication", new { id = setting.SettingId }, setting);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("~/api/NotificationSettings/{id}")]
        public async Task<IActionResult> DeleteNotificationSetting([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var setting = await _context.NotificationSettings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            _context.NotificationSettings.Remove(setting);
            await _context.SaveChangesAsync();

            return Ok(setting);
        }

        private bool NotificationSettingExists(long id)
        {
            return _context.NotificationSettings.Any(e => e.SettingId == id);
        }
        #endregion
        #region Notifications
        [HttpGet("~/api/Notifications/")]
        public async Task<IEnumerable<Models.TaskNotification>> GetNotifications([FromQuery] SearchNotificationModel model)
        {
            return await _manager.GetNotifications(model);
        }

        [HttpGet("~/api/Notifications/{id}")]
        public async Task<IActionResult> GetNotification([FromRoute] long id)
        {
            try
            {
                return Ok(await _manager.GetNotification(id));
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut("~/api/Notifications/{id}")]
        public async Task<IActionResult> PutNotification([FromRoute] long id, [FromBody] Models.TaskNotification notification)
        {
            return Ok(await _manager.PutNotification(id, notification));
        }

        [HttpPost("~/api/Notifications/")]
        public async Task<IActionResult> PostNotification([FromBody] Models.TaskNotification notification)
        {
            return Ok(await _manager.PostNotification(notification));
        }

        [HttpDelete("~/api/Notifications/{id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] long id)
        {
            return Ok(await _manager.DeleteNotification(id));
        }

        #endregion

        //#region Popup
        //[HttpGet("~/api/Notifications/getpopups")]
        //public async Task<IEnumerable<Models.ResponseModels.TaskNotificationViewModel>> getpopups([FromQuery] SearchNotificationModel model)
        //{
        //    return await _manager.GetNotifications(model);
        //}

        //private TaskNotificationViewModel GetTaskNotificationViewModel(taskn)
        //#endregion
    }
}
