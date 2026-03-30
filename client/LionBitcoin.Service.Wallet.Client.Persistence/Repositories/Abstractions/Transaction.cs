using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;

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