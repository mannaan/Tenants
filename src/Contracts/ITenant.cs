namespace Shigar.Core.Tenants.Contracts
{
    public interface ITenant
    {
        int TenantId { get; set; }
        string Key { get; set; }
        string Name { get; set; }
        string Host { get; set; }
        bool Active { get; set; }
        string Description { get; set; }

    }
}
