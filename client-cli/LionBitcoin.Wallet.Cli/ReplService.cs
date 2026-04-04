using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace LionBitcoin.Wallet.Cli;

internal class ReplService(IHostApplicationLifetime lifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ShowHeader();

        while (!stoppingToken.IsCancellationRequested)
        {
            var input = await ReadInputAsync(stoppingToken);

            if (input is null) break; // Ctrl+C or stream closed
            if (string.IsNullOrWhiteSpace(input)) continue;

            var quit = await HandleAsync(input.Trim(), stoppingToken);
            if (quit) break;
        }

        lifetime.StopApplication();
    }

    private static async Task<bool> HandleAsync(string input, CancellationToken ct)
    {
        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLowerInvariant();
        var rest = parts.Length > 1 ? parts[1] : string.Empty;

        switch (command)
        {
            case "help":
                ShowHelp();
                break;

            case "exit":
            case "quit":
                AnsiConsole.MarkupLine("[grey]Goodbye.[/]");
                return true;

            default:
                AnsiConsole.MarkupLine($"[red]Unknown command:[/] [white]{command}[/]. Type [grey]help[/] for available commands.");
                break;
        }

        return false;
    }

    private static async Task<string?> ReadInputAsync(CancellationToken cancellationToken)
    {
        return await AnsiConsole.PromptAsync(
            new TextPrompt<string>("[bold gold1]>[/] ")
                .AllowEmpty(),
            cancellationToken);
    }

    private static void ShowHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("LionBitcoin Wallet").Color(Color.Gold1));
        AnsiConsole.MarkupLine("Type [grey]help[/] for available commands, [grey]exit[/] to quit.\n");
    }

    private static void ShowHelp()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[grey]Command[/]")
            .AddColumn("[grey]Description[/]");

        table.AddRow("[white]help[/]", "Show this help");
        table.AddRow("[white]exit[/]", "Exit the application");

        AnsiConsole.Write(table);
    }
}
