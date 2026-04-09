using DotNetCore.SharpStreamer.Storage.Sqlite;
using DotNetCore.SharpStreamer.Transport.Sqlite;
using LionBitcoin.Wallet.Cli.Application.Repositories;
using LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;
using LionBitcoin.Wallet.Cli.Persistence.Repositories;
using LionBitcoin.Wallet.Cli.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LionBitcoin.Wallet.Cli.Persistence;

public static class PersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence()
        {
            return services
                .AddDbContext()
                .AddScoped<IWalletRepository, WalletRepository>()
                .AddSharpStreamerStorageSqlite<LionBitcoinDbContext>()
                .AddSharpStreamerTransportSqlite();
        }

        private IServiceCollection AddDbContext()
        {
            services.AddDbContext<LionBitcoinDbContext>((serviceProvider, options) =>
            {
                IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                options.UseSqlite(configuration.GetConnectionString(nameof(LionBitcoinDbContext)));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<LionBitcoinDbContext>>();
            return services;
        }
    }

    extension(IHost host)
    {
        public IHost ConfigurePersistence()
        {
            IServiceScopeFactory scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
            scope.ServiceProvider.GetRequiredService<LionBitcoinDbContext>().Database.Migrate();
            scope.DisposeAsync();
            return host;
        }
    }
}