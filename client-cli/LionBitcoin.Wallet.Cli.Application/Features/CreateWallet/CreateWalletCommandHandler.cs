using MediatR;

namespace LionBitcoin.Wallet.Cli.Application.Features.CreateWallet;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand>
{
    public Task Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}