using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using System.Text.Json;

namespace NetCoreApi.Infrastructure.ContextsFactory
{
    internal abstract class BaseFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public abstract TContext CreateDbContext(string[] args);

        protected static string GetConnectionStrig()
        {
            string envPath = Path.Combine(RelativeToServerPath(), "env.json");

            using StreamReader reader = new(envPath);

            string json = reader.ReadToEnd();
            JsonDocument token = JsonDocument.Parse(json);

            string connStr = token.RootElement.GetProperty("Database").GetProperty("ConnectionString").ToString();

            return connStr;
        }

        private static string RelativeToServerPath()
        {
            var currentRoot = new DirectoryInfo(Directory.GetCurrentDirectory());

            return Path.Combine(currentRoot.Parent.FullName, "NetCoreApi");
        }
    }
}
