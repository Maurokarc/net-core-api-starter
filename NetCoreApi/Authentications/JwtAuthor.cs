using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreApi.DTO;
using NetCoreApi.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace NetCoreApi.Authentications
{
    public sealed class JwtAuthor : BaseAuthor
    {
        public JwtAuthor(IOptions<AuthorOptions> options) : base(options) { }

        public override string Generate(string UserId, string userName)
        {
            var userClaimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Sid, UserId),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(ExpireMinutes).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(ExpireMinutes),
                SigningCredentials = _signCredentials
            };

            var securityToken = _tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = _tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }

        public override IdentityDTO GetIdentity(ClaimsPrincipal principal)
        {
            IdentityDTO dto = new IdentityDTO();
            DateTime defaultTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var dict = principal.Claims.ToDictionary(p => p.Type, p => p.Value);

            if (dict.ContainsKey(JwtRegisteredClaimNames.Sid))
            {
                dto.UserId = dict[JwtRegisteredClaimNames.Sid];
            }

            if (dict.ContainsKey(ClaimTypes.NameIdentifier))
            {
                dto.UserName = dict[ClaimTypes.NameIdentifier];
            }

            if (dict.ContainsKey(JwtRegisteredClaimNames.Iat))
            {
                dto.AuthorizeTime = defaultTime.AddSeconds(Convert.ToDouble(dict[JwtRegisteredClaimNames.Iat]));
            }

            if (dict.ContainsKey(JwtRegisteredClaimNames.Exp))
            {
                dto.ExpireTime = defaultTime.AddSeconds(Convert.ToDouble(dict[JwtRegisteredClaimNames.Exp]));
            }

            return dto;
        }
    }
}
