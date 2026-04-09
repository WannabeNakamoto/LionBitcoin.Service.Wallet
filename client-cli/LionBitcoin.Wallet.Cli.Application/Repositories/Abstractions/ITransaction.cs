namespace LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    void Commit();

    Task CommitAsync();
}