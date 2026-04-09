using System.Reflection;
using LionBitcoin.Wallet.Cli.Application.Features.CreateWallet;
using LionBitcoin.Wallet.Cli.Constants;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace LionBitcoin.Wallet.Cli;

internal class ReplService(
    IHostApplicationLifetime lifetime,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ShowHeader();

        while (!stoppingToken.IsCancellationRequested)
        {
            string input = await ReadInputAsync(stoppingToken);

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            bool isQuitCommand = await HandleAsync(input.Trim(), stoppingToken);
            if (isQuitCommand)
            {
                break;
            }
        }

        lifetime.StopApplication();
    }

    private async Task<bool> HandleAsync(string input, CancellationToken cancellationToken)
    {
        string[] parts = input.Split(' ', 2); // First is command and second are parameters.
        string command = parts[0].ToLower();
        string? parameters = parts.Length > 1 ? parts[1] : null;

        if (command == Commands.Help)
        {
            ShowHelp();
        }else if (command == Commands.Exit)
        {
            AnsiConsole.MarkupLine("[grey]Goodbye.[/]");
            return true;
        }
        else if(command == Commands.CreateWallet)
        {
            List<string> seedPhrase = parts[1].Split(' ').ToList();
            CreateWalletCommand c = new() { SeedPhrase = seedPhrase };
            await RunThroughMediator(c, cancellationToken);
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Unknown command:[/] [white]{command}[/]. Type [grey]{Commands.Help}[/] for available commands.");
        }

        return false;
    }

    private static async Task<string> ReadInputAsync(CancellationToken cancellationToken)
    {
        return await AnsiConsole.PromptAsync(
            new TextPrompt<string>("[bold gold1]>[/] "),
            cancellationToken);
    }

    private static void ShowHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("LionBitcoin Wallet").Color(Color.Gold1));
        AnsiConsole.MarkupLine($"Type [grey]{Commands.Help.Name}[/] for available commands, [grey]{Commands.Exit.Name}[/] to quit.\n");
    }

    private static void ShowHelp()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[grey]Command[/]")
            .AddColumn("[grey]Description[/]");

        Type commandType = typeof(Command);
        IEnumerable<FieldInfo> commands = typeof(Commands)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f is { IsStatic: true, IsInitOnly: true } &&
                        f.FieldType == commandType);
        foreach (FieldInfo command in commands)
        {
            table.AddRow($"[white]{((Command)command.GetValue(null)!).Name}[/]", ((Command)command.GetValue(null)!).Description);
        }
        

        AnsiConsole.Write(table);
    }

    private async Task RunThroughMediator<T>(T command, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        try
        {
            await mediator.Send(command!, cancellationToken);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
        }
    }
}
