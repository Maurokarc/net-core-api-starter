using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetCoreApi.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApi.Infrastructure.Contexts
{
    internal sealed class ServerContext : BaseContext
    {
        #region Tables

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        #endregion

        public ServerContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Key Define

            modelBuilder.Entity<User>().HasKey(p => p.UserId);
            modelBuilder.Entity<UserToken>().HasKey(p => p.TokenId);

            #endregion

            #region Relationships

            modelBuilder.Entity<User>().HasMany(p => p.Tokens);

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(x => Debug.WriteLine(x));

#if DEBUG

            // Enable Parameter Values in Logging

            optionsBuilder.EnableSensitiveDataLogging();

#endif

            base.OnConfiguring(optionsBuilder);
        }

        #region Extend Methods

        public async Task<dynamic> RawSql(string comdText, SqlParameter[] parameters = null)
        {
            var conn = GetConnection();

            if (conn.State != System.Data.ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            using var command = conn.CreateCommand();
            command.CommandText = comdText;
            command.CommandType = System.Data.CommandType.Text;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            var reader = await command.ExecuteReaderAsync();
            List<Dictionary<string, object>> result = new();

            while (await reader.ReadAsync())
            {
                result.Add(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
            }

            conn.Close();

            return result;

        }

        public async Task<(bool isSucceed, Dictionary<string, object> data)> ExecuteStorePro(string storeProName, SqlParameter[] parameters = null)
        {
            var conn = GetConnection();
            Dictionary<string, object> result = new();

            if (conn.State != System.Data.ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            try
            {
                using var command = conn.CreateCommand();
                command.CommandText = storeProName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters == null)
                {
                    parameters = Array.Empty<SqlParameter>();
                }

                command.Parameters.AddRange(parameters);

                await command.ExecuteNonQueryAsync();

                result = parameters.ToDictionary(p => p.ParameterName, p => p.Value);

                return (true, result);
            }
            catch (Exception)
            {
                return (false, result);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<DateTime> GetSysDate()
        {
            var raw = await RawSql("select getdate() as SYSDATE");

            var result = ((List<Dictionary<string, object>>)raw).FirstOrDefault()?["SYSDATE"];

            return (result != null && (result is DateTime time)) ? time : DateTime.Now;
        }

        #endregion
    }
}
