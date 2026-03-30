using DotNetCore.SharpStreamer.Bus;
using LionBitcoin.Service.Wallet.Client.Application.Features.SyncUtxos;
using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Repositories.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateWalletWithMnemonic;

public class CreateWalletWithMnemonicCommandHandler(
    IWalletService walletService,
    IWalletRepository walletRepository,
    IStreamerBus streamerBus,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateWalletWithMnemonicCommand, CreateWalletWithMnemonicResponse>
{
    public async Task<CreateWalletWithMnemonicResponse> Handle(CreateWalletWithMnemonicCommand request, CancellationToken cancellationToken)
    {
        byte[] privateKey = walletService.GetPrivateKey(request.Mnemonic);
        Domain.Entities.Wallet wallet = new Domain.Entities.Wallet
        {
            AccountPrivateKey = privateKey,
            DepositAddress = walletService.GenerateDepositAddress(privateKey),
        };

        await using (ITransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            await walletRepository.Insert(wallet, cancellationToken);
            await streamerBus.PublishAsync(new SyncUtxosCommand(wallet.Id), Guid.NewGuid().ToString());
            await transaction.CommitAsync();
        }

        return new CreateWalletWithMnemonicResponse
        {
            WalletId = wallet.Id,
        };
    }
}