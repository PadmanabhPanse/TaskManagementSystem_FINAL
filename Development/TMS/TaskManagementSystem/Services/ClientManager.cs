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
    public class ClientManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public ClientManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region Clients
        public async System.Threading.Tasks.Task<List<Client>> GetClients(SearchClient model)
        {
            var resp = await HttpRequestFactory.Post(this.ApiUrl + "/Clients/getclients",model);
            return resp.ContentAsType<List<Client>>();
        }

        public async System.Threading.Tasks.Task<Client> GetClient(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/Clients/{id}");
            return resp.ContentAsType<Client>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutClient(long id, Client client)
        {
            return await HttpRequestFactory.Put(this.ApiUrl + $"/Clients/{id}", client);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostClient(Client client)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/Clients/", client);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteClient(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/Clients/{id}");
        }
        #endregion
    }
}
