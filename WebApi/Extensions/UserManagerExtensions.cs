using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> SearchUserAndIncludeAddressAsync(this UserManager<User> input, ClaimsPrincipal userContext)
        {
            var email = userContext?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            var userWithAddress = await input.Users.Include(user => user.Address).SingleOrDefaultAsync(user => user.Email == email);

            return userWithAddress;
        }

        public static async Task<User> SearchUserAsync(this UserManager<User> input, ClaimsPrincipal userContext)
        {
            var email = userContext?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            var user = await input.Users.SingleOrDefaultAsync(user => user.Email == email);

            return user;
        }

    }
}
