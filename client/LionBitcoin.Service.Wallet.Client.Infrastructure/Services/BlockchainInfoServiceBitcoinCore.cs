using LionBitcoin.Service.Wallet.Client.Application.Options;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Utils;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Enums;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NBitcoin;
using Utxo = LionBitcoin.Service.Wallet.Client.Application.Services.Models.Utxo;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.Services;

public class BlockchainInfoServiceBitcoinCore(
    ILogger<BlockchainInfoServiceBitcoinCore> logger,
    IOptions<ApplicationOptions> applicationOptions,
    IBitcoinCoreClient bitcoinCoreClient) : IBlockchainInfoService
{
    public async Task<List<Utxo>> GetUtxos(string address, CancellationToken cancellationToken = default)
    {
        List<BitcoinCoreClient.Models.Utxo> utxos = await GetUtxoSet(address, cancellationToken);
        Utxo[] result = await Task.WhenAll(utxos.Select(async utxoFromBitcoinCore =>
        {
            TxOut output = await GetOutput(utxoFromBitcoinCore, cancellationToken);
            return new Utxo
            {
                Amount = utxoFromBitcoinCore.Amount,
                TransactionId = utxoFromBitcoinCore.TransactionId,
                OutputIndex = utxoFromBitcoinCore.OutputIndex,
                LockingScriptHex = output.ScriptPubKey.ToHex(),
                Confirmations = 0, // TODO: calculate confirmations based on bitcoin core data
            };
        }));

        return result.ToList();
    }

    private async Task<List<BitcoinCoreClient.Models.Utxo>> GetUtxoSet(string address, CancellationToken cancellationToken)
    {
        BitcoinRpcRequest request = new BitcoinRpcRequest
        {
            MethodType = MethodType.GetUtxos,
            Params = [
                "start",
                new List<object>
                {
                    new { desc = $"addr({address})"}
                }
            ]
        };
        HttpResponseMessage response = await bitcoinCoreClient.ExecuteMethod(request, cancellationToken);

        string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError(
                "There was error while fetching utxos for address: {Address}. Response: {Response}, status code: {StatusCode}",
                address,
                rawResponse,
                response.StatusCode);
            throw new Exception("Error fetching utxos from bitcoin core");
        }

        
    }

    private async Task<TxOut> GetOutput(BitcoinCoreClient.Models.Utxo utxoFromBitcoinCore, CancellationToken cancellationToken)
    {
        string transactionHex = await GetRawTransactionHex(utxoFromBitcoinCore.TransactionId, cancellationToken);
        Transaction transaction = Transaction.Parse(transactionHex, applicationOptions.Value.Network);
        TxOut output = transaction.Outputs[utxoFromBitcoinCore.OutputIndex];
        return output;
    }

    private async Task<string> GetRawTransactionHex(
        string txId,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response =
            await bitcoinCoreClient.GetTransactionHex(txId, cancellationToken);

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
        throw new Exception("Error fetching transaction hex from bitcoin core");
    }
}