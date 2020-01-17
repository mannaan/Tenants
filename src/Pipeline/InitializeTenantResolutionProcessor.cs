using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    internal class InitializeTenantResolutionProcessor
    {
        private readonly RequestDelegate _next;
        private readonly ITenantContext _tenantContext;

        public InitializeTenantResolutionProcessor(RequestDelegate next, ITenantContext tenantContext)
        {
            _next = next;
            _tenantContext = tenantContext;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _tenantContext.Set(null);
            await _next(context);
        }
    }
}
