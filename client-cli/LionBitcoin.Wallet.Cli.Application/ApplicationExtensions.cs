using System.Reflection;
using DotNetCore.SharpStreamer;
using LionBitcoin.Wallet.Cli.Application.Options;
using LionBitcoin.Wallet.Cli.Application.Services;
using LionBitcoin.Wallet.Cli.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Wallet.Cli.Application;

public static class ApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            // Options
            services.AddOptions<ApplicationOptions>()
                .BindConfiguration(nameof(ApplicationOptions));

            // Services
            services.AddSingleton(TimeProvider.System);
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IScriptService, ScriptService>();
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.Lifetime = ServiceLifetime.Transient;
            });
            services
                .AddSharpStreamer("SharpStreamerSettings", Assembly.GetExecutingAssembly());
            return services;
        }
    }
}