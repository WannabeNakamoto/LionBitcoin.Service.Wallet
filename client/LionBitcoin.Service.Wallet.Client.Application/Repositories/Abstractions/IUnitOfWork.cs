namespace LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;

public interface IUnitOfWork
{
    ITransaction BeginTransaction();

    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}