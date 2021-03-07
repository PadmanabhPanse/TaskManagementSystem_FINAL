using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystem.HttpClient;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class SubscriptionManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public SubscriptionManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }


        #region "Subscription"

        public async Task<List<Subscription>> GetSubscriptions()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/Subscriptions/");
            return resp.ContentAsType<List<Subscription>>();
        }

        public async Task<Subscription> GetSubscription(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Subscriptions/{id}");
            return resp.ContentAsType<Subscription>();
        }

        public async System.Threading.Tasks.Task PutSubscription(long id, Subscription subscription)
        {
            var resp = await HttpRequestFactory.Put(this.ApiUrl + $"/Subscriptions/{id}", subscription);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostSubscription(Subscription subscription)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/Subscriptions/", subscription);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteSubscription(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Subscriptions/{id}");
        }

        #endregion
    }
}
