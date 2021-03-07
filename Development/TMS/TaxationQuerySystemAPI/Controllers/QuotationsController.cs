using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxationQuerySystemAPI.Models;
using TaxationQuerySystemAPI.Models.FilterModels;
using TaxationQuerySystemAPI.Models.ResponseModels;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationsController : ControllerBase
    {
        readonly NotificationManager _notifier;
        readonly TaskManager _taskManager;
        readonly TMSDBContext _context;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SubscriberManager _subscriberManager;
        readonly UserRoleManager _userRoleManager;

        public QuotationsController(
            TMSDBContext context,
            UserManager<ApplicationUser> userManager,
            UserRoleManager userRoleManager,
            TaskManager taskManager,
            NotificationManager notifier,
            SubscriberManager subscriberManager
            )
        {
            _context = context;
            _userManager = userManager;
            _userRoleManager = userRoleManager;
            _taskManager = taskManager;
            _notifier = notifier;
            _subscriberManager = subscriberManager;
        }

        // GET: api/Quotations
        [HttpPost("getquotations")]
        public async Task<IEnumerable<QuotationViewModel>> GetQuotations(SearchQuotation model)
        {
            IQueryable<Models.Quotation> quotations = _context.Quotations;
            var SearchFieldMutators = new List<SearchFieldMutator<Models.Quotation, SearchQuotation>>();

            if (model != null)
            {
                SearchFieldMutators.AddRange(new SearchFieldMutator<Models.Quotation, SearchQuotation>[] {
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => c.QuotationDate.HasValue && c.QuotationDate.Value.Date!=DateTime.MinValue.Date,(list,c)=>list.Where(item=>item.QuotationDate.Date==c.QuotationDate.Value.Date)),
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => c.ConversionDate.HasValue && c.ConversionDate.Value.Date != DateTime.MinValue.Date ,(list,c)=>list.Where(item=>item.ConversionDate.Date==c.ConversionDate.Value.Date)),
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => !string.IsNullOrEmpty(c.Title),(list,c)=>list.Where(item => item.Title.Contains(c.Title))),
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => !string.IsNullOrEmpty(c.QuoteStatus), (list, c) => list.Where(item => string.Compare(item.QuoteStatus, c.QuoteStatus) == 0)),
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => !string.IsNullOrEmpty(c.UserId), (list, c) => list.Where(item => string.Compare(item.UserId, c.UserId) == 0)),
                        new SearchFieldMutator<Models.Quotation, SearchQuotation>(c => c.TotalCost.HasValue && c.TotalCost.Value>0,(list,c)=>list.Where(item =>  item.TotalCost==c.TotalCost.Value)),
                   });

                foreach (var filter in SearchFieldMutators)
                {
                    quotations = filter.Apply(model, quotations);
                }
            }

            return (await quotations.OrderByDescending(o => o.QuoteId).ToListAsync()).Select(q => GetQuotationVMObject(q));
        }
        [HttpGet("getuserquotation")]
        public async Task<IActionResult> GetUserQuotation([FromQuery] string UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var quotation = await _context.Quotations.SingleOrDefaultAsync(q => String.Compare(q.UserId, UserId) == 0);

            if (quotation == null)
            {
                return NotFound();
            }
            quotation.tasks = await _context.QuoteTasks.Where(o => o.QuoteId == quotation.QuoteId)?.ToListAsync();
            return Ok(GetQuotationVMObject(quotation));
        }
        // GET: api/Quotations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotation([FromRoute] long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var quotation = await _context.Quotations.FindAsync(id);

                if (quotation == null)
                {
                    return NotFound();
                }
                quotation.tasks = await _context.QuoteTasks.Where(o => o.QuoteId == id)?.ToListAsync();
                return Ok(GetQuotationVMObject(quotation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/Quotations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuotation([FromRoute] long id, [FromBody] QuotationViewModel quotationVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotationVM.QuoteId)
            {
                return BadRequest();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Quotation currentQuotation = await _context.Quotations.SingleOrDefaultAsync(q => q.QuoteId == quotationVM.QuoteId);
                    _context.Entry(currentQuotation).State = EntityState.Detached;

                    Quotation quotation = GetQuotationObject(quotationVM);
                    _context.Entry(quotation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    if (quotationVM.tasks != null && quotationVM.tasks.Any(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached))
                    {
                        await this.UpdateQuoteTasks(quotationVM.tasks.Where(t => t.ChangeState != EntityState.Unchanged || t.ChangeState != EntityState.Detached).ToList(), quotation);
                    }


                    if (string.Compare(currentQuotation.QuoteStatus, quotationVM.QuoteStatus) != 0)
                    {
                        User admin = _userRoleManager.getAdmin();
                        ApplicationUser user = await _userManager.FindByIdAsync(quotation.UserId);
                        Subscriber subscriber = await _subscriberManager.GetActiveSubscriberByUser(quotation.UserId);
                        if (string.Compare(quotationVM.QuoteStatus, "CostAccepted") == 0)
                        {
                            TaskNotificationSetting setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "QuotationCostAccepted") == 0);
                            var newNotification = new TaskNotification
                            {
                                SettingId = setting.SettingId,
                                Description = setting.TaskChange
                                .Replace("{Quotation}", quotation.Title)
                                .Replace("{UserType}", subscriber != null ? "Subscriber" : "QuoteUser")
                                .Replace("{UserName}", user.FirstName),
                                NotificationDate = DateTime.Now,
                                SmsTime = DateTime.Now,
                                EmailTime = DateTime.Now,
                                PopupDate = DateTime.Now,
                                IsRead = !setting.Dashboard,
                                ObjectId = quotation.QuoteId.ToString(),
                                ObjectType = "Quotation",
                                UserId = admin.UserId,
                                OwnerId = admin.OwnerId
                            };
                            await _notifier.PostNotification(newNotification);

                            await _notifier.SendNotification(
                                setting,
                                newNotification,
                                string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                                new Tuple<string, string>(user.UserName, user.Email),
                                new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) });
                        }
                        string[] quotationProposalStatuses = new string[] { "ProposalAccepted", "ProposalRejected" };
                        if (quotationProposalStatuses.Contains(quotationVM.QuoteStatus))
                        {
                            var desc = "";
                            if (string.Compare(quotationVM.QuoteStatus, "ProposalAccepted") == 0)
                            {
                                desc = "Your task has been accepted please review quotation and accept.";
                            }
                            else
                            {
                                desc = "Your task has been rejected.";
                            }
                            TaskNotificationSetting setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "QuotationProposalAccepted") == 0);
                            var newNotification = new TaskNotification
                            {
                                SettingId = setting.SettingId,
                                Description = desc,
                                NotificationDate = DateTime.Now,
                                SmsTime = DateTime.Now,
                                EmailTime = DateTime.Now,
                                PopupDate = DateTime.Now,
                                IsRead = !setting.Dashboard,
                                ObjectId = quotation.QuoteId.ToString(),
                                ObjectType = "Quotation",
                                UserId = user.Id,
                                OwnerId = admin.OwnerId
                            };
                            await _notifier.PostNotification(newNotification);
                            await _notifier.SendNotification(
                                setting,
                                newNotification,
                                string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                                new Tuple<string, string>(admin.UserName, admin.Email),
                                new List<Tuple<string, string>> { new Tuple<string, string>(user.FirstName, user.Email) });
                        }
                    }

                    transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    if (!QuotationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // POST: api/Quotations
        [HttpPost]
        public async Task<IActionResult> PostQuotation([FromBody] QuotationViewModel quotationVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    TaskNotificationSetting setting = null;
                    TaskNotification newNotification = null;
                    User admin = _userRoleManager.getAdmin();
                    ApplicationUser user = await _userManager.FindByIdAsync(quotationVM.UserId);
                    Subscriber subscriber = await _subscriberManager.GetActiveSubscriberByUser(quotationVM.UserId);

                    if (subscriber == null && !_context.Quotations.Any(q => string.Compare(q.UserId, quotationVM.UserId) == 0))
                    {
                        setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "NewQuoteUserRegistration") == 0);
                        newNotification = new TaskNotification
                        {
                            SettingId = setting.SettingId,
                            Description = setting.TaskChange.Replace("{QuoteUser}", user.FirstName),
                            NotificationDate = DateTime.Now,
                            SmsTime = DateTime.Now,
                            EmailTime = DateTime.Now,
                            PopupDate = DateTime.Now,
                            IsRead = !setting.Dashboard,
                            ObjectId = quotationVM.UserId,
                            ObjectType = "QuoteUser",
                            UserId = admin.UserId,
                            OwnerId = admin.OwnerId
                        };
                        await _notifier.PostNotification(newNotification);

                        await _notifier.SendNotification(
                            setting,
                            newNotification,
                            string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                            new Tuple<string, string>(user.UserName, user.Email),
                            new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) });
                    }
                    Quotation quotation = GetQuotationObject(quotationVM);
                    _context.Quotations.Add(quotation);
                    await _context.SaveChangesAsync();
                    if (quotationVM.tasks != null && quotationVM.tasks.Any(t => t.ChangeState == EntityState.Added))
                    {
                        await this.UpdateQuoteTasks(quotationVM.tasks.Where(t => t.ChangeState == EntityState.Added).ToList(), quotation);
                    }

                    setting = await _context.NotificationSettings.FirstOrDefaultAsync(ns => string.Compare(ns.Type, "QuotationAdded") == 0);

                    newNotification = new TaskNotification
                    {
                        SettingId = setting.SettingId,
                        Description = setting.TaskChange
                        .Replace("{UserType}", subscriber != null ? "Subscriber" : "QuoteUser")
                        .Replace("{UserName}", user.FirstName),
                        NotificationDate = DateTime.Now,
                        SmsTime = DateTime.Now,
                        EmailTime = DateTime.Now,
                        PopupDate = DateTime.Now,
                        IsRead = !setting.Dashboard,
                        ObjectId = quotation.QuoteId.ToString(),
                        ObjectType = "Quotation",
                        UserId = admin.UserId,
                        OwnerId = admin.OwnerId
                    };
                    await _notifier.PostNotification(newNotification);

                    newNotification = new TaskNotification
                    {
                        SettingId = setting.SettingId,
                        Description = "Quotation Created Successfully",
                        NotificationDate = DateTime.Now,
                        SmsTime = DateTime.Now,
                        EmailTime = DateTime.Now,
                        PopupDate = DateTime.Now,
                        IsRead = !setting.Dashboard,
                        ObjectId = quotation.QuoteId.ToString(),
                        ObjectType = "Quotation",
                        UserId = user.Id,
                        OwnerId = admin.OwnerId
                    };
                    await _notifier.PostNotification(newNotification);
                    await _notifier.SendNotification(
                        setting,
                        newNotification,
                        string.IsNullOrEmpty(admin.PhoneNumber) ? null : new List<string> { admin.PhoneNumber },
                        new Tuple<string, string>(user.UserName, user.Email),
                        new List<Tuple<string, string>> { new Tuple<string, string>(admin.UserName, admin.Email) });

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return Ok(quotationVM);
        }

        // DELETE: api/Quotations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation([FromRoute] long id)
        {
            Quotation quotation = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    quotation = await _context.Quotations.FindAsync(id);
                    if (quotation == null)
                    {
                        return NotFound();
                    }

                    quotation.tasks = _context.QuoteTasks.Where(d => d.QuoteId == id).ToList();
                    if (quotation.tasks != null && quotation.tasks.Any())
                    {
                        foreach (var task in quotation.tasks)
                        {
                            QuoteTask quoteTask = task;
                            _context.QuoteTasks.Remove(quoteTask);
                        }
                    }

                    _context.Quotations.Remove(quotation);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return Ok(quotation);
        }

        private bool QuotationExists(long id)
        {
            return _context.Quotations.Any(e => e.QuoteId == id);
        }
        private Models.Quotation GetQuotationObject(QuotationViewModel quotationViewModel)
        {
            return new Models.Quotation
            {
                UserId = quotationViewModel.UserId,
                QuoteId = quotationViewModel.QuoteId,
                ConversionDate = quotationViewModel.ConversionDate,
                Description = quotationViewModel.Description,
                QuotationDate = quotationViewModel.QuotationDate,
                QuoteStatus = quotationViewModel.QuoteStatus,
                RejectReason = quotationViewModel.RejectReason,
                Title = quotationViewModel.Title,
                TaxRate=quotationViewModel.TaxRate,
                TaxType=quotationViewModel.TaxType,
                Country=quotationViewModel.Country,
                Currency=quotationViewModel.Currency,
                TotalCost = quotationViewModel.TotalCost,
                tasks = null
            };
        }
        private Models.ResponseModels.QuotationViewModel GetQuotationVMObject(Models.Quotation quotation)
        {
            ApplicationUser user = _userManager.FindByIdAsync(quotation.UserId).Result;
            return new Models.ResponseModels.QuotationViewModel
            {
                ConversionDate = quotation.ConversionDate,
                Description = quotation.Description,
                QuotationDate = quotation.QuotationDate,
                QuoteId = quotation.QuoteId,
                QuoteStatus = quotation.QuoteStatus,
                RejectReason = quotation.RejectReason,
                Title = quotation.Title,
                TaxRate = quotation.TaxRate,
                TaxType = quotation.TaxType,
                Country = quotation.Country,
                Currency = quotation.Currency,
                TotalCost = quotation.TotalCost,
                UserId = quotation.UserId,
                User = $"{user.FirstName} {user.LastName} ({user.UserName})",
                tasks = getQuotationTaskVMs(quotation.tasks, quotation.QuoteId, EntityState.Unchanged)
            };
        }
        private List<QuoteTaskViewModel> getQuotationTaskVMs(List<QuoteTask> quoteTasks, long id, EntityState entityState)
        {
            if (quoteTasks != null && quoteTasks.Any())
            {
                List<QuoteTaskViewModel> taskVMs = new List<QuoteTaskViewModel>();
                foreach (var quotetask in quoteTasks)
                {
                    QuoteTaskViewModel taskReview = new QuoteTaskViewModel
                    {
                        ChangeState = entityState,
                        QuoteId = id,
                        Cost = quotetask.Cost,
                        TaskId = quotetask.TaskId,
                        TaskName = quotetask.TaskName,
                        TaskTitle = quotetask.TaskTitle,
                    };
                    taskVMs.Add(taskReview);
                }

                return taskVMs;
            }
            return new List<QuoteTaskViewModel>();
        }
        private async Task<IActionResult> UpdateQuoteTasks(List<Models.ResponseModels.QuoteTaskViewModel> quoteTaskVMs, Models.Quotation quotation)
        {
            try
            {
                if (quoteTaskVMs != null && quoteTaskVMs.Any())
                {
                    foreach (var quotetaskvm in quoteTaskVMs.Where(list => list.ChangeState != EntityState.Unchanged || list.ChangeState != EntityState.Detached))
                    {
                        QuoteTask quoteTask = new QuoteTask
                        {
                            Cost = quotetaskvm.Cost,
                            Quotation = quotation,
                            QuoteId = quotetaskvm.QuoteId,
                            TaskId = quotetaskvm.TaskId,
                            TaskName = quotetaskvm.TaskName,
                            TaskTitle = quotetaskvm.TaskTitle,
                        };
                        _context.Entry(quoteTask).State = quotetaskvm.ChangeState;
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id}/closequotation")]
        public async Task<IActionResult> CloseQuotation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == 0)
            {
                return BadRequest();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var quotation = await _context.Quotations.FindAsync(id);

                    if (quotation == null)
                    {
                        return NotFound();
                    }
                    if (string.Compare(quotation.QuoteStatus, "Closed") == 0)
                    {
                        return BadRequest("Quotation already Closed");
                    }
                    quotation.tasks = await _context.QuoteTasks.Where(o => o.QuoteId == id)?.ToListAsync();
                    if (quotation.tasks != null && quotation.tasks.Any())
                    {
                        var Admin = (from _user in _context.Users
                                     join _userrole in _context.UserRoles on _user.UserId equals _userrole.UserId
                                     join _role in _context.Roles on _userrole.RoleId equals _role.RoleId
                                     join _owner in _context.TaskOwners on _user.UserId equals _owner.UserId
                                     where string.Compare(_role.RoleName, "admin") == 0
                                     select new User { UserId = _user.UserId, UserName = _user.UserName, Email = _user.Email, PhoneNumber = _user.PhoneNumber, OwnerId = _owner.TaskOwnerId })
                                                .SingleOrDefault();
                        long maxPriority = (await _taskManager.GetTasks(new TaskSearchModel { TaskOwnerId = Admin.OwnerId })).Max(t => t.TaskPriority.HasValue ? t.TaskPriority.Value : 0);
                        foreach (var qtask in quotation.tasks)
                        {
                            maxPriority = maxPriority + 1;
                            Models.Task newtask = new Models.Task
                            {
                                TaskName = qtask.TaskName,
                                TaskTitle = qtask.TaskTitle,
                                UserId = quotation.UserId,
                                TaskOwnerId = Admin.OwnerId,
                                TaskStaffId = Admin.OwnerId,
                                TaskStatusId = "Waiting",
                                TaskAdminInstructions = string.Empty,
                                TaskOwner = "Admin",
                                TaskPriority = maxPriority,
                                TaskThresholdDate = DateTime.Now,
                                TaskStartDate = DateTime.Now,
                                TaskEstimateTime = DateTime.Now,
                            };
                            _context.Tasks.Add(newtask);
                            await _context.SaveChangesAsync();
                        }
                        quotation.QuoteStatus = "Closed";
                        quotation.ConversionDate = DateTime.Now;

                        var user = await _userManager.FindByIdAsync(quotation.UserId);
                        if (await _userManager.IsInRoleAsync(user, "QuoteUser"))
                        {
                            await _userManager.RemoveFromRoleAsync(user, "QuoteUser");
                            await _userManager.AddToRoleAsync(user, "User");
                        }
                        if (await _userManager.IsInRoleAsync(user, "User") && await _subscriberManager.IsSubscriber(quotation.UserId))
                        {
                            Subscriber subscriber = await _subscriberManager.GetActiveSubscriberByUser(quotation.UserId);
                            if (subscriber != null)
                            {
                                decimal newBalanceAmount = subscriber.BalanceAmount - quotation.TotalCost;
                                if (newBalanceAmount >= 0)
                                {
                                    subscriber.BalanceAmount = newBalanceAmount;
                                }
                                if (newBalanceAmount <= subscriber.ThresholdPrice)
                                {
                                    subscriber.IsLocked = true;
                                }
                                await _subscriberManager.PutSubscriber(subscriber.SubscriberId, subscriber);
                            }
                            else
                            {
                                throw new Exception("No Active Subscription");
                            }
                        }
                        
                    }
                    else
                    {
                        quotation.QuoteStatus = "Incomplete";
                    }

                    _context.Entry(quotation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (!QuotationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            return NoContent();
        }
    }
}