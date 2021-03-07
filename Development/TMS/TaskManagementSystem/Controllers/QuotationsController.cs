using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("quotations")]
    public class QuotationsController : Controller
    {
        private readonly QuotationManager _manager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public QuotationsController(QuotationManager manager, IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("getquotations")]
        public async Task<IActionResult> getquotations(SearchQuotation model)
        {
            if (User.IsInRole("User"))
            {
                model.UserId = Session.GetString("UserId");
            }
            return Json(await _manager.GetQuotations(model));
        }
        [HttpGet("getuserquotation")]
        public async Task<IActionResult> getuserquotation()
        {
            return Json(await _manager.GetUserQuotation(Session.GetString("UserId")));
        }
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("editquotation")]
        public async Task<IActionResult> EditQuotation(long? id)
        {
            return Json(await _manager.GetQuotation(id.Value));
        }
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.Quotation model)
        {
            if (model.QuoteId == 0)
            {
                await _manager.PostQuotation(model);
            }
            else
            {
                await _manager.PutQuotation(model.QuoteId, model);
            }
            return NoContent();
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            Models.Quotation quotation = new Models.Quotation();
            if (id.HasValue)
            {
                quotation = await _manager.GetQuotation(id.Value);
                if (quotation == null)
                {
                    return NotFound();
                }

                await _manager.DeleteQuotation(id.Value);
            }
            return NoContent();
        }
        [Route("closequotation")]
        [HttpGet]
        public async Task<IActionResult> CloseQuotation(long? id)
        {
            Models.Quotation quotation = new Models.Quotation();
            if (id.HasValue)
            {
                quotation = await _manager.GetQuotation(id.Value);
                if (quotation == null)
                {
                    return NotFound();
                }

                await _manager.CloseQuotation(id.Value);
            }
            return NoContent();
        }

    }
}
