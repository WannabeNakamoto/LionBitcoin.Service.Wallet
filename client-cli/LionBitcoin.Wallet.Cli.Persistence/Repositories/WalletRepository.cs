using LionBitcoin.Wallet.Cli.Application.Repositories;
using LionBitcoin.Wallet.Cli.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Wallet.Cli.Persistence.Repositories;

public class WalletRepository(
    LionBitcoinDbContext context,
    TimeProvider timeProvider)
    : BaseRepository<Domain.Entities.Wallet, Guid>(context, timeProvider), IWalletRepository
{
    public async Task<bool> Any(CancellationToken cancellationToken)
    {
        return await DbContext.Wallets.AnyAsync(cancellationToken);
    }
}