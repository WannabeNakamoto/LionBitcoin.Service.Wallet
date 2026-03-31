using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;
using Refit;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient;

public interface IBitcoinCoreClient
{
    [Post("")]
    Task<HttpResponseMessage> ExecuteMethod([Body] BitcoinRpcRequest request, CancellationToken cancellationToken);
}