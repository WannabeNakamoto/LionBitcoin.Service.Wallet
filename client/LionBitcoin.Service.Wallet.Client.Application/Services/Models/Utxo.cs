namespace LionBitcoin.Service.Wallet.Client.Application.Services.Models;

public class Utxo
{
    public required string TransactionId { get; set; }

    public int OutputIndex { get; set; }

    public ulong Amount { get; set; }

    public required string LockingScriptHex { get; set; }

    public int Confirmations { get; set; }
}