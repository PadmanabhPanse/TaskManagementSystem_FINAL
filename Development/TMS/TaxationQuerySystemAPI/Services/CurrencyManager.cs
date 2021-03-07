using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRestCountries;
using FixerSharp;
using CurrencyLayer4NET;
using Microsoft.Extensions.Configuration;

namespace TaxationQuerySystemAPI.Services
{
    public class CurrencyManager
    {
        readonly IConfiguration _configuration;

        public CurrencyManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<string>> GetCountries()
        {
            return (await RestCountry.AllAsync()).Select(c => c.Name).ToList();
        }

        public async Task<List<CurrencyLayer4NET.Entities.Currency>> GetCurrencies()
        {
            return await Task.Run(() =>
            {
                CLClient client = new CLClient(_configuration.GetValue<string>("CurrencyLayer:APIkey"));
                return client.GetCurrencies().ToList();
            });
       
        }

        public async Task<double> ConvertCurrency(string fromCurrency, string toCurrency, double amount)
        {
            Fixer.SetApiKey(_configuration.GetValue<string>("Fixer:APIkey"));
            return await Fixer.ConvertAsync(fromCurrency, toCurrency, amount);
        }
    }
}
