using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories;

public class WalletRepository(
    WalletClientDbContext dbContext, TimeProvider timeProvider)
    : BaseRepository<Domain.Wallet, Guid>(dbContext, timeProvider), IWalletRepository;