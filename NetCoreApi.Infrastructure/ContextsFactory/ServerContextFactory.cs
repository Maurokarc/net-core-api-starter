using Microsoft.EntityFrameworkCore;
using NetCoreApi.Infrastructure.Contexts;

namespace NetCoreApi.Infrastructure.ContextsFactory
{
    internal class ServerContextFactory : BaseFactory<ServerContext>
    {
        public override ServerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServerContext>();

            optionsBuilder.UseSqlServer(GetConnectionStrig());

            return new ServerContext(optionsBuilder.Options);
        }
    }
}
