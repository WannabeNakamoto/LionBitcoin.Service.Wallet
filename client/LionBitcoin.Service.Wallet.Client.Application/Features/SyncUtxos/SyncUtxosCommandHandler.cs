using DotNetCore.SharpStreamer.Bus;
using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Models;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.SyncUtxos;

public class SyncUtxosCommandHandler(
    IBlockchainInfoService blockchainInfoService,
    IWalletRepository walletRepository,
    IStreamerBus streamerBus,
    IUtxoRepository utxoRepository) : IRequestHandler<SyncUtxosCommand>
{
    public async Task Handle(SyncUtxosCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Wallet wallet = await walletRepository.GetWalletById(request.WalletId, includeUtxos: true, cancellationToken);
        List<Domain.Entities.Utxo> utxos = await GetUtxosToInsert(wallet, cancellationToken);

        if (utxos.Any())
        {
            await utxoRepository.InsertRange(utxos, cancellationToken);
        }

        await streamerBus.PublishDelayedAsync(request, TimeSpan.FromMinutes(5));
    }

    private async Task<List<Domain.Entities.Utxo>> GetUtxosToInsert(Domain.Entities.Wallet wallet, CancellationToken cancellationToken)
    {
        List<Utxo> utxos = await blockchainInfoService.GetUtxos(wallet.DepositAddress, cancellationToken);

        List<Domain.Entities.Utxo> utxoEntities = utxos.Select(utxo =>
            new Domain.Entities.Utxo
            {
                TransactionId = [], // TODO: implement transaction id convention into bytes array
                Amount = utxo.Amount,
                OutputIndex = utxo.OutputIndex,
                BlockHeight = 0, // TODO: implement block height logic
            }).ToList();

        return utxoEntities
            .AsParallel()
            .WithDegreeOfParallelism(utxoEntities.Count >= 10 ? utxoEntities.Count / 10 : 1) // It prevents case when count < 10, in that case count / 10 = 0 and degree of parallelism cannot be 0
            .Where(u => !wallet.Utxos!.Any(wu => wu.IsEquivalent(u)))
            .ToList();
    }
}