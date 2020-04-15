using Shigar.Core.Tenants.Contracts;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shigar.Core.Tenants.Services
{
    public class TenantSearcher : ITenantSearcher
    {
        private readonly ITenantRepository _tenantRepository;
        public TenantSearcher(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        public ITenant FindByHostName(string host)
        {
            var allTenants = _tenantRepository.GetActiveTenants();
            var tenant = allTenants.FirstOrDefault(t => t.Host.Equals(host, StringComparison.InvariantCultureIgnoreCase));
            if (tenant != null)
                return tenant;
            return allTenants.FirstOrDefault(t => Regex.IsMatch(host, t.Host));
        }

        public ITenant FindByKey(string key)
        {
            var allTenants = _tenantRepository.GetActiveTenants();
           return allTenants.FirstOrDefault(t => t.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
