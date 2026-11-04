using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using Talabat.Core.Entites.Identity;

namespace talabat.apis.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)  // constructor injection for the user manager and sign in manager
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            // check if the user exists in the database
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiErrorResponse(401));

            // check if the password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);    // CheckPasswordSignInAsync : checks on the password only , don't sign in
            if (!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "token"
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            // check if the user already exists in the database
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null) 
               return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email already exists" } });

            // create a new user
            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0] ,
                PhoneNumber = model.PhoneNumber
            };

            // CreateAsync : create the user in the database with the specified password
            var result = await _userManager.CreateAsync(user, model.Password);

            // check if the user was created successfully
            if (!result.Succeeded)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "token"
            });

        }


    }
}
