# Tenants
Helps create a multi-tenanted web application (Asp.NetCore)

# How to use
- Create a new aspnet core web/api project
- At the start of configure services in Startup, add tenant services (i.e services.AddTenants("[SQL Connection string]");)
- In Startup.Configure use Tenants (i.e 
                 app.InitTenantResolution()
                .Then< AttemptResolutionByQueryString>()
                .Then< AttemptResolutionByHost>()
                .Then< VerifyTenantResolution>();)

First execution of application should create a table in DB called "Tenants". 

To apply the functionality to your DbContext, 
- inherit it from "TenantedContext".
- Each tenanted entity should implement ITenantedEntity interface

                       
