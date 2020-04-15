using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class AttemptResolutionByReferrer : IMiddleware
    {
        private readonly ITenantSearcher _tenantSearcher;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByReferrer(ITenantSearcher tenantSearcher, ITenantContext tenantContext)
        {
            _tenantSearcher = tenantSearcher;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var referrer = context.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referrer))
            {
                var uriReferer = new Uri(referrer);
                var tenant = _tenantSearcher.FindByHostName(uriReferer.Host);
                if (tenant != null && tenant.Active)
                {
                    _tenantContext.Set(tenant.Key);
                }
            }
            await next(context);
        }
    }
}
