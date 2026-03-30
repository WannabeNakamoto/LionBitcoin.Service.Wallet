namespace LionBitcoin.Service.Wallet.Client.Domain.Abstractions;

public abstract class BaseEntity<T> where T : IEquatable<T>
{
    public T Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}