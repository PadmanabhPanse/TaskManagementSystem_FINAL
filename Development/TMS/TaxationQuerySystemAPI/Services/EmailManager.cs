using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;
using Microsoft.AspNetCore.Hosting;

namespace TaxationQuerySystemAPI.Services
{
    public class EmailManager:XmlFileHandler<List<Email>>
    {
        public EmailManager(IHostingEnvironment environment) : base(environment, "ArrayOfEmail", "email.xml")
        {

        }

        public void AddEmail(Email newEmail)
        {
            xmlObjects.Add(newEmail);
        }

        public void EditEmail(Email updatedEmail)
        {
            int index = xmlObjects.FindIndex(m => m.Id == updatedEmail.Id);
            Email[] EmailArr = xmlObjects.ToArray();
            EmailArr[index] = updatedEmail;
            xmlObjects = EmailArr.ToList();
        }

        public void RemoveEmail(Email Email)
        {
            xmlObjects.Remove(Email);
        }
    }
}
