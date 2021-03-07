using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystem.HttpClient;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class EmailManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }


        #region "Email"

        public async Task<List<Email>> GetEmails()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/Emails/");
            return resp.ContentAsType<List<Email>>();
        }

        public async Task<Email> GetEmail(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Emails/{id}");
            return resp.ContentAsType<Email>();
        }

        public async System.Threading.Tasks.Task PutEmail(long id, Email Email)
        {
            var resp = await HttpRequestFactory.Put(this.ApiUrl + $"/Emails/{id}", Email);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostEmail(Email Email)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/Emails/", Email);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteEmail(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Emails/{id}");
        }

        #endregion
    }
}
