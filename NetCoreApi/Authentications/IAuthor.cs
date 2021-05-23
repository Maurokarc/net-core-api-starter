using NetCoreApi.DTO;
using System.Security.Claims;

namespace NetCoreApi.Authentications
{
    public interface IAuthor
    {
        string Issuer { get; }
        int ExpireMinutes { get; }
        int RefreshExpireDays { get; }
        string GetSecretKey();
        string Generate(string UserId, string userName);
        IdentityDTO GetIdentity(ClaimsPrincipal principal);
    }
}
