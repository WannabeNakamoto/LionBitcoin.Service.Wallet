using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateWalletWithMnemonic;

public class CreateWalletWithMnemonicCommandHandler(
    IWalletService walletService,
    DbContext dbContext,
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
        dbContext.Set<Domain.Wallet>().Add(wallet);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateWalletWithMnemonicResponse
        {
            WalletId = wallet.Id,
        };
    }
}