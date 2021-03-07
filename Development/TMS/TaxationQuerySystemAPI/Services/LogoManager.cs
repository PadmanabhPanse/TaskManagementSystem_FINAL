using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class LogoManager : XmlFileHandler<Logo>
    {
        public LogoManager(IHostingEnvironment environment) : base(environment, "Logo", "logo.xml")
        {

        }
    }
}
