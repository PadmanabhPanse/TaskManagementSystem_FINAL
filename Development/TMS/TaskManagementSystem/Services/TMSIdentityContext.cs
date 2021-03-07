using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Services
{
    public class TMSIdentityContext : IdentityDbContext<Models.ApplicationUser>
    {
        public TMSIdentityContext()
        {
        }

        public TMSIdentityContext(DbContextOptions<TMSIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
