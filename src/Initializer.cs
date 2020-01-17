using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shigar.Core.Tenants.Contracts;
using Shigar.Core.Tenants.Data;
using Shigar.Core.Tenants.Pipeline;
using Shigar.Core.Tenants.Services;
using System.Linq;

namespace Shigar.Core.Tenants
{
    public static class Initializer
    {
        public static IApplicationBuilder UseTenants(
            this IApplicationBuilder builder)
        {
            //Tenant Resolution
            builder.UseMiddleware<InitializeTenantResolutionProcessor>();
            builder.UseMiddleware<ResolveTenantByQueryStringProcessor>();
            builder.UseMiddleware<ResolveTenantByHostProcessor>();
            return builder.UseMiddleware<VerifyTenantResolutionProcessor>();
        }

        public static void AddTenants(this IServiceCollection services, string connectionString)
        {
            services.AddHttpContextAccessor();
            //pipeline processors
            services.AddTransient<InitializeTenantResolutionProcessor>();
            services.AddTransient<ResolveTenantByHostProcessor>();
            services.AddTransient<ResolveTenantByQueryStringProcessor>();
            services.AddTransient<VerifyTenantResolutionProcessor>();

            //Services
            services.AddDbContext<TenantDbContext>(options =>
               options.UseSqlServer(connectionString));
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<ITenantContext, TenantContext>();

            services.InitializeDefaultTenant(connectionString);
        }
        private static void InitializeDefaultTenant(this IServiceCollection services, string connectionString)
        {
            DesignTimeDbContextFactory.SetConnectionString(connectionString);

            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<TenantDbContext>();
            var tenantRepository = serviceProvider.GetService<ITenantRepository>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            var defaultTenant = AddTenantToDb("local", "Default tenant", "For dev", "localhost", tenantRepository);

        }
        private static ITenant AddTenantToDb(string key, string name, string desciption, string host, ITenantRepository tenantRepository)
        {
            var existing = tenantRepository.FindByKey(key);
            if (existing != null)
                return existing;
            var tenant = new Tenant
            {
                Active = true,
                TenantId = -1,
                Name = name,
                Key = key,
                Host = host,
                Description = desciption
            };
            return tenantRepository.CreateOrUpdate(tenant);
        }
    }
}
