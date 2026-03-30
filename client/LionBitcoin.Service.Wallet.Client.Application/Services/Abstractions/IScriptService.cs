namespace LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;

public interface IScriptService
{
    byte[] GenerateDepositAddressLockingScript(byte[] privateKey);
}