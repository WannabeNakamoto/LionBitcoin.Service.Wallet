using DotNetCore.SharpStreamer.Storage.Npgsql;
using DotNetCore.SharpStreamer.Transport.Npgsql;
using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using LionBitcoin.Service.Wallet.Client.Persistence.Repositories;
using LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LionBitcoin.Service.Wallet.Client.Persistence;

public static class PersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence()
        {
            return services
                .AddDbContext()
                .AddScoped<IWalletRepository, WalletRepository>()
                .AddScoped<IUtxoRepository, UtxoRepository>()
                .AddSharpStreamerStorageNpgsql<WalletClientDbContext>()
                .AddSharpStreamerTransportNpgsql();
        }

        private IServiceCollection AddDbContext()
        {
            services.AddDbContext<WalletClientDbContext>((serviceProvider, options) =>
            {
                IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                options.UseNpgsql(configuration.GetConnectionString(nameof(WalletClientDbContext)));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<WalletClientDbContext>>();
            return services;
        }
    }

    extension(IHost host)
    {
        public IHost ConfigurePersistence()
        {
            IServiceScopeFactory scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
            scope.ServiceProvider.GetRequiredService<WalletClientDbContext>().Database.Migrate();
            scope.DisposeAsync();
            return host;
        }
    }
}