using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shigar.Core.Tenants.Contracts;

namespace Shigar.Core.Tenants.Data
{
    internal class DynamicModelCacheKeyFactory<TContext> : IModelCacheKeyFactory
        where TContext : TenantedContext<TContext>
    {
        public object Create(DbContext context)
        {

            if (context is TenantedContext<TContext> dynamicContext)
            {
                return (context.GetType(), dynamicContext.TenantKey);
            }
            return context.GetType();
        }
    }
}
