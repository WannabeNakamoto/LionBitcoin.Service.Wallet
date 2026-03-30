namespace LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;

public interface IWalletService
{
    string GenerateDepositAddress(byte[] privateKey);

    byte[] GetPrivateKey(string[] mnemonic);
}