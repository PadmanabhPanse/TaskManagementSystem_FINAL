using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TaskManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception tiex)
            {
                // Throw the new exception
                throw tiex.InnerException;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           // (IWebHostBuilder)WebHost.CreateDefaultBuilder(args)
           //.UseKestrel()
           //.UseContentRoot(Directory.GetCurrentDirectory())
           //.UseUrls("http://*:5001")
           //.UseStartup<Startup>()
           //.Build();

           WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>();
    }
}
