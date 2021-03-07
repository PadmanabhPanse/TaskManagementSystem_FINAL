using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystem.HttpClient;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class SmsManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public SmsManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }


        #region "Sms"

        public async Task<List<Sms>> GetSmss()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/Sms/");
            return resp.ContentAsType<List<Sms>>();
        }

        public async Task<Sms> GetSms(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Sms/{id}");
            return resp.ContentAsType<Sms>();
        }

        public async System.Threading.Tasks.Task PutSms(long id, Sms Sms)
        {
            var resp = await HttpRequestFactory.Put(this.ApiUrl + $"/Sms/{id}", Sms);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostSms(Sms Sms)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/Sms/", Sms);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteSms(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Sms/{id}");
        }

        #endregion
    }
}
