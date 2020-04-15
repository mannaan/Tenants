using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class AttemptResolutionByHost : IMiddleware
    {
        private readonly ITenantSearcher _tenantSearcher;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByHost(ITenantSearcher tenantSearcher, ITenantContext tenantContext)
        {
            _tenantSearcher = tenantSearcher;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var host = context.Request.Host.Host;
            var tenant = _tenantSearcher.FindByHostName(host);
            if (tenant != null && tenant.Active)
            {
                _tenantContext.Set(tenant.Key);
            }
            await next(context);
        }
    }
}
