using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateWalletWithMnemonic;

public class CreateWalletWithMnemonicCommandHandler(
    IWalletService walletService,
    IWalletRepository walletRepository,
    TimeProvider timeProvider) : IRequestHandler<CreateWalletWithMnemonicCommand, CreateWalletWithMnemonicResponse>
{
    public async Task<CreateWalletWithMnemonicResponse> Handle(CreateWalletWithMnemonicCommand request, CancellationToken cancellationToken)
    {
        byte[] privateKey = walletService.GetPrivateKey(request.Mnemonic);
        Domain.Wallet wallet = new Domain.Wallet()
        {
            AccountPrivateKey = privateKey,
            CreatedAt = timeProvider.GetUtcNow(),
            UpdatedAt = null,
        };
        await walletRepository.Insert(wallet, cancellationToken);
        return new CreateWalletWithMnemonicResponse
        {
            WalletId = wallet.Id,
        };
    }
}