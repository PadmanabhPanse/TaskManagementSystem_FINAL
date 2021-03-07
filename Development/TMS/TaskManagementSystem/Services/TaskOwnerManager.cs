using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class TaskOwnerManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public TaskOwnerManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region TaskOwners
        public async System.Threading.Tasks.Task<List<TaskOwner>> GetTaskOwners(TaskOwnerSearchModel model)
        {
            var resp = await HttpRequestFactory.Post(this.ApiUrl + "/TaskOwners/gettaskowners", model);
            return resp.ContentAsType<List<TaskOwner>>();
        }

        public async System.Threading.Tasks.Task<TaskOwner> GetTaskOwner(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/TaskOwners/{id}");
            return resp.ContentAsType<TaskOwner>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutTaskOwner(long id, TaskOwner taskOwner)
        {
            return await HttpRequestFactory.Put(this.ApiUrl + $"/TaskOwners/{id}", taskOwner);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostTaskOwner(TaskOwner taskOwner)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/TaskOwners/", taskOwner);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteTaskOwner(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/TaskOwners/{id}");
        }
        #endregion
    }
}
