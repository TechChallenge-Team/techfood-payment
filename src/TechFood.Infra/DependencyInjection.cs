using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TechFood.Application;
using TechFood.Application.Common.Services.Interfaces;
using TechFood.Domain.Repositories;
using TechFood.Infra.Payments.MercadoPago;
using TechFood.Infra.Persistence.Repositories;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Events;
using TechFood.Shared.Domain.UoW;
using TechFood.Shared.Infra.Persistence.Contexts;
using TechFood.Shared.Infra.Persistence.UoW;

namespace TechFood.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        //Context
        services.AddScoped<TechFoodContext>();
        services.AddDbContext<TechFoodContext>((serviceProvider, options) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            options.UseSqlServer(config.GetConnectionString("DataBaseConection"));
        });

        //UoW
        services.AddScoped<IUnitOfWorkTransaction, UnitOfWorkTransaction>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TechFoodContext>());

        //DomainEvents
        services.AddScoped<IDomainEventStore>(serviceProvider => serviceProvider.GetRequiredService<TechFoodContext>());

        //Data
        services.AddScoped<IPaymentRepository, PaymentRepository>();

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

        //MediatR
        services.AddMediatR(typeof(DependecyInjection));

        var mediatR = services.First(s => s.ServiceType == typeof(IMediator));

        services.Replace(ServiceDescriptor.Transient<IMediator, Shared.Infra.EventualConsistency.Mediator>());
        services.Add(
            new ServiceDescriptor(
                mediatR.ServiceType,
                Shared.Infra.EventualConsistency.Mediator.ServiceKey,
                mediatR.ImplementationType!,
                mediatR.Lifetime));

        return services;
    }
}
