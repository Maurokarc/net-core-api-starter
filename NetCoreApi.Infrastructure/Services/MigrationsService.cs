using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NetCoreApi.Infrastructure.Services
{
    public sealed class MigrationsService : AbstractService
    {
        public MigrationsService(DbContextOptions dbOptions, ILoggerFactory factory) : base(dbOptions, factory) { }

        public bool UpMigrations()
        {
            try
            {
                _Context.Database.Migrate();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex, "Migration Fail !!");
                return false;
            }
        }

    }
}
