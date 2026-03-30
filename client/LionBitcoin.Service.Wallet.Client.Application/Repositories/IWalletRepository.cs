using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Application.Repositories;

public interface IWalletRepository : IBaseRepository<Domain.Wallet, Guid>;