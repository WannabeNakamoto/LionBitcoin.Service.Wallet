using LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient.Models;
using Refit;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient;

public interface IMempoolClient
{
    [Get("/api/address/{address}/utxo")]
    Task<List<Utxo>> GetUtxos([AliasAs("address")] string address, CancellationToken cancellationToken = default);

    [Get("/api/tx/{txId}/hex")]
    Task<HttpResponseMessage> GetTransactionHex(
        [AliasAs("txId")] string transactionId, CancellationToken cancellationToken = default);
}