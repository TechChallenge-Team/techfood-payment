using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechFood.Payment.Application;
using TechFood.Payment.Infra;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddWorker();
    builder.Services.AddApplication();
    builder.Services.AddInfra();
}

var app = builder.Build();
{
    app.Run();
}
