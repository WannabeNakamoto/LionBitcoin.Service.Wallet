using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Domain.Entities;
using LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories;

public class UtxoRepository(
    WalletClientDbContext dbContext,
    TimeProvider timeProvider) : BaseRepository<Utxo, Guid>(dbContext, timeProvider), IUtxoRepository
{
    
}