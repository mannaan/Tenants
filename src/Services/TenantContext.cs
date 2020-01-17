using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;


namespace Shigar.Core.Tenants.Services
{
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Key => _httpContextAccessor?.HttpContext?.Items?["_tenant"]?.ToString();

        public bool Resolved => !string.IsNullOrEmpty(Key);
        public void Set(string key)
        {
            if (_httpContextAccessor?.HttpContext?.Items == null)
                return;
            _httpContextAccessor.HttpContext.Items["_tenant"] = key;
        }
    }


}
