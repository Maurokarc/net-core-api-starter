using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NetCoreApi.Infrastructure.Contexts
{
    internal abstract class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options) { }

        public bool Rollback()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State != EntityState.Unchanged
                           select e;

            foreach (var entity in entities)
            {
                entity.State = EntityState.Unchanged;
            }

            base.SaveChanges();

            return true;
        }

        protected SqlConnection GetConnection()
        {
            return (SqlConnection)this.Database.GetDbConnection();
        }

    }
}
