using LionBitcoin.Service.Wallet.Client.Application.Options;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Service.Wallet.Client.Application;

public static class ApplicationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddOptions<ApplicationOptions>()
                .BindConfiguration(nameof(ApplicationOptions));
            return services;
        }
    }
}