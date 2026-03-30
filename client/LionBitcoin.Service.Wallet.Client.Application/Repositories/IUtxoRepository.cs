using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using LionBitcoin.Service.Wallet.Client.Domain.Entities;

namespace LionBitcoin.Service.Wallet.Client.Application.Repositories;

public interface IUtxoRepository : IBaseRepository<Utxo, Guid>;