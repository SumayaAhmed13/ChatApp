using ChatingApi.Entities;
using ChatingApi.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatingApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)

            };

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var securityDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddMonths(1),
                SigningCredentials=cred

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityDes);
            return tokenHandler.WriteToken(token);
        }
    }
}
