using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    // DbContext ( Database for redis ) 
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) 
            : base(options)
        {
            
        }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        base.OnConfiguring(optionsBuilder);
    //    }
    }
}
