using DotNetCore.SharpStreamer.Bus.Attributes;
using MediatR;

namespace LionBitcoin.Service.Wallet.Client.Application.Features.SyncUtxos;

[PublishEvent("sync_utxos", "wallet")]
[ConsumeEvent("sync_utxos", checkPredecessor: false)]
public record SyncUtxosCommand(Guid WalletId) : IRequest;