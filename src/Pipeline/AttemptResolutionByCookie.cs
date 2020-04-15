using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class AttemptResolutionByCookie : IMiddleware
    {
        private readonly ITenantSearcher _tenantSearcher;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByCookie(ITenantSearcher tenantSearcher, ITenantContext tenantContext)
        {
            _tenantSearcher = tenantSearcher;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var key = context.Request.Cookies[Constants.TenantCookie];
            if (!string.IsNullOrEmpty(key))
            {
                var tenant = _tenantSearcher.FindByKey(key);
                if (tenant != null && tenant.Active)
                {
                    _tenantContext.Set(tenant.Key);
                }
            }
            await next(context);
        }
    }
}
