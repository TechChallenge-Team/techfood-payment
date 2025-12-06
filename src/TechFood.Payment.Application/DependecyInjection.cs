using Microsoft.Extensions.DependencyInjection;

namespace TechFood.Payment.Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
