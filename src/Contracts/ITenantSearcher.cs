namespace Shigar.Core.Tenants.Contracts
{
    public interface ITenantSearcher
    {
        ITenant FindByHostName(string host);
        ITenant FindByKey(string key);
    }
}
