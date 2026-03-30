using System.Reflection;
using LionBitcoin.Service.Wallet.Client.Application.Options;
using LionBitcoin.Service.Wallet.Client.Application.Services;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Service.Wallet.Client.Application;

public static class ApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddSingleton(TimeProvider.System);
            services.AddOptions<ApplicationOptions>()
                .BindConfiguration(nameof(ApplicationOptions));
            services.AddScoped<IScriptService, ScriptService>();
            services.AddScoped<IWalletService, WalletService>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
    }
}