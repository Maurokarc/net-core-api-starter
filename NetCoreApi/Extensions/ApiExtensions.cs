using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            services.Configure<AuthorOptions>(Configuration.GetSection(Constraint.Section.Author));
            services.Configure<CryptoOptions>(Configuration.GetSection(Constraint.Section.Crypto));

            services.AddTransient(provider =>
            {
                var author = provider.GetService<IAuthor>();
                var Configuration = provider.GetService<IConfiguration>();
                var options = Configuration.GetSection(Constraint.Section.Crypto).Get<CryptoOptions>();

                return new CryptoService(author.Issuer, options.IV, options.Salt);
            });
        }


        public static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
        }


    }
}
