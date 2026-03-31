using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Options;
using LionBitcoin.Service.Wallet.Client.Infrastructure.Services;
using Refit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure;

public static class InfrastructureExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            return services
                .AddBitcoinCoreClient()
                .AddScoped<IBlockchainInfoService, BlockchainInfoServiceBitcoinCore>();
        }

        private IServiceCollection AddBitcoinCoreClient()
        {
            services.AddOptions<BitcoinCoreClientOptions>()
                .BindConfiguration(nameof(BitcoinCoreClientOptions));
            services.AddRefitClient<IBitcoinCoreClient>()
                .ConfigureHttpClient((serviceProvider,client) =>
                {
                    BitcoinCoreClientOptions clientOptions =
                        serviceProvider.GetRequiredService<IOptions<BitcoinCoreClientOptions>>().Value;
                    client.BaseAddress = new Uri(clientOptions.BaseUrl);
                });
            return services;
        }
    }
}