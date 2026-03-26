using LionBitcoin.Service.Wallet.Client.Persistence.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Configurations;

public class WalletConfigurations : BaseEntityTypeConfiguration<Domain.Wallet, Guid>
{
    protected override void ConfigureBaseEntity(EntityTypeBuilder<Domain.Wallet> builder)
    {
        builder.Property(x => x.AccountPrivateKey)
            .IsRequired()
            .HasMaxLength(32);
    }
}