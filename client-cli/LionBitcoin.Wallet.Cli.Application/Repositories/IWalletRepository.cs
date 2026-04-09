using LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;

namespace LionBitcoin.Wallet.Cli.Application.Repositories;

public interface IWalletRepository : IBaseRepository<Domain.Entities.Wallet, Guid>
{
    Task<bool> Any(CancellationToken cancellationToken);
}