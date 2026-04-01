namespace LionBitcoin.Service.Wallet.Client.Application.Services.Models;

public class BuildSignedTransactionParams
{
    public required List<Domain.Entities.Utxo> Utxos { get; set; }

    public required byte[] PrivateKey { get; set; }

    public required string DestinationAddress { get; set; }

    /// <summary>
    /// Amount is in satoshis
    /// </summary>
    public required ulong Amount {get; set;}

    /// <summary>
    /// Fees is in satoshis
    /// </summary>
    public required ulong Fees { get; set; }
}