using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class BlogPageManager : XmlFileHandler<Blog>
    {
        public BlogPageManager(IHostingEnvironment environment) : base(environment,"Blog", "blogpage.xml")
        {

        }
    }
}
