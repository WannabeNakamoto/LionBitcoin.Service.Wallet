using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.SyncUtxos;

public class SyncUtxosCommandHandler : IRequestHandler<SyncUtxosCommand>
{
    public Task Handle(SyncUtxosCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}