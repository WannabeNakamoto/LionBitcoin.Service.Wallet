using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateTransaction;

public class CreateTransactionCommand : IRequest<string>
{
    public Guid WalletId { get; set; }

    public ulong Amount { get; set; }

    public ulong Fee { get; set; }

    public required string Destination { get; set; }
}