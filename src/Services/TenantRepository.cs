using Shigar.Core.Tenants.Contracts;
using Shigar.Core.Tenants.Data;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Shigar.Core.Tenants.Services
{

    public  class TenantRepository : ITenantRepository
    {
        private readonly TenantDbContext _applicationContext;

        public TenantRepository(TenantDbContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IList<ITenant> GetAllTenants()
        {
            return _applicationContext.Tenants.ToList<ITenant>();
        }

        public IList<ITenant> GetActiveTenants()
        {
            return _applicationContext.Tenants.Where(t => t.Active).ToList<ITenant>();
        }

        public bool SetStatus(int id, bool flag)
        {
            var tenant = _applicationContext.Tenants.FirstOrDefault(t => t.TenantId == id);
            if (tenant == null)
            {
                return false;
            }

            tenant.Active = flag;
            _applicationContext.SaveChanges();
            return true;
        }

        public ITenant FindById(int id)
        {
            return _applicationContext.Tenants.FirstOrDefault(t => t.TenantId == id);
        }

        public ITenant CreateOrUpdate(ITenant tenantValues)
        {
            ITenant tenant = null;
            if (tenantValues.TenantId > 0)
                tenant = _applicationContext.Tenants.FirstOrDefault(t => t.TenantId == tenantValues.TenantId || t.Key == tenantValues.Key);
            if (tenant == null)
                tenant = _applicationContext.Tenants.Add(new Tenant()).Entity;
            tenant.Active = tenantValues.Active;
            tenant.Host = tenantValues.Host;
            tenant.Name = tenantValues.Name;
            tenant.Key = tenantValues.Key;
            tenant.Description = tenantValues.Description;


            _applicationContext.SaveChanges();
            return tenant;

        }

    }
}
