namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Options;

public class BitcoinCoreClientOptions
{
    public required string BaseUrl { get; set; }

    public required string User { get; set; }

    public required string Password { get; set; }
}