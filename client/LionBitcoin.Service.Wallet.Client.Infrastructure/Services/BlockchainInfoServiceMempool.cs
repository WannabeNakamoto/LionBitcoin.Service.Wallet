using LionBitcoin.Service.Wallet.Client.Application.Options;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Models;
using LionBitcoin.Service.Wallet.Client.Application.Utils;
using LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NBitcoin;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.Services;

public class BlockchainInfoServiceMempool(
    ILogger<BlockchainInfoServiceMempool> logger,
    IOptions<ApplicationOptions> applicationOptions,
    IMempoolClient mempoolClient) : IBlockchainInfoService
{
    public async Task<List<Utxo>> GetUtxos(string address, CancellationToken cancellationToken = default)
    {
        List<MempoolClient.Models.Utxo> utxos = await mempoolClient.GetUtxos(address, cancellationToken);
        Utxo[] result = await Task.WhenAll(utxos.Select(async utxoFromMempool =>
        {
            TxOut output = await GetOutput(utxoFromMempool, cancellationToken);
            return new Utxo
            {
                Amount = utxoFromMempool.Amount,
                TransactionId = utxoFromMempool.TransactionId,
                OutputIndex = utxoFromMempool.OutputIndex,
                LockingScriptHex = output.ScriptPubKey.ToHex(),
                Confirmations = 0, // TODO: calculate confirmations based on mempool data
            };
        }));

        return result.ToList();
    }

    private async Task<TxOut> GetOutput(MempoolClient.Models.Utxo utxoFromMempool, CancellationToken cancellationToken)
    {
        string transactionHex = await GetRawTransactionHex(utxoFromMempool.TransactionId, cancellationToken);
        Transaction transaction = Transaction.Parse(transactionHex, applicationOptions.Value.Network);
        TxOut output = transaction.Outputs[utxoFromMempool.OutputIndex];
        return output;
    }

    private async Task<string> GetRawTransactionHex(
        string txId,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response =
            await mempoolClient.GetTransactionHex(txId, cancellationToken);

        string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return rawResponse;
        }

        logger.LogError(
            "There was error while fetching transaction hex for txId: {TxId}. Response: {Response}, status code: {StatusCode}",
            txId,
            rawResponse,
            response.StatusCode);
        throw new Exception("Error fetching transaction hex from mempool.space");
    }
}