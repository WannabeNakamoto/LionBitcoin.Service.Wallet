using LionBitcoin.Wallet.Cli;
using LionBitcoin.Wallet.Cli.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddHostedService<ReplService>();

IHost app = builder.Build();

await app.RunAsync();
