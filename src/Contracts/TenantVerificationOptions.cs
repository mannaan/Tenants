using System.Collections.Generic;

namespace Shigar.Core.Tenants.Contracts
{
    public class TenantVerificationOptions
    {
        public TenantVerificationOptions(List<string> ignoredPaths)
        {
            IgnoredPaths = ignoredPaths;
        }

        public List<string> IgnoredPaths { get; }
    }
}