using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Models.ResponseModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class SubscriberManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public SubscriberManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        public async System.Threading.Tasks.Task<List<SubscriberViewModel>> GetSubscribers(SearchSubscriber model)
        {
            var resp = await HttpRequestFactory.Post($"{this.ApiUrl}/Subscribers/getsubscribers", model);
            return resp.ContentAsType<List<SubscriberViewModel>>();
        }
        public async System.Threading.Tasks.Task<SubscriberViewModel> GetActiveSubscriberByUserId(string UserId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Subscribers/getactivesubscriberbyuser?UserId={UserId}");
            return resp.ContentAsType<SubscriberViewModel>();
        }
        public async System.Threading.Tasks.Task<SubscriberViewModel> GetLastSubscriberByUser(string UserId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Subscribers/getlastsubscriberbyuser?UserId={UserId}");
            return resp.ContentAsType<SubscriberViewModel>();
        }
        public async System.Threading.Tasks.Task<bool> IsSubscriber(string UserId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Subscribers/issubscriber?UserId={UserId}");
            return resp.ContentAsType<bool>();
        }
        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutSubscriber(string id, Subscriber Subscriber)
        {
            return await HttpRequestFactory.Put($"{this.ApiUrl}/Subscribers/{id}", Subscriber);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostSubscriber(Subscriber Subscriber)
        {
            return await HttpRequestFactory.Post($"{this.ApiUrl}/Subscribers/", Subscriber);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteSubscriber(string id)
        {
            return await HttpRequestFactory.Delete($"{this.ApiUrl}/Subscribers/{id}");
        }

        public async System.Threading.Tasks.Task<SubscriberViewModel> GetSubscriber(string id)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Subscribers/{id}");
            return resp.ContentAsType<SubscriberViewModel>();
        }
    }
}
