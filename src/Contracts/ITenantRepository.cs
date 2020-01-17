﻿using System.Collections.Generic;

namespace Shigar.Core.Tenants.Contracts
{
    public interface ITenantRepository
    {
        IList<ITenant> GetAllTenants();
        IList<ITenant> GetActiveTenants();
        bool SetStatus(int id, bool flag);
        ITenant FindById(int id);
        ITenant FindByHostName(string host);
        ITenant FindByKey(string key);
        ITenant CreateOrUpdate(ITenant tenantValues);

    }
}
