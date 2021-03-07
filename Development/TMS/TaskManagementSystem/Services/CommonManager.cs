using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class CommonManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public CommonManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        public async System.Threading.Tasks.Task<List<TaskOwner>> GetTaskOwners()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Common/gettaskowners");
            return resp.ContentAsType<List<TaskOwner>>();
        }

        public async System.Threading.Tasks.Task<List<Models.Task>> GetTasks()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Common/gettasks");
            return resp.ContentAsType<List<Models.Task>>();
        }

        public async System.Threading.Tasks.Task<List<Client>> GetClients()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Common/getclients");
            return resp.ContentAsType<List<Client>>();
        }

        public async System.Threading.Tasks.Task<List<string>> GetCountries()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Currency/getcountries");
            return resp.ContentAsType<List<string>>();
        }

        public async System.Threading.Tasks.Task<List<CurrencyLayer4NET.Entities.Currency>> GetCurrencies()
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Currency/getcurrencies");
            return resp.ContentAsType<List<CurrencyLayer4NET.Entities.Currency>>();
        }

        public async System.Threading.Tasks.Task<double> ConvertCurrency(string fromCurrency,string toCurrency, double amount)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Currency/convertcurrency?fromCurrency={fromCurrency}&toCurrency={toCurrency}&amount={amount}");
            return resp.ContentAsType<double>();
        }

    }
}
