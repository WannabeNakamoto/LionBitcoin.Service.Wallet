namespace LionBitcoin.Wallet.Cli.Constants;

public static class Commands
{
    public static readonly Command Help = new("help", "Show this help");
    public static readonly Command Exit = new("exit", "Exit the application");
    public static readonly Command CreateWallet = new(
        "create-wallet",
        "Creates wallet. You only can work with one configured wallet. usage: create-wallet phrase1 phrase2 phrase3 ...");
}

public record Command(string Name, string Description)
{
    public static bool operator ==(string command, Command commandObj)
    {
        return command == commandObj.Name;
    }

    public static bool operator !=(string command, Command commandObj)
    {
        return command != commandObj.Name;
    }
}