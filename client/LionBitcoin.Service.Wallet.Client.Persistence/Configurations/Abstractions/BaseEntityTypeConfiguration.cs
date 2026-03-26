using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Configurations.Abstractions;

public abstract class BaseEntityTypeConfiguration<TEntity, TEntityId> : IEntityTypeConfiguration<TEntity>
    where TEntityId : struct
    where TEntity : BaseEntity<TEntityId>
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz").IsRequired(false);

        builder.HasIndex(x => x.CreatedAt, $"ix_{typeof(TEntity).Name.ToLower()}s_created_at_index").HasMethod("brin");
        ConfigureBaseEntity(builder);
    }

    protected abstract void ConfigureBaseEntity(EntityTypeBuilder<TEntity> builder);
}