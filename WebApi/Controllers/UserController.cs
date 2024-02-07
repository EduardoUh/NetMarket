using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMapper mapper) : BaseApiController
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new CodeErrorResponse(401, "User or password are incorrect"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401, "User or password are incorrect"));
            }

            return new UserDto
            {
                Email = user.Email!,
                UserName = user.UserName!,
                Name = user.Name,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> SignUp([FromBody] SignUpDto signUpDto)
        {
            var user = new User
            {
                Name = signUpDto.Name,
                LastName = signUpDto.LastName,
                UserName = signUpDto.UserName,
                Email = signUpDto.Email
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "One or more fields are invalid"));
            }

            return new UserDto
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        // only users with a token are granted the access
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            //if (string.IsNullOrEmpty(email))
            //{
            //    return Unauthorized(new CodeErrorResponse(401));
            //}

            //var user = await _userManager.FindByEmailAsync(email);

            //if (user == null)
            //{
            //    return NotFound(new CodeErrorResponse(404));
            //}

            var user = await _userManager.SearchUserAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404));
            }

            return new UserDto
            {
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("emailvalid")]
        public async Task<ActionResult<bool>> ValidateEmail([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            //if (string.IsNullOrEmpty(email))
            //{
            //    return Unauthorized(new CodeErrorResponse(401));
            //}

            //var user = await _userManager.FindByEmailAsync(email);

            //if (user == null)
            //{
            //    return NotFound(new CodeErrorResponse(404));
            //}

            // using the extension class
            var user = await _userManager.SearchUserAndIncludeAddressAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404));
            }

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var user = await _userManager.SearchUserAndIncludeAddressAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404));
            }

            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

    }
}
