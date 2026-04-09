using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Wallet.Cli.Persistence;

public class LionBitcoinDbContext(DbContextOptions<LionBitcoinDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}