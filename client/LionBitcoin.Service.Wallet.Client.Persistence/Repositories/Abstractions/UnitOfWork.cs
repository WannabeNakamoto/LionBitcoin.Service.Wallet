using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LionBitcoin.Service.Wallet.Client.Persistence.Repositories.Abstractions;

public sealed class UnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork
    where TDbContext : DbContext
{
    public ITransaction BeginTransaction()
    {
        return new Transaction(dbContext.Database.BeginTransaction());
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new Transaction(transaction);
    }
}