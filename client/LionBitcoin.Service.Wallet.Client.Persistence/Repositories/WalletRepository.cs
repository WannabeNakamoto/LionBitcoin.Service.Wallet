using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories;

public class WalletRepository(
    WalletClientDbContext dbContext,
    TimeProvider timeProvider)
    : BaseRepository<Domain.Entities.Wallet, Guid>(dbContext, timeProvider), IWalletRepository
{
    public async Task<Domain.Entities.Wallet> GetWalletById(Guid id, bool includeUtxos = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Domain.Entities.Wallet> wallets = DbContext.Wallets.AsNoTracking();

        if (includeUtxos)
        {
            wallets = wallets.Include(w => w.Utxos);
        }

        return await wallets.SingleAsync(w => w.Id.Equals(id), cancellationToken);
    }
}