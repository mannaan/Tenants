using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shigar.Core.Tenants.Data
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        private static string _connectionString = "";
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        public TenantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TenantDbContext>();
            builder.UseSqlServer(_connectionString);
            return new TenantDbContext(builder.Options);
        }
    }
}
