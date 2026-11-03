using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Talabat.Core.Entites.Identity
{
    // first step in applying security
    public class AppUser : IdentityUser                             // by default Id internally is string
    {
        public string DisplayName { get; set; }        // Name of user
         
        public Address Address { get; set; }
    }
}
