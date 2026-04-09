namespace LionBitcoin.Wallet.Cli.Application.Services.Abstractions;

public interface IWalletService
{
    string GenerateDepositAddress(byte[] privateKey);

    byte[] GetPrivateKey(string[] mnemonic);
}