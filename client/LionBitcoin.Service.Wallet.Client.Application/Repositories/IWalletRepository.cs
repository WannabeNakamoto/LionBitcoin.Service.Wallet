using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Application.Repositories;

public interface IWalletRepository : IBaseRepository<Domain.Entities.Wallet, Guid>
{
    Task<Domain.Entities.Wallet> GetWalletById(
        Guid id,
        bool includeUtxos = false,
        CancellationToken cancellationToken = default);
}