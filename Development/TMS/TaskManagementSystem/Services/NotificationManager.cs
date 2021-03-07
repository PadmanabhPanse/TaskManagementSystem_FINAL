using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Models.ResponseModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class NotificationManager
    {

        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public NotificationManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region NotificationSettings

        public async System.Threading.Tasks.Task<List<NotificationSettingResponse>> GetNotificationSettings(SearchNotificationSettingModel model)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/NotificationSettings?" + model.ToQueryString());
            return resp.ContentAsType<List<NotificationSettingResponse>>();
        }

        public async System.Threading.Tasks.Task<TaskNotificationSetting> GetNotificationSetting(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/NotificationSettings/{id}");
            return resp.ContentAsType<TaskNotificationSetting>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutNotificationSetting(long id, TaskNotificationSetting TaskNotification)
        {
            return await HttpRequestFactory.Put(this.ApiUrl + $"/NotificationSettings/{id}", TaskNotification);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostNotificationSetting(TaskNotificationSetting TaskNotification)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/NotificationSettings/", TaskNotification);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteNotificationSetting(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/NotificationSettings/{id}");
        }
        #endregion
        #region Notifications
        public async System.Threading.Tasks.Task<List<TaskNotification>> GetNotifications(SearchNotificationModel model)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/Notifications?" + model.ToQueryString());
            return resp.ContentAsType<List<TaskNotification>>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutNotification(long id, TaskNotification TaskNotification)
        {
            return await HttpRequestFactory.Put(this.ApiUrl + $"/Notifications/{id}", TaskNotification);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostNotification(TaskNotification TaskNotification)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + $"/Notifications/", TaskNotification);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteNotification(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Notifications/{id}");
        }

        public async System.Threading.Tasks.Task<TaskNotification> GetNotification(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Notifications/{id}");
            return resp.ContentAsType<TaskNotification>();
        }
        #endregion
    }
}
