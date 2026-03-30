using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Domain.Entities;

public class Utxo : BaseEntity<Guid>
{
    public required string TransactionId { get; set; }

    public int OutputIndex { get; set; }

    /// <summary>
    /// Amount is represented in satoshis.
    /// </summary>
    public ulong Amount { get; set; }

    public uint BlockHeight { get; set; }
}