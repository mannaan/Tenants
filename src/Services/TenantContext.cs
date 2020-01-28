using Microsoft.AspNetCore.Http;
using Shigar.Core.Tenants.Contracts;
using System;

namespace Shigar.Core.Tenants.Services
{
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Key => _httpContextAccessor?.HttpContext?.Items?[Constants.TenantQueryStringParam]?.ToString();

        public bool Resolved => !string.IsNullOrEmpty(Key);
        public void Set(string key)
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context == null)
                return;
            if (string.IsNullOrEmpty(key))
            {
                context.Response.Cookies.Delete(Constants.TenantCookie);
            }
            else
            {
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(60)
                };
                context.Response.Cookies.Append(Constants.TenantCookie, key, option);
            }
            if (_httpContextAccessor?.HttpContext?.Items == null)
                return;
            _httpContextAccessor.HttpContext.Items[Constants.TenantQueryStringParam] = key;
        }
    }


}
