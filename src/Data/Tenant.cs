using Shigar.Core.Tenants.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Shigar.Core.Tenants.Data
{
    public class Tenant :ITenant
    {
        public int TenantId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Host { get; set; }
        [Required]
        public bool Active { get; set; }
        public string Description { get; set; }
    }
}
