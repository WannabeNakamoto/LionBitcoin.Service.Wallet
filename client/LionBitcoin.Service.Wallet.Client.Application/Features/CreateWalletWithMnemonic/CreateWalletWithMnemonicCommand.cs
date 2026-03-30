using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.CreateWalletWithMnemonic;

public class CreateWalletWithMnemonicCommand : IRequest<CreateWalletWithMnemonicResponse>
{
    public string[] Mnemonic { get; set; }
}