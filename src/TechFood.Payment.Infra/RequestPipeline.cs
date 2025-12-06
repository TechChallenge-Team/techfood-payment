using Microsoft.AspNetCore.Builder;

namespace TechFood.Payment.Infra;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfra(this IApplicationBuilder app)
    {
        app.UseSharedInfra();

        return app;
    }
}
