using LionBitcoin.Wallet.Cli;
using LionBitcoin.Wallet.Cli.Application;
using LionBitcoin.Wallet.Cli.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddApplication()
    .AddPersistence();

builder.Services.AddHostedService<ReplService>();

IHost app = builder.Build();

app.ConfigurePersistence();

await app.RunAsync();
