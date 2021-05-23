using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreApi.Authentications;
using NetCoreApi.Options;
using NetCoreApi.Toolkit;

namespace NetCoreApi.Extensions
{
    public static class ApiExtensions
    {
        public static void ConfigureApi(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var Configuration = provider.GetService<IConfiguration>();

            services.AddTransient<IAuthor, JwtAuthor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<AuthorOptions>(Configuration.GetSection(DbConstraint.Section.Author));
            services.Configure<CryptoOptions>(Configuration.GetSection(DbConstraint.Section.Crypto));

            services.AddTransient(provider =>
            {
                var author = provider.GetService<IAuthor>();
                var Configuration = provider.GetService<IConfiguration>();
                var options = Configuration.GetSection(DbConstraint.Section.Crypto).Get<CryptoOptions>();

                return new CryptoService(author.Issuer, options.IV, options.Salt);
            });
        }
    }
}
