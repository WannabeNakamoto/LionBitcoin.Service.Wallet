using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;
using Refit;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;

[Headers("Content-Type: application/json")]
public interface IBitcoinCoreClient
{
    [Post("")]
    Task<HttpResponseMessage> ExecuteMethod(
        [Body] BitcoinRpcRequest request,
        [Header("Authorization")] string authHeader,
        CancellationToken cancellationToken);
}