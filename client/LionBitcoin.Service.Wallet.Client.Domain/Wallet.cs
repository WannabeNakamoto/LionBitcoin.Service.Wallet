using LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

namespace LionBitcoin.Service.Wallet.Client.Domain;

public class Wallet : BaseEntity<Guid>
{
    public required byte[] AccountPrivateKey { get; set; }
}