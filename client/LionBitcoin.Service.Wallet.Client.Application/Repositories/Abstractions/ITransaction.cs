namespace LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    void Commit();

    Task CommitAsync();
}