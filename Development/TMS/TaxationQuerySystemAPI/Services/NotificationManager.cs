using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;

namespace TaxationQuerySystemAPI.Services
{
    public class NotificationManager
    {
        readonly TMSDBContext _context;
        readonly INotificationSender _sender;

        public NotificationManager(TMSDBContext context, INotificationSender sender)
        {
            _context = context;
            _sender = sender;
        }

        public async Task<IEnumerable<Models.TaskNotification>> GetNotifications(SearchNotificationModel model)
        {
            IQueryable<TaskNotification> settings = _context.Notifications;

            var SearchFieldMutators = new List<SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>>();

            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => !string.IsNullOrEmpty(c.ObjectId) && !string.IsNullOrEmpty(c.ObjectType), (list, c) => list.Where(o => o.ObjectId == c.ObjectId)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.OwnerId > 0, (list, c) => list.Where(o => o.OwnerId == c.OwnerId)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(o => o.UserId == c.UserId)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.SettingId > 0, (list, c) => list.Where(o => o.SettingId == c.SettingId)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.IsRead.HasValue, (list, c) => list.Where(o => o.IsRead == c.IsRead.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.IsPopup.HasValue, (list, c) => list.Where(o => o.IsPopup == c.IsPopup.Value)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.EmailTime != DateTime.MinValue, (list, c) => list.Where(o => o.EmailTime.Date == c.EmailTime.Date)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.SmsTime != DateTime.MinValue, (list, c) => list.Where(o => o.SmsTime.Date == c.SmsTime.Date)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.NotificationDate != DateTime.MinValue, (list, c) => list.Where(o => o.SmsTime.Date == c.SmsTime.Date)));
            SearchFieldMutators.Add(new SearchFieldMutator<Models.TaskNotification, SearchNotificationModel>(c => c.PopupDate != DateTime.MinValue, (list, c) => list.Where(o => o.PopupDate.Date == c.PopupDate.Date)));



            foreach (var item in SearchFieldMutators)
            {
                settings = item.Apply(model, settings);
            }


            return await settings.OrderByDescending(o => o.NotificationDate).ToListAsync();
        }

        public async Task<Models.TaskNotification> GetNotification(long id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    throw new Exception("Not Found");
                }
                return notification;
            }
            catch (Exception ex) { throw ex; }
        }

        // PUT: api/Tasks/5
        [HttpPut]
        public async Task<int> PutNotification(long id, Models.TaskNotification notification)
        {
            if (id != notification.NotificationId)
            {
                throw new Exception("Not Found");
            }
            try
            {
                _context.Entry(notification).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!NotificationExists(id))
                {
                    throw new Exception("Not Found");
                }
                else
                {
                    throw ex;
                }
            }

        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<int> PostNotification(Models.TaskNotification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/Tasks/5
        [HttpDelete]
        public async Task<int> DeleteNotification(long id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    throw new Exception("Not Found");
                }
                _context.Notifications.Remove(notification);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool NotificationExists(long id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }

        public async Task<System.Threading.Tasks.Task> SendNotification(
            TaskNotificationSetting setting,
            TaskNotification newNotification,
            List<string> recipientPhoneNos,
            Tuple<string, string> sender,
            List<Tuple<string, string>> recipients)
        {
            if ((setting.Sms || setting.Email) && setting != null && newNotification != null)
            {
                if (setting.Sms && recipientPhoneNos != null && recipientPhoneNos.Count > 0)
                {
                    var resp = await _sender.SendSMS(recipientPhoneNos, newNotification.Description);
                }
                if (setting.Email)
                {
                    await _sender.SendEmail(
                   sender,
                    recipients
                   , "TMS:" + setting.Type
                   , newNotification.Description
                   , null);
                }
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
