using Moq;
using NUnit.Framework;
using Shigar.Core.Tenants.Contracts;
using Shigar.Core.Tenants.Services;
using System.Collections.Generic;

namespace Shigar.Core.Tenants.Tests
{
    public class WhenMappingToASingleTenants
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
                    Description="Local Tenant",
                    Host="localhost",
                    Key="local",
                    Name="Local host",
                    TenantId=1
                },
                new Data.Tenant()
                {
                    Active=true,
                    Description="React",
                    Host="react-dumps",
                    Key="react",
                    Name="React host",
                    TenantId=2
                },
                new Data.Tenant()
                {
                    Active=true,
                    Description="Shiagr",
                    Host="shigar.co.uk",
                    Key="shigar",
                    Name="shigar host",
                    TenantId=3
                },
                 new Data.Tenant()
                {
                    Active=true,
                    Description="Regex",
                    Host="(?i).*\\.abc\\.com$",
                    Key="abc",
                    Name="abc generic host",
                    TenantId=4
                },
            });
            tenantSearcher = new TenantSearcher(tenantRepoMock.Object);
        }

        [TestCase("localhost", "local")]
        [TestCase("Shigar.co.uk", "shigar")]
        public void FindsExactHostnameMatch(string host, string key)
        {
            var tenant = tenantSearcher.FindByHostName(host);
            Assert.NotNull(tenant);
            Assert.AreEqual(tenant.Key, key);
        }
        [TestCase("local")]
        [TestCase("Shigar")]
        public void FindsKeyMatch(string key)
        {
            var tenant = tenantSearcher.FindByKey(key);
            Assert.NotNull(tenant);
        }

        [TestCase("admin.Abc.com", "abc")]
        [TestCase("second.abc.Com", "abc")]
        public void FindsRegexHostnameMatch(string host, string key)
        {
            var tenant = tenantSearcher.FindByHostName(host);
            Assert.NotNull(tenant);
            Assert.AreEqual(tenant.Key, key);
        }
        [TestCase("admin.abc.coma")]
        [TestCase("second.aabc.com")]
        public void ReturnsNullOnInvalidMatches(string host)
        {
            var tenant = tenantSearcher.FindByHostName(host);
            Assert.Null(tenant);
        }
    }
}