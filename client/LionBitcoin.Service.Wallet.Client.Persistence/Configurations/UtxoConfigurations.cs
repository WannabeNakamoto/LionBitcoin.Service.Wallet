using LionBitcoin.Service.Wallet.Client.Domain.Entities;
using LionBitcoin.Service.Wallet.Client.Persistence.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Configurations;

public class UtxoConfigurations : BaseEntityTypeConfiguration<Utxo, Guid>
{
    protected override void ConfigureBaseEntity(EntityTypeBuilder<Utxo> builder)
    {
        builder.Property(x => x.TransactionId)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.OutputIndex)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.Property(x => x.BlockHeight)
            .IsRequired();

        builder.HasOne<Domain.Entities.Wallet>(x => x.Wallet)
            .WithMany(x => x.Utxos)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}