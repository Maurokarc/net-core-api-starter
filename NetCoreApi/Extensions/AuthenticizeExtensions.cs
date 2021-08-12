using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreApi.Authentications;
using NetCoreApi.Options;
using System.Security.Claims;
using System.Text;

namespace NetCoreApi.Extensions
{
    public static class AuthenticizeExtensions
    {
        public static void SetJwtAuth(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var handler = provider.GetService<IAuthor>();
            var Configuration = provider.GetService<IConfiguration>();
            var author = Configuration.GetSection(Constraint.Section.Author).Get<AuthorOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.IncludeErrorDetails = true;
                        options.SaveToken = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = ClaimTypes.NameIdentifier,
                            RoleClaimType = ClaimTypes.Role,
                            ValidIssuer = !string.IsNullOrWhiteSpace(author?.Issuer) ? author?.Issuer : Constraint.DefaultIssuer,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = false,
                            ValidateActor = false,
                            ValidateTokenReplay = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(handler.GetSecretKey()))
                        };
                    });
        }
    }
}
