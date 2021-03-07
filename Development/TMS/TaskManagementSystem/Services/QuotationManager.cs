using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class QuotationManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public QuotationManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region Tasks

        public async System.Threading.Tasks.Task<List<Quotation>> GetQuotations(SearchQuotation model)
        {
            var resp = await HttpRequestFactory.Post($"{this.ApiUrl}/Quotations/getquotations", model);
            return resp.ContentAsType<List<Quotation>>();
        }
        public async System.Threading.Tasks.Task<Quotation> GetUserQuotation(string UserId)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Quotations/getuserquotation?UserId=" + UserId);
            return resp.ContentAsType<Quotation>();
        }

        public async System.Threading.Tasks.Task<Quotation> GetQuotation(long id)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Quotations/{id}");
            return resp.ContentAsType<Quotation>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutQuotation(long id, Quotation quotation)
        {
            return await HttpRequestFactory.Put($"{this.ApiUrl}/Quotations/{id}", quotation);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostQuotation(Quotation quotation)
        {
            return await HttpRequestFactory.Post($"{this.ApiUrl}/Quotations/", quotation);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteQuotation(long id)
        {
            return await HttpRequestFactory.Delete($"{this.ApiUrl}/Quotations/{id}");
        }
        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> CloseQuotation(long id)
        {
            return await HttpRequestFactory.Put($"{this.ApiUrl}/Quotations/{id}/closequotation",null);
        }
        #endregion  
    }
}
