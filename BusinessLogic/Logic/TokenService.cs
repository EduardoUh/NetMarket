using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic.Logic
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]!));
        }

        public string CreateToken(User user)
        {
            // a token is composed by three parts:
            // claims -> data about the user you want to include
            // credentials -> key and algorithm to be used to encrypt the token and to validate it was indeed
            // expeded by this backend
            // expiration time -> defined time in which the token will expire

            // claims are the data you want to be included in the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim("lastname", user.LastName),
                new Claim("username", user.UserName!)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = credentials,
                Issuer = _configuration["Token:Issuer"]
            };

            // tokenHandler is neccesary to create the token and to parse it to string
            var tokenHandler = new JwtSecurityTokenHandler();
            // creating the token based on the previous config
            var tokenObject = tokenHandler.CreateToken(tokenConfiguration);

            // returning the token but parsed to string
            return tokenHandler.WriteToken(tokenObject);
        }

    }
}
