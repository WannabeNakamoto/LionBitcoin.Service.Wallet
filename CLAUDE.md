# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

**Build:**
```bash
dotnet build
```

**Run Client API:**
```bash
dotnet run --project client/LionBitcoin.Service.Wallet.Client.Api/LionBitcoin.Service.Wallet.Client.Api.csproj
```

**Run Server API:**
```bash
dotnet run --project server/LionBitcoin.Service.Wallet.Api/LionBitcoin.Service.Wallet.Api.csproj
```

**Database migrations (EF Core):**
```bash
dotnet ef migrations add <MigrationName> --project client/LionBitcoin.Service.Wallet.Client.Persistence --startup-project client/LionBitcoin.Service.Wallet.Client.Api
dotnet ef database update --project client/LionBitcoin.Service.Wallet.Client.Persistence --startup-project client/LionBitcoin.Service.Wallet.Client.Api
```

**Start local infrastructure:**
```bash
# PostgreSQL
docker compose -f client/LionBitcoin.Service.Wallet.Client.Api/LionBitcoinWalletClientInfra.yaml up -d

# Bitcoin RegTest node
docker compose -f client/LionBitcoin.Service.Wallet.Client.Api/bitcoin-env.yaml up -d
```

**Bootstrap RegTest wallet and mine initial blocks:**
```bash
docker compose exec bitcoind bitcoin-cli -regtest -rpcuser=admin -rpcpassword=admin createwallet "dev"
docker compose exec bitcoind bitcoin-cli -rpcwallet="dev" -regtest -rpcuser=admin -rpcpassword=admin getnewaddress
docker compose exec bitcoind bitcoin-cli -regtest -rpcuser=admin -rpcpassword=admin generatetoaddress 101 <address>
```

## Architecture

The solution has two top-level services: **client** (fully implemented) and **server** (stub/placeholder). All active development is in the `client/` tree.

### Client service layer structure

```
client/
├── Api          → ASP.NET Core minimal API entry point; wires DI, runs migrations
├── Application  → CQRS commands/handlers (MediatR), domain services, repository interfaces
├── Domain       → Wallet + Utxo entities, business rules
├── Infrastructure → Refit HTTP client (IMempoolClient) for Mempool API
└── Persistence  → EF Core DbContext (PostgreSQL), repository + UoW implementations
```

### Key flows

**Wallet creation** (`POST /api/wallet`):
1. `CreateWalletWithMnemonicCommand` → handler derives HD private key via NBitcoin
2. `ScriptService` builds P2WSH locking script → Bech32 deposit address
3. Wallet persisted; `SyncUtxosCommand` published async via SharpStreamer

**UTXO sync** (triggered on creation + every 5 minutes via delayed SharpStreamer event):
1. Distributed lock (Medallion.Threading) prevents concurrent syncs per wallet
2. `IMempoolClient` queries the Mempool HTTP API (`MempoolClientOptions.BaseUrl`) for UTXOs at the deposit address
3. New UTXOs deduplicated via `IsEquivalent()` and upserted to PostgreSQL
4. Next sync rescheduled with a 5-minute delay event

### Event bus — SharpStreamer

- `@PublishEvent` / `@ConsumeEvent` attributes on command handlers declare the event contract
- Transport and storage are both backed by PostgreSQL (`DotNetCore.SharpStreamer.Storage.Npgsql`, `DotNetCore.SharpStreamer.Transport.Npgsql`)
- `ConsumerGroup`, `ProcessorThreadCount`, `ProcessingBatchSize` tuned in `SharpStreamerSettings`

### Domain entities

| Entity | Notable fields | Key business logic |
|--------|---------------|-------------------|
| `Wallet` | `AccountPrivateKey` (bytes), `DepositAddress`, `LastSyncedTime` | `IsSyncNeeded()` — 2-minute threshold |
| `Utxo` | `TransactionId` (bytes), `OutputIndex`, `Amount` (satoshis), `BlockHeight`, `WalletId` | `IsEquivalent()` — deduplication by txid + index |

### Network selection

`ApplicationOptions.IsMainNet` (bool) in `appsettings.json` controls mainnet vs. regtest/testnet address derivation. EF migrations target the client persistence project.

### DI registration convention

Each layer exposes a single extension method (`AddApplication()`, `AddInfrastructure()`, `AddPersistence()`) that is called from the API project. The C# `extension` keyword is used (static extension methods on `IServiceCollection`).
