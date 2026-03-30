using LionBitcoin.Service.Wallet.Client.Application.Features.CreateWalletWithMnemonic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LionBitcoin.Service.Wallet.Client.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateWallet(
        [FromBody] CreateWalletWithMnemonicCommand command,
        CancellationToken cancellationToken)
    {
        CreateWalletWithMnemonicResponse response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}