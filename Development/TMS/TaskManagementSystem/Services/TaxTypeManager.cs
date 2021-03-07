using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystem.HttpClient;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class TaxTypeManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public TaxTypeManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }


        #region "TaxType"

        public async Task<List<TaxType>> GetTaxTypes()
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + "/TaxTypes/");
            return resp.ContentAsType<List<TaxType>>();
        }

        public async Task<TaxType> GetTaxType(long id)
        {
            var resp = await HttpRequestFactory.Get(this.ApiUrl + $"/TaxTypes/{id}");
            return resp.ContentAsType<TaxType>();
        }

        public async System.Threading.Tasks.Task PutTaxType(long id, TaxType taxtype)
        {
            var resp = await HttpRequestFactory.Put(this.ApiUrl + $"/TaxTypes/{id}", taxtype);
        }

        public async Task<System.Net.Http.HttpResponseMessage> PostTaxType(TaxType taxtype)
        {
            return await HttpRequestFactory.Post(this.ApiUrl + "/TaxTypes/", taxtype);
        }

        public async Task<System.Net.Http.HttpResponseMessage> DeleteTaxType(long id)
        {
            return await HttpRequestFactory.Delete(this.ApiUrl + $"/TaxTypes/{id}");
        }

        #endregion
    }
}
