using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Services
{
    public interface INotificationSender
    {
        Task<List<Twilio.Rest.Api.V2010.Account.MessageResource>> SendSMS(List<string> Recipients, string Body);
        Task<Task> SendEmail(Tuple<string, string> From, List<Tuple<string, string>> Recipients, string Subject, string Body, List<string> Attachments);
    }
}
