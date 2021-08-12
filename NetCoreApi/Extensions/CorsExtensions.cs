using Microsoft.Extensions.DependencyInjection;

namespace NetCoreApi.Extensions
{
    public static class CorsExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Constraint.PolicyKey,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(new string[] { Constraint.CorsAuthKey, Constraint.CorsAuthRefreshKey }));
            });
        }
    }
}
