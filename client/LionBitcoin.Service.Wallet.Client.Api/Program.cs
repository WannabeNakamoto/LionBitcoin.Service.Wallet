using LionBitcoin.Service.Wallet.Client.Application;
using LionBitcoin.Service.Wallet.Client.Infrastructure;
using LionBitcoin.Service.Wallet.Client.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi("docs");

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/docs.json", "LionBitcoin.Service.Wallet.Client.Api");
});

app.MapControllers();

app.ConfigurePersistence();

app.Run();