using Microsoft.EntityFrameworkCore;

namespace Shigar.Core.Tenants.Data
{
    public class TenantDbContext : DbContext
    {

        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Tenant>()
               .HasIndex(u => u.Key)
               .IsUnique();
            base.OnModelCreating(builder);
        }
    }
}
