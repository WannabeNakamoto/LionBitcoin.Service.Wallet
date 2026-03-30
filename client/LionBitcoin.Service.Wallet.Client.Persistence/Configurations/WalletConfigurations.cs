using LionBitcoin.Service.Wallet.Client.Persistence.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Configurations;

public class WalletConfigurations : BaseEntityTypeConfiguration<Domain.Entities.Wallet, Guid>
{
    protected override void ConfigureBaseEntity(EntityTypeBuilder<Domain.Entities.Wallet> builder)
    {
        builder.Property(x => x.AccountPrivateKey)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.DepositAddress)
            .IsRequired()
            .HasMaxLength(62);
    }
}