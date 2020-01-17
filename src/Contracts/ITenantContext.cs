namespace Shigar.Core.Tenants.Contracts
{
    public interface ITenantContext
    {
        string Key { get; }
        bool Resolved { get; }
        void Set(string key);
    }
}
