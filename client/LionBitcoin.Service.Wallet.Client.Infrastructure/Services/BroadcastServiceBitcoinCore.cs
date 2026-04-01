using System.Text;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Enums;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Options;
using Microsoft.Extensions.Options;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.Services;

public class BroadcastServiceBitcoinCore(
    IOptions<BitcoinCoreClientOptions> options,
    IBitcoinCoreClient client) : IBroadcastService
{
    public async Task<string> BroadcastAsync(string rawTxHex, CancellationToken cancellationToken = default)
    {
        BitcoinRpcRequest request = new BitcoinRpcRequest
        {
            MethodType = MethodType.SendRawTransaction,
            Params     = [rawTxHex]
        };

        string auth = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{options.Value.User}:{options.Value.Password}"));

        HttpResponseMessage response = await client.ExecuteMethod(request, $"Basic {auth}", cancellationToken);
        string raw = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"send raw transaction failed: {raw}");
        }

        BitcoinRpcResponse<string> parsed = BitcoinRpcResponse.Create<string>(raw);
        return parsed.Resut!;
    }
}