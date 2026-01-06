using TechFood.Payment.Infra;
using TechFood.Payment.Application;
using Microsoft.AspNetCore.Builder;

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
