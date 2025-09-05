using BLL.Infrastructure;
using DAL.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ITokenBLL
    {
        string GenerateAccessToken(Staff staff, DateTime expires);
    }
    public class TokenBLL : ITokenBLL
    {

        private readonly IOptions<JWTSetting> appSettings;

        public TokenBLL(IOptions<JWTSetting> appSettings)
        {
            this.appSettings = appSettings;
        }

        #region GenerateAccessToken

        /// <summary>
        /// Generate access token
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string GenerateAccessToken(Staff staff, DateTime expires)
        {
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.AccessTokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = appSettings.Value.Issuer,
                Audience = "",
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, staff.Name),
                new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()),
                new Claim(ClaimTypes.Email, staff.Email),
                new Claim(ClaimTypes.Role, staff.Role),
                new Claim(ClaimTypes.Expiration, expires.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToString(), ClaimValueTypes.Integer64),
                }),
                NotBefore = DateTime.UtcNow,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }

        #endregion
    }
}
