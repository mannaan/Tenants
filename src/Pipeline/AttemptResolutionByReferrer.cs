using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class AttemptResolutionByReferrer : IMiddleware
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByReferrer(ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            _tenantRepository = tenantRepository;
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
                var tenant = _tenantRepository.FindByHostName(uriReferer.Host);
                if (tenant != null && tenant.Active)
                {
                    _tenantContext.Set(tenant.Key);
                }
            }
            await next(context);
        }
    }
}
