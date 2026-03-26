using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Service.Wallet.Client.Persistence;

public class WalletClientDbContext(DbContextOptions<WalletClientDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}