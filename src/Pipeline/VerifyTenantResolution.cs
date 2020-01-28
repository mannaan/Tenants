using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class VerifyTenantResolution : IMiddleware
    {
        private readonly ITenantContext _tenantContext;

        public VerifyTenantResolution(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            else

                throw new InvalidDataException($"Site could not be resolved");
        }
    }
}
