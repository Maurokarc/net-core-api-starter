using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreApi.Infrastructure.Services;
using NetCoreApi.Options;
using System;
using System.Linq;
using System.Reflection;

namespace NetCoreApi.Extensions
{
    public static class InfrastructureExtensions
    {
        public static void ConfigureInfrastructure(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var Configuration = provider.GetService<IConfiguration>();
            var factory = provider.GetService<ILoggerFactory>();
            var database = Configuration.GetSection(DbConstraint.Section.Db).Get<DatabaseOptions>();

            var optionsBuilder = new DbContextOptionsBuilder<DbContext>()
                .UseSqlServer(database.ConnectionString)
                .UseLoggerFactory(factory);

            var entryAssembly = Assembly.GetEntryAssembly();
            var assy = entryAssembly.GetReferencedAssemblies()
                                    .Where(p => p.Name.Equals($"{DbConstraint.DefaultNamespace}.Infrastructure"))
                                    .Select(Assembly.Load).FirstOrDefault();

            var interfaces = assy.GetExportedTypes()
                            .Where(t => t.Namespace.StartsWith($"{DbConstraint.DefaultNamespace}.Infrastructure.Services") && t.IsInterface)
                            .ToList();

            foreach (var basicType in interfaces)
            {
                var implementationTypes = assy.GetExportedTypes().Where(p => !p.IsInterface && !p.IsAbstract && p.IsClass && basicType.IsAssignableFrom(p)).ToList();

                foreach (var implementationType in implementationTypes)
                {
                    services.AddTransient(basicType, ctx => Activator.CreateInstance(implementationType, new object[] { optionsBuilder.Options, factory }));
                }
            }

            // auto migration
            using var serv = new MigrationsService(optionsBuilder.Options, factory);
            serv.UpMigrations();
        }
    }
}
