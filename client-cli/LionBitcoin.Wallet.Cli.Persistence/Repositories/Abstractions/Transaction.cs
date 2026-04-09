using LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace LionBitcoin.Wallet.Cli.Persistence.Repositories.Abstractions;

internal record Transaction(IDbContextTransaction DbContextTransaction) : ITransaction
{
    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await DbContextTransaction.DisposeAsync();
    }

    public void Commit()
    {
        DbContextTransaction.Commit();
    }

    public async Task CommitAsync()
    {
        await DbContextTransaction.CommitAsync();
    }
}