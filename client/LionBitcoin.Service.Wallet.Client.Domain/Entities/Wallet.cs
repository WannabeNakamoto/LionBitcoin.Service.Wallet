using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Domain.Entities;

public class Wallet : BaseEntity<Guid>
{
    public required byte[] AccountPrivateKey { get; set; }

    public required string DepositAddress { get; set; }

    public DateTimeOffset? LastSyncedTime { get; set; }

    public List<Utxo>? Utxos { get; set; }

    public bool IsSyncNeeded(TimeProvider timeProvider)
    {
        if (LastSyncedTime is null)
        {
            return true;
        }

        DateTimeOffset currentTimeUtc = timeProvider.GetUtcNow();
        return currentTimeUtc - LastSyncedTime > TimeSpan.FromMinutes(2);
    }
}