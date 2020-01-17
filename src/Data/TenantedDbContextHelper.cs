using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shigar.Core.Tenants.Contracts;

namespace Shigar.Core.Tenants.Data
{
    internal class TenantDbContexhelper
    {
        private static readonly string _tenantsColumnName = "Tenant";
        private static readonly MethodInfo PropertyMethod = typeof(EF).GetMethod(nameof(EF.Property),
            BindingFlags.Static |
            BindingFlags.Public).MakeGenericMethod(typeof(string));
        public static LambdaExpression IsTenantRestriction(Type type, string _tenantsColumnName, string TenantKey)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(PropertyMethod, parm, Expression.Constant(_tenantsColumnName));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop,
                Expression.Constant(TenantKey));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }
        public static void SetTenantValue(ChangeTracker changeTracker, string tenantKey)
        {
            changeTracker.DetectChanges();

            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Added &&
                    entry.Properties.Any(p => p.Metadata.Name.Equals(_tenantsColumnName)))
                {
                    var prop = entry.Property(_tenantsColumnName);
                    entry.Property(_tenantsColumnName).CurrentValue = tenantKey;
                }
            }
        }
        public static void AddTenantProperty(ModelBuilder builder, string TenantKey)
        {
            foreach (var entity in builder.Model.GetEntityTypes().Where(x =>
                typeof(ITenantedEntity).IsAssignableFrom(x.ClrType)))
            {
                var thisEntity = builder.Entity(entity.ClrType);
                thisEntity.Property<string>(_tenantsColumnName).HasMaxLength(256).IsRequired();
                ;
                thisEntity.HasQueryFilter(
                    IsTenantRestriction(entity.ClrType, _tenantsColumnName, TenantKey));
            }
        }
    }
}
