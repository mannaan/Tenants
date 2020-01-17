namespace Shigar.Core.Tenants.Contracts
{
    public interface ITenantedDbContext
    {
        string TenantKey { get; }
    }
}