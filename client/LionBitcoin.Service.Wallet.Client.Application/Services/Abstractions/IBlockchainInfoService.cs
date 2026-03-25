using LionBitcoin.Service.Wallet.Client.Application.Services.Models;

namespace LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;

public interface IBlockchainInfoService
{
    Task<List<Utxo>> GetUtxos(string address, CancellationToken cancellationToken = default);
}