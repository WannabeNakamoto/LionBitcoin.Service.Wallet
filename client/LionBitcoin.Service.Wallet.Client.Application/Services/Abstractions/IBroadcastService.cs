namespace LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;

public interface IBroadcastService
{
    /// <summary>
    /// Returns transaction id
    /// </summary>
    Task<string> BroadcastAsync(string rawTxHex, CancellationToken cancellationToken = default);
}