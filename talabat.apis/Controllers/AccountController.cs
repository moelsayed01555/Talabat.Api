using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using talabat.apis.Extensions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services.Contract;

namespace talabat.apis.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public IMapper _mapper { get; }

        public AccountController(
            IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService
            )  // constructor injection for the user manager and sign in manager
        {
            _mapper = mapper;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._authService = authService;
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


            // create token for the user
            var token = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            // check if the user already exists in the database
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null) 
               return BadRequest(new ApiErrorResponse(404) );

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

            // create token for the user
            var token = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });

        }




        [Authorize]
        [HttpGet]   //GET : /api/account
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);  // get the email of the user that is logged in

            //بمعلومية الايميل اقدر اعرف مين المستخدم اللي عامل لوجن
            var user = await _userManager.FindByEmailAsync(email);

            var userDto = new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            };

            return Ok(userDto);
        }



        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<UserAddressDto>> GetCurrentAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var address = _mapper.Map<UserAddressDto>(user.Address);

            return Ok(address);
        }


        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<UserAddressDto>> UpdateUserAddress(UserAddressDto updatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            if (user is null) return Unauthorized();

            if (user.Address is null)
            {
                // اليوزر ماعندوش عنوان لسه: ننشئ واحد جديد
                user.Address = _mapper.Map<Address>(updatedAddress);
            }
            else
            {
                // نعدّل العنوان الموجود (الـ Id بيفضل زي ما هو)
                _mapper.Map(updatedAddress, user.Address);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(_mapper.Map<UserAddressDto>(user.Address));
        }



        // GET: api/account/emailexists?email=ahmed@gmail.com
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new ApiErrorResponse(400, "Email is required"));

            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
