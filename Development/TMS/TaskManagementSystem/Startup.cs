using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;

namespace TaskManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddDbContext<TMSIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TMSContextConnection"));
            })
            .AddIdentity<Models.ApplicationUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedEmail = false;
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<TMSIdentityContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            });


            // Set token life span to 5 hours
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(5));

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDistributedMemoryCache();
            services.AddSession();

            services
                .AddSingleton<SiteMapManager>()
                .AddSingleton<TaskManager>()
                .AddSingleton<TaskOwnerManager>()
                .AddSingleton<SubscriptionManager>()
                .AddSingleton<SubscriberManager>()
                .AddSingleton<SmsManager>()
                .AddSingleton<EmailManager>()  
                .AddSingleton<ClientManager>()
                .AddSingleton<DashboardManager>()
                .AddSingleton<NotificationManager>()
                .AddSingleton<CommonManager>()
                .AddSingleton<QuotationManager>() 
                .AddSingleton<TaxTypeManager>()
                .AddSingleton<Microsoft.AspNetCore.Http.HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "home",
                  template: "{controller=home}/{action=index}/{id?}"
                );
            });
        }
    }
}
