namespace LionBitcoin.Wallet.Cli.Application.Repositories.Abstractions;

public interface IUnitOfWork
{
    ITransaction BeginTransaction();

    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}