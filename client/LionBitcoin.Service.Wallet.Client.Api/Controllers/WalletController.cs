using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LionBitcoin.Service.Wallet.Client.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController(
    IBlockchainInfoService blockchainInfoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await blockchainInfoService.GetUtxos("bc1qztuue9qkmj48zhwxww6rzqm3caz7xtg7kudnv2", cancellationToken));
    }
}