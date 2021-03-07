using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class DashboardManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public DashboardManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        public async System.Threading.Tasks.Task<Dashboard> GetDashboardData(string UserId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Home/dashboarddata?UserId={UserId}");
            return resp.ContentAsType<Dashboard>();
        }

        public async System.Threading.Tasks.Task<Dashboard> GetDashboardData(string qs, long TaskOwnerId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Home/dashboarddata?{qs}={TaskOwnerId}");
            return resp.ContentAsType<Dashboard>();
        }

        public async System.Threading.Tasks.Task<Dashboard> GetDashboardData()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Home/dashboarddata");
            return resp.ContentAsType<Dashboard>();
        }
    }
}
