using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Models;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateTransaction;

public class CreateTransactionCommandHandler(
    ITransactionService transactionService,
    IWalletRepository walletRepository,
    IBroadcastService broadcastService)
    : IRequestHandler<CreateTransactionCommand, string>
{
    public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Wallet wallet = await walletRepository.GetWalletById(request.WalletId, includeUtxos: true, cancellationToken);
        string transactionHex = transactionService.BuildSignedTransaction(new BuildSignedTransactionParams
        {
            Utxos = wallet.Utxos!,
            PrivateKey = wallet.AccountPrivateKey,
            DestinationAddress = request.Destination,
            Amount = request.Amount,
            Fees = request.Fee,
        });
        return await broadcastService.BroadcastAsync(transactionHex, cancellationToken);
    }
}