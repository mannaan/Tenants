using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    internal class ResolveTenantByQueryStringProcessor : IMiddleware
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantContext _tenantContext;

        public ResolveTenantByQueryStringProcessor(ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            _tenantRepository = tenantRepository;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var key = context.Request.Query["_tenant"].ToString();
            var tenant = _tenantRepository.FindByKey(key);
            if (tenant != null && tenant.Active)
            {
                _tenantContext.Set(tenant.Key);
            }
            await next(context);
        }
    }
}
