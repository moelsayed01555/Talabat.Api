using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Identity;

namespace Talabat.Core.Services.Contract
{
    public interface IAuthService
    {
        // create token for user --- Note : Token is string & the proprty signature will take the user i will create a token for him
        Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager);
    }
}
