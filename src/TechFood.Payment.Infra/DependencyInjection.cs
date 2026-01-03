using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechFood.Payment.Application.Common.Services.Interfaces;
using TechFood.Payment.Domain.Repositories;
using TechFood.Payment.Infra.Payments.MercadoPago;
using TechFood.Payment.Infra.Persistence.Contexts;
using TechFood.Payment.Infra.Persistence.Repositories;
using TechFood.Payment.Infra.Services;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.Payment.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        //Context
        services.AddSharedInfra<PaymentContext>(new InfraOptions
        {
            DbContext = (serviceProvider, dbOptions) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                dbOptions.UseSqlServer(config.GetConnectionString("DataBaseConection"));
            },
            ApplicationAssembly = typeof(Application.DependecyInjection).Assembly
        });

        //Data
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        //service
        services.AddTechFoodClient<IOrderService, OrderService>("Order");
        services.AddTechFoodClient<IBackofficeService, BackofficeService>("Backoffice");

        // Payments
        services.AddOptions<MercadoPagoOptions>()
            .Configure<IConfiguration>((options, config) =>
            {
                var configSection = config.GetSection(MercadoPagoOptions.SectionName);
                configSection.Bind(options);
            });

        services.AddKeyedTransient<IPaymentService, MercadoPagoPaymentService>(PaymentType.MercadoPago);

        services.AddHttpClient(MercadoPagoOptions.ClientName, (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(MercadoPagoOptions.BaseAddress);
            client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

            client.DefaultRequestHeaders.Authorization = new("Bearer", serviceProvider.GetRequiredService<IOptions<MercadoPagoOptions>>().Value.AccessToken);
        });

        return services;
    }
}
