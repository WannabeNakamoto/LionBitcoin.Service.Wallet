using LionBitcoin.Wallet.Cli.Persistence.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LionBitcoin.Wallet.Cli.Persistence.Configurations;

public class WalletConfiguration : BaseEntityTypeConfiguration<Domain.Entities.Wallet, Guid>
{
    protected override void ConfigureBaseEntity(EntityTypeBuilder<Domain.Entities.Wallet> builder)
    {
        builder.Property(x => x.AccountPrivateKey)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.DepositAddress)
            .IsRequired()
            .HasMaxLength(64); // In case of mainnet, address is 62 in length, but on regtest, it is 64

        builder.Property(x => x.LastSyncedTime)
            .HasColumnType("timestamptz")
            .IsRequired(false);
    }
}