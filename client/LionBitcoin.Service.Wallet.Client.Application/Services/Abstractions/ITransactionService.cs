using LionBitcoin.Service.Wallet.Client.Application.Services.Models;

namespace LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;

public interface ITransactionService
{
    string BuildSignedTransaction(BuildSignedTransactionParams parameters);
}