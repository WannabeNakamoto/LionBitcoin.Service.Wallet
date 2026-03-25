using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient;
using LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient.Options;
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
                .AddMempoolClient()
                .AddScoped<IBlockchainInfoService, BlockchainInfoServiceMempool>();
        }

        private IServiceCollection AddMempoolClient()
        {
            services.AddOptions<MempoolClientOptions>()
                .BindConfiguration(nameof(MempoolClientOptions));
            services.AddRefitClient<IMempoolClient>()
                .ConfigureHttpClient((serviceProvider,client) =>
                {
                    MempoolClientOptions clientOptions =
                        serviceProvider.GetRequiredService<IOptions<MempoolClientOptions>>().Value;
                    client.BaseAddress = new Uri(clientOptions.BaseUrl);
                });
            return services;
        }
    }
}