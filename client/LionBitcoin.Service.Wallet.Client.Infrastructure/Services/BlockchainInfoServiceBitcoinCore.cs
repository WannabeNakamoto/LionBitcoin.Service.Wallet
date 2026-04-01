using System.Text;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Enums;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utxo = LionBitcoin.Service.Wallet.Client.Application.Services.Models.Utxo;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.Services;

public class BlockchainInfoServiceBitcoinCore(
    ILogger<BlockchainInfoServiceBitcoinCore> logger,
    IOptions<BitcoinCoreClientOptions> bitcoinCoreClientOptions,
    IBitcoinCoreClient bitcoinCoreClient) : IBlockchainInfoService
{
    public async Task<List<Utxo>> GetUtxos(string address, CancellationToken cancellationToken = default)
    {
        UtxosResponse utxosResponse = await GetUtxoSet(address, cancellationToken);
        List<Utxo> result = utxosResponse.Utxos.Select(utxoFromBitcoinCore =>
            new Utxo
            {
                Amount = (ulong)(utxoFromBitcoinCore.Amount * 100_000_000),
                TransactionId = utxoFromBitcoinCore.TransactionId,
                OutputIndex = utxoFromBitcoinCore.OutputIndex,
                LockingScriptHex = utxoFromBitcoinCore.ScriptPubKey,
                Confirmations = utxosResponse.ChainHeight - utxoFromBitcoinCore.Height + 1,
                Height = utxoFromBitcoinCore.Height
            }).ToList();

        return result;
    }

    private async Task<UtxosResponse> GetUtxoSet(string address, CancellationToken cancellationToken)
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

        string auth =
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    $"{bitcoinCoreClientOptions.Value.User}:{bitcoinCoreClientOptions.Value.Password}"));
        HttpResponseMessage response = await bitcoinCoreClient.ExecuteMethod(request, $"Basic {auth}", cancellationToken);

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

        BitcoinRpcResponse<UtxosResponse> parsedResponse = BitcoinRpcResponse.Create<UtxosResponse>(rawResponse);
        return parsedResponse.Resut!;
    }
}