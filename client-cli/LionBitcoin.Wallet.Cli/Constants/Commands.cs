namespace LionBitcoin.Wallet.Cli.Constants;

public static class Commands
{
    public static readonly Command Help = new("help", "Show this help");
    public static readonly Command Exit = new("exit", "Exit the application");
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