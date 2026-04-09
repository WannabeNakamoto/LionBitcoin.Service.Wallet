using LionBitcoin.Wallet.Cli.Domain.Entities.Abstractions;

namespace LionBitcoin.Wallet.Cli.Domain.Entities;

public class Wallet : BaseEntity<Guid>
{
    public required byte[] AccountPrivateKey { get; set; }

    public required string DepositAddress { get; set; }

    public DateTimeOffset? LastSyncedTime { get; set; }
}