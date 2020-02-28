using System;
using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shigar.Core.Tenants.Pipeline
{
    public class VerifyTenantResolution 
    {
        private readonly ITenantContext _tenantContext;
        private TenantVerificationOptions _options;
        private readonly RequestDelegate _next;

        public VerifyTenantResolution(RequestDelegate next, ITenantContext tenantContext)
        :this(next,tenantContext,null)
        {
        }
        public VerifyTenantResolution(RequestDelegate next, ITenantContext tenantContext, TenantVerificationOptions options)
        {
            _tenantContext = tenantContext;
            _options = options;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (_tenantContext.Resolved)
                await _next(context);
            else
            {
                if (_options?.IgnoredPaths == null || !_options.IgnoredPaths.Any())
                    throw new InvalidDataException($"Site could not be resolved");
                if (_options.IgnoredPaths.Any(p => p.Equals(context.Request.Path.Value, StringComparison.InvariantCultureIgnoreCase)))
                    await _next(context);
                else
                    throw new InvalidDataException($"Site could not be resolved");
            }
        }
    }
}
