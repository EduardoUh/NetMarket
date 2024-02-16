using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        IGenericSecurityRepository<User> securityRepository,
        RoleManager<IdentityRole> roleManager
        )
        : BaseApiController
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly IGenericSecurityRepository<User> _securityRepository = securityRepository;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                Name = user.Name,
                LastName = user.LastName,
                Image = user.Image,
                Admin = roles.Contains("ADMIN"),
                Token = _tokenService.CreateToken(user, roles)
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
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Admin = false,
                Image = "",
                Token = _tokenService.CreateToken(user, null)
            };
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("account/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound(new CodeErrorResponse(404, "User not found"));

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                LastName = user.LastName,
                Admin = roles.Contains("ADMIN"),
                Image = user.Image
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

            var roles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404));
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName!,
                Email = user.Email!,
                Admin = roles.Contains("ADMIN"),
                Image = user.Image,
                Token = _tokenService.CreateToken(user, roles)
            };
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, [FromBody] SignUpDto signUpDto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound(new CodeErrorException(404, "User not found"));

            user.Name = signUpDto.Name;
            user.LastName = signUpDto.LastName;
            user.Image = signUpDto.Image;

            if (string.IsNullOrEmpty(signUpDto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, signUpDto.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new CodeErrorException(400, "Couldn't update the user"));

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Image = user.Image,
                UserName = user.UserName,
                Admin = roles.Contains("ADMIN"),
                Token = _tokenService.CreateToken(user, roles)
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("pagination")]
        public async Task<ActionResult<Pagination<UserDto>>> GetUsers([FromQuery] UserSpecificationParams userParams)
        {
            var spec = new UserSpecification(userParams);
            var countingSpec = new UserForCountingSpecification(userParams);

            var users = await _securityRepository.GetAllWithSpecAsync(spec);

            var totalUsers = await _securityRepository.CountAsync(countingSpec);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalUsers) / Convert.ToDecimal(userParams.PageSize));

            var totalPages = Convert.ToInt32(rounded);

            return Ok(new Pagination<UserDto>
            {
                Count = totalUsers,
                Data = _mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserDto>>(users),
                PageCount = totalPages,
                PageIndex = userParams.PageIndex,
                PageSize = userParams.PageSize,
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("role/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUserRol(string id, RoleDto roleParam)
        {
            var role = await _roleManager.FindByNameAsync(roleParam.Name);

            if (role == null) return NotFound(new CodeErrorResponse(404, "Role not found"));

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound(new CodeErrorResponse(404, "User not found"));

            var userDto = _mapper.Map<User, UserDto>(user);

            if (roleParam.Status)
            {
                var result = await _userManager.AddToRoleAsync(user, roleParam.Name);

                if (result.Succeeded) userDto.Admin = true;

                if (result.Errors.Any())
                {
                    if (result.Errors.Where(error => error.Code == "UserAlreadyRole").Any()) userDto.Admin = true;
                }
            }
            else
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleParam.Name);

                if (result.Succeeded)
                {
                    userDto.Admin = false;
                }
            }

            if (userDto.Admin)
            {
                var roles = new List<string>
                {
                    "ADMIN"
                };

                userDto.Token = _tokenService.CreateToken(user, roles);
            }
            else
            {
                userDto.Token = _tokenService.CreateToken(user, null);
            }

            return userDto;
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
