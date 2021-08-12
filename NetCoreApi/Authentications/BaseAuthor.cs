using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreApi.DTO;
using NetCoreApi.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCoreApi.Authentications
{
    public abstract class BaseAuthor : IAuthor
    {
        public string Issuer { get; }
        public int ExpireMinutes { get; }
        public int RefreshExpireDays { get; }

        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int KEY_LENGTH = 32;
        private static readonly SymmetricSecurityKey _securityKey;

        protected static readonly SigningCredentials _signCredentials;
        protected static readonly JwtSecurityTokenHandler _tokenHandler;
        protected static readonly string _secretKey;

        static BaseAuthor()
        {
            Random random = new Random();
            _secretKey = new string(Enumerable.Repeat(CHARS, KEY_LENGTH).Select(s => s[random.Next(s.Length)]).ToArray());
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            _signCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public BaseAuthor(IOptions<AuthorOptions> options)
        {
            Issuer = !string.IsNullOrWhiteSpace(options.Value.Issuer) ? options.Value.Issuer : Constraint.DefaultIssuer;
            ExpireMinutes = options.Value.ExpireMinutes.HasValue ?
                              options.Value.ExpireMinutes.Value > 0 ? options.Value.ExpireMinutes.Value : 30
                              : 30;

            RefreshExpireDays = options.Value.RefreshExpireDays.HasValue ?
                              options.Value.RefreshExpireDays.Value > 0 ? options.Value.RefreshExpireDays.Value : 7
                              : 7;
        }

        public abstract string Generate(string UserId, string userName);
        public abstract IdentityDTO GetIdentity(ClaimsPrincipal principal);
        public string GetSecretKey() => _secretKey;
    }
}
