namespace LionBitcoin.Wallet.Cli.Application.Services.Abstractions;

public interface IScriptService
{
    byte[] GenerateDepositAddressLockingScript(byte[] privateKey);
}