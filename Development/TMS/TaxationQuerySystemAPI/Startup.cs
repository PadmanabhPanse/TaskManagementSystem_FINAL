using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaxationQuerySystemAPI.Services;

namespace TaxationQuerySystemAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TMSDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TMSContextConnection"));
            })
            .AddDbContext<TMSIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TMSContextConnection"));
            })
            .AddIdentity<Models.ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TMSIdentityContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<SiteMapManager>()
                    .AddSingleton<BlogPageManager>()
                    .AddSingleton<LogoManager>()
                    .AddSingleton<MediaManager>()
                    .AddSingleton<EmailManager>()
                    .AddSingleton<SmsManager>()
                    .AddScoped<DashboardManager>()
                    .AddScoped<TaskManager>()
                    .AddScoped<NotificationManager>()
                    .AddScoped<INotificationSender, NotificationSender>()
                    .AddSingleton<SubscriptionManager>()  
                    .AddSingleton<TaxTypeManager>()
                    .AddSingleton<CurrencyManager>()
                    .AddScoped<SubscriberManager>()
                    .AddScoped<UserRoleManager>();
           
            // register the swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Taxation Query API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable Swagger middleware 
            app.UseSwagger();

            // specify the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxation Query API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
