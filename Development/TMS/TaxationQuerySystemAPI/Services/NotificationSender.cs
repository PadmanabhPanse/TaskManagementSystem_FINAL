using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Twilio.Rest.Api.V2010.Account;

namespace TaxationQuerySystemAPI.Services
{
    public class NotificationSender : INotificationSender
    {
        public IConfiguration _configuration { get; }

        public NotificationSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<MessageResource>> SendSMS(List<string> Recipients, string Body)
        {
            string accountSid = _configuration.GetValue<string>("SMSConfig:accountSid");
            string authToken = _configuration.GetValue<string>("SMSConfig:authToken");
            List<MessageResource> messageResources = new List<MessageResource>();
            TwilioClient.Init(accountSid, authToken);
            if (Recipients != null && Recipients.Count > 0)
            {
                foreach (var recipient in Recipients.Distinct())
                {
                    messageResources.Add(await MessageResource.CreateAsync(
                        body: Body,
                        from: new Twilio.Types.PhoneNumber(_configuration.GetValue<string>("SMSConfig:Originator")),
                        to: new Twilio.Types.PhoneNumber(recipient)));
                }
            }
            else
            {
                throw new Exception("Provide Recepient List");
            }
            return messageResources;
        }

        public async Task<Task> SendEmail(Tuple<string, string> From, List<Tuple<string, string>> Recipients, string Subject, string Body, List<string> Attachments)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress(From.Item1, From.Item2);
            message.From.Add(from);

            if (Recipients != null && Recipients.Count > 0)
            {
                foreach (var recipient in Recipients.Distinct())
                {
                    MailboxAddress to = new MailboxAddress(recipient.Item1, recipient.Item2);
                    message.To.Add(to);
                }
            }
            else
            {
                throw new Exception("Provide Recepient List");
            }

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = Body;

            if (Attachments != null && Attachments.Count > 0)
            {
                foreach (var attachment in Attachments)
                {
                    bodyBuilder.Attachments.Add(attachment);
                }
            }
            message.Subject = Subject;
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect(_configuration.GetValue<string>("SMTPConfig:Host"), _configuration.GetValue<int>("SMTPConfig:Port"), true);
            client.Authenticate(_configuration.GetValue<string>("SMTPConfig:Username"), _configuration.GetValue<string>("SMTPConfig:Password"));
            await client.SendAsync(message);
            client.Disconnect(true);
            client.Dispose();
            return Task.CompletedTask;
        }
    }
}