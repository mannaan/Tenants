using Microsoft.AspNetCore.Builder;

namespace Shigar.Core.Tenants
{
    public class TenantResolutionBuilder
    {
        private IApplicationBuilder _builder { get; }
        public TenantResolutionBuilder(IApplicationBuilder appBuider)
        {
            _builder = appBuider;
        }

        public void UseMiddleware<T>() 
        {
            _builder.UseMiddleware<T>();
        }
        public void UseMiddleware<T>(object options) 
        {
            _builder.UseMiddleware<T>(options);
        }

    }
    public static class TenantResolutionBuilderExtensions
    {
        public static TenantResolutionBuilder Then<T>(
            this TenantResolutionBuilder builder) 
        {
            builder.UseMiddleware<T>();
            return builder;
        }
        public static TenantResolutionBuilder Then<T,TOptions>(
            this TenantResolutionBuilder builder, TOptions options)
        {
            builder.UseMiddleware<T>(options);
            return builder;
        }

    }
}
