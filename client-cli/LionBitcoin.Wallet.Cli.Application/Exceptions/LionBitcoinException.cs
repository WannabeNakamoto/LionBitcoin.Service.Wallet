namespace LionBitcoin.Wallet.Cli.Application.Exceptions;

public class LionBitcoinException(string message, Exception? innerException = null)
    : Exception(message, innerException);