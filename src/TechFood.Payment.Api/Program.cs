using Microsoft.AspNetCore.HostFiltering;
using TechFood.Payment.Application;
using TechFood.Payment.Infra;
using TechFood.Payment.Infra.Persistence.Contexts;
using TechFood.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    // Desabilitar validação de host
    builder.Services.Configure<HostFilteringOptions>(options =>
    {
        options.AllowedHosts.Clear();
    });

    builder.Services.AddPresentation(builder.Configuration, new PresentationOptions
    {
        AddSwagger = true,
        AddJwtAuthentication = true,
        SwaggerTitle = "TechFood Payment API V1",
        SwaggerDescription = "TechFood Payment API V1"
    });

    builder.Services.AddApplication();

    builder.Services.AddInfra();

    builder.Services.AddAuthorizationBuilder()
       .AddPolicy("orders.read", policy => policy.RequireClaim("scope", "orders.read"))
       .AddPolicy("orders.write", policy => policy.RequireClaim("scope", "orders.write"));
}

var app = builder.Build();
{
    app.RunMigration<PaymentContext>();

    app.UsePathBase("/api/payment");

    app.UseForwardedHeaders();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
        });
    }

    app.UseSwaggerUI();

    app.UseInfra();

    app.UseHealthChecks("/health");

    app.UseRouting();

    app.UseCors();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
