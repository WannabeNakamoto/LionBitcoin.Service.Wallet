using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;
using LionBitcoin.Service.Wallet.Client.Domain.Utils;

namespace LionBitcoin.Service.Wallet.Client.Domain.Entities;

public class Utxo : BaseEntity<Guid>
{
    public required byte[] TransactionId { get; set; }

    public int OutputIndex { get; set; }

    /// <summary>
    /// Amount is represented in satoshis.
    /// </summary>
    public ulong Amount { get; set; }

    public uint BlockHeight { get; set; }

    public Guid WalletId { get; set; }

    public Wallet? Wallet { get; set; }

    public bool IsEquivalent(Utxo utxo)
    {
        return utxo.TransactionId.IsEquivalent(TransactionId) && utxo.OutputIndex == OutputIndex;
    }
}