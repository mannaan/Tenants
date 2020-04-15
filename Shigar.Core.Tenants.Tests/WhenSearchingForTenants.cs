using Moq;
using NUnit.Framework;
using Shigar.Core.Tenants.Contracts;
using Shigar.Core.Tenants.Services;
using System.Collections.Generic;

namespace Shigar.Core.Tenants.Tests
{
    public class WhenSearchingForTenants
    {
        private ITenantSearcher tenantSearcher;
        [SetUp]
        public void Setup()
        {
            var tenantRepoMock = new Mock<ITenantRepository>();
            tenantRepoMock.Setup(r => r.GetActiveTenants()).Returns(new List<ITenant>()
            {
             
                 new Data.Tenant()
                {
                    Active=true,
                    Description="Generic Regex",
                    Host=".*",
                    Key="all",
                    Name="abc generic host",
                    TenantId=4
                },
            });
            tenantSearcher = new TenantSearcher(tenantRepoMock.Object);
        }

 

        [TestCase("admin.Abc.com", "all")]
        [TestCase("second.abc.Com", "all")]
        public void FindsRegexHostnameMatch(string host, string key)
        {
            var tenant = tenantSearcher.FindByHostName(host);
            Assert.NotNull(tenant);
            Assert.AreEqual(tenant.Key, key);
        }
        
    }
}