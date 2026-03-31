using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Domain.Entities;

public class Wallet : BaseEntity<Guid>
{
    public required byte[] AccountPrivateKey { get; set; }

    public required string DepositAddress { get; set; }

    public List<Utxo>? Utxos { get; set; }
}