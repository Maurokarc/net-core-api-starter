using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetCoreApi.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddLogger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var Configuration = provider.GetService<IConfiguration>();

            services.AddLogging(builder =>
            builder.AddConfiguration(Configuration.GetSection(Constraint.Section.Log))
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddConsole()
                .AddDebug());
        }

        public static ILogger CreateDomainLogger(this ILoggerFactory factory)
        {
            return factory.CreateLogger("domain");
        }
    }
}
