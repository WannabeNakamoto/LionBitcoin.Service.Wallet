using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;

public interface IBaseRepository<TEntity, in TId>
    where TId : IEquatable<TId>
    where TEntity : BaseEntity<TId>
{
    Task Insert(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertRange(List<TEntity> entities, CancellationToken cancellationToken = default);

    Task Update(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> GetById(TId id, CancellationToken cancellationToken = default);

    Task DeleteRange(List<TEntity> entitiesToDelete, CancellationToken cancellationToken = default);
}