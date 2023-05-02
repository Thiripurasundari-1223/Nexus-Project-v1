using IAM.DAL.DBContext;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace IAM.API.AzureAuthenticationManager
{
    public class A2DAuthenticationManager : IA2DAuthenticationManager
    {
        private readonly string tokenKey = string.Empty;
        public A2DAuthenticationManager()
        { }
        public string Authenticate(IAMDBContext iAMDBContext, string pEmailAddress, string pPassword)
        {
            if (!iAMDBContext.User.Any(u => u.EmailAddress == pEmailAddress && u.Password == pPassword))
            {
                return null;
            }
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(tokenKey);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, pEmailAddress)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(bytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}