using DotNetCore.SharpStreamer.Storage.Sqlite;
using DotNetCore.SharpStreamer.Transport.Sqlite;
using LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;
using LionBitcoin.Wallet.Cli.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LionBitcoin.Wallet.Cli.Persistence;

public static class PersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence()
        {
            return services
                .AddDbContext()
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
}