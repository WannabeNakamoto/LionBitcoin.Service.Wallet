namespace LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

public class LockableEntity<T> : BaseEntity<T>
    where T : IEquatable<T>
{
    public bool IsLocked { get; set; }

    public string? IsLockedBy { get; set; }
}