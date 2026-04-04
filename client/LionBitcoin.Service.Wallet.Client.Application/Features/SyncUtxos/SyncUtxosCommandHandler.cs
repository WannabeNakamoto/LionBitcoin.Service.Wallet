using DotNetCore.SharpStreamer.Bus;
using LionBitcoin.Service.Wallet.Client.Application.Repositories;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Models;
using LionBitcoin.Service.Wallet.Client.Application.Utils;
using Medallion.Threading;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.SyncUtxos;

public class SyncUtxosCommandHandler(
    IBlockchainInfoService blockchainInfoService,
    IWalletRepository walletRepository,
    IStreamerBus streamerBus,
    TimeProvider timeProvider,
    IDistributedLockProvider distributedLockProvider,
    IUtxoRepository utxoRepository) : IRequestHandler<SyncUtxosCommand>
{
    public async Task Handle(SyncUtxosCommand request, CancellationToken cancellationToken)
    {
        await using IDistributedSynchronizationHandle _ = await distributedLockProvider.TryLock(
            $"{nameof(SyncUtxosCommandHandler)}_{request.WalletId}",
            cancellationToken);

        Domain.Entities.Wallet wallet = await walletRepository.GetWalletById(request.WalletId, includeUtxos: true, cancellationToken);
        if (!wallet.IsSyncNeeded(timeProvider))
        {
            return;
        }

        (List<Domain.Entities.Utxo> utxosToInsert, List<Domain.Entities.Utxo> utxosToDelete) = await GetUtxosToWorkWith(wallet, cancellationToken);

        if (utxosToInsert.Any())
        {
            await utxoRepository.InsertRange(utxosToInsert, cancellationToken);
        }

        if (utxosToDelete.Any())
        {
            await utxoRepository.DeleteRange(utxosToDelete, cancellationToken);
        }

        await streamerBus.PublishDelayedAsync(request, TimeSpan.FromMinutes(5));

        wallet.LastSyncedTime = timeProvider.GetUtcNow();
        await walletRepository.Update(wallet, cancellationToken);
    }

    private async Task<(List<Domain.Entities.Utxo> utxosToInsert, List<Domain.Entities.Utxo> utxosToDelete)> GetUtxosToWorkWith(Domain.Entities.Wallet wallet, CancellationToken cancellationToken)
    {
        List<Utxo> utxos = await blockchainInfoService.GetUtxos(wallet.DepositAddress, cancellationToken);

        List<Domain.Entities.Utxo> utxoEntities = utxos.Select(utxo =>
            new Domain.Entities.Utxo
            {
                TransactionId = Convert.FromHexString(utxo.TransactionId),
                Amount = utxo.Amount,
                OutputIndex = utxo.OutputIndex,
                BlockHeight = utxo.Height,
                WalletId = wallet.Id,
            }).ToList();

        List<Domain.Entities.Utxo> utxosToInsert = utxoEntities
            .AsParallel()
            .WithDegreeOfParallelism(utxoEntities.Count >= 10 ? utxoEntities.Count / 10 : 1) // It prevents case when count < 10, in that case count / 10 = 0 and degree of parallelism cannot be 0
            .Where(u => !wallet.Utxos!.Any(wu => wu.IsEquivalent(u)))
            .ToList();
        List<Domain.Entities.Utxo> utxosToDelete = wallet.Utxos!
            .AsParallel()
            .WithDegreeOfParallelism(utxoEntities.Count >= 10 ? utxoEntities.Count / 10 : 1) // It prevents case when count < 10, in that case count / 10 = 0 and degree of parallelism cannot be 0
            .Where(u => !utxoEntities.Any(wu => wu.IsEquivalent(u)))
            .ToList();
        return (utxosToInsert, utxosToDelete);
    }
}