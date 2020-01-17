using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shigar.Core.Tenants.Contracts;

namespace Shigar.Core.Tenants.Data
{
    internal class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            if (context is ITenantedDbContext dynamicContext)
            {
                return (context.GetType(), dynamicContext.TenantKey);
            }
            return context.GetType();
        }
    }
}
