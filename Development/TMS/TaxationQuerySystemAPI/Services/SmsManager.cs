using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using Microsoft.AspNetCore.Hosting;

namespace TaxationQuerySystemAPI.Services
{
    public class SmsManager:XmlFileHandler<List<Sms>>
    {
        public SmsManager(IHostingEnvironment environment) : base(environment, "ArrayOfSms", "sms.xml")
        {

        }

        public void AddSms(Sms newSms)
        {
            xmlObjects.Add(newSms);
        }

        public void EditSms(Sms updatedSms)
        {
            int index = xmlObjects.FindIndex(m => m.Id == updatedSms.Id);
            Sms[] SmsArr = xmlObjects.ToArray();
            SmsArr[index] = updatedSms;
            xmlObjects = SmsArr.ToList();
        }

        public void RemoveSms(Sms Sms)
        {
            xmlObjects.Remove(Sms);
        }
    }
}
