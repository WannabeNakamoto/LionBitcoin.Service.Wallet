using LionBitcoin.Wallet.Cli.Application.Exceptions;
using LionBitcoin.Wallet.Cli.Application.Repositories;
using MediatR;

namespace LionBitcoin.Wallet.Cli.Application.Features.CreateWallet;

public class CreateWalletCommandHandler(
    IWalletRepository walletRepository)
    : IRequestHandler<CreateWalletCommand>
{
    public async Task Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (await walletRepository.Any(cancellationToken))
        {
            throw new LionBitcoinException("You already have added one wallet");
        }

        throw new NotImplementedException();
    }
}