using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class AttemptResolutionByQueryString : IMiddleware
    {
        private readonly ITenantSearcher _tenantSearcher;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByQueryString(ITenantSearcher tenantSearcher, ITenantContext tenantContext)
        {
            _tenantSearcher = tenantSearcher;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var key = context.Request.Query[Constants.TenantQueryStringParam].ToString();
            var tenant = _tenantSearcher.FindByKey(key);
            if (tenant != null && tenant.Active)
            {
                _tenantContext.Set(tenant.Key);
            }
            await next(context);
        }
    }
}
