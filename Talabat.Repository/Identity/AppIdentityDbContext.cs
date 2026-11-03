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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");   // to change the name of the table in the database from Address to Addresses


        }
    }
}


//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        base.OnConfiguring(optionsBuilder);
//    }