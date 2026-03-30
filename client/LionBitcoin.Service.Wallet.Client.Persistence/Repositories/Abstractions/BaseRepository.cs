using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;

public abstract class BaseRepository<TEntity, TId>(
    WalletClientDbContext dbContext, TimeProvider timeProvider)
    : IBaseRepository<TEntity, TId>
        where TId : IEquatable<TId>
        where TEntity : BaseEntity<TId>
{
    public async Task Insert(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = timeProvider.GetUtcNow();
        dbContext.Set<TEntity>().Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = timeProvider.GetUtcNow();
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task<TEntity> GetById(TId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .AsNoTracking()
            .SingleAsync(e => e.Id.Equals(id), cancellationToken);
    }
}