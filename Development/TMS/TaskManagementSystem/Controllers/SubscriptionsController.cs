using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("subscriptions")]
    public class SubscriptionsController : Controller
    {
        readonly SubscriptionManager _subscriptionManager;
        readonly IHostingEnvironment _environment;
        public SubscriptionsController(SubscriptionManager subscriptionManager, IHostingEnvironment environment)
        {
            _subscriptionManager = subscriptionManager;
            _environment = environment;
        }
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _subscriptionManager.GetSubscriptions());
        }
        [Route("create")]
        public IActionResult Create()
        {
            var subscription = new Subscription();
            var queryTypeSelectList = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(QueryType)))
            {
                queryTypeSelectList.Add(new SelectListItem(Enum.GetName(typeof(QueryType), item), item.ToString(), item.ToString() == subscription.queryType.ToString()));
            }
            ViewBag.QueryTypes = queryTypeSelectList;
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subscription model)
        {
            if (ModelState.IsValid)
            {
                model.TotalCost = this.CalculateTotalCost(model);
                await _subscriptionManager.PostSubscription(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        private decimal CalculateTotalCost(Subscription model)
        {
            decimal TotalCost = (model.Credits * model.CostPerCredit) + (model.CostPerQuery * Convert.ToDecimal((new int[] { model.queryRange?.Min ?? 0, model.queryRange?.Max ?? 0 }).Average()));
            decimal TaxedCost = (TotalCost * (model.taxRate / 100));
            return TotalCost + TaxedCost;
        }
        // GET: Menu/Edit/5
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _subscriptionManager.GetSubscription(id.Value);
            if (subscription == null)
            {
                return NotFound();
            }

            var queryTypeSelectList = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(QueryType)))
            {
                queryTypeSelectList.Add(new SelectListItem(Enum.GetName(typeof(QueryType), item), item.ToString(), item.ToString() == subscription.queryType.ToString()));
            }
            ViewBag.QueryTypes = queryTypeSelectList;

            return View(subscription);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Subscription model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model.TotalCost = this.CalculateTotalCost(model);
                await _subscriptionManager.PutSubscription(id, model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _subscriptionManager.GetSubscription(id.Value);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _subscriptionManager.DeleteSubscription(id);
            return RedirectToAction(nameof(Index));
        }
        [Route("view")]
        [HttpGet]
        public async Task<IActionResult> ViewSubscriptions()
        {
            var subscriptions = await _subscriptionManager.GetSubscriptions();
            return View(subscriptions);
        }
    }
}