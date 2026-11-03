using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    public static  class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0) // check if the database is empty
            {
                var user = new AppUser
                {
                    DisplayName = "Mohamed Elsayed",
                    Email = "mohamed.elsayed@example.com",
                    UserName = "mohamed.elsayed"
                };
                await _userManager.CreateAsync(user, "P@ssw0rd");    // (CreateAsync) create the user in the database with the specified password
            }
        }
    }
}
