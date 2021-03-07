using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxationQuerySystemAPI.Services;
namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        readonly CurrencyManager _currencyManager;
        public CurrencyController(CurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        [HttpGet("getcountries")]
        public async Task<List<string>> GetCountries()
        {
            return await _currencyManager.GetCountries();
        }

        [HttpGet("getcurrencies")]
        public async Task<List<CurrencyLayer4NET.Entities.Currency>> GetCurrencies()
        {
            return await _currencyManager.GetCurrencies();
        }

        [HttpGet("convertcurrency")]
        public async Task<double> ConvertCurrency([FromQuery] string fromCurrency, [FromQuery] string toCurrency, [FromQuery] double amount)
        {
            return await _currencyManager.ConvertCurrency(fromCurrency, toCurrency, amount);
        }
    }
}
