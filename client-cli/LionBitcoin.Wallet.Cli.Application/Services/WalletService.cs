using System.Security.Cryptography;
using LionBitcoin.Wallet.Cli.Application.Options;
using LionBitcoin.Wallet.Cli.Application.Services.Abstractions;
using LionBitcoin.Wallet.Cli.Application.Utils;
using Microsoft.Extensions.Options;
using NBitcoin;

namespace LionBitcoin.Wallet.Cli.Application.Services;

public class WalletService(
    IScriptService scriptService,
    IOptions<ApplicationOptions> applicationOptions)
    : IWalletService
{
    public string GenerateDepositAddress(byte[] privateKey)
    {
        byte[] lockingScript = scriptService.GenerateDepositAddressLockingScript(privateKey);
        Network network = applicationOptions.Value.Network;
        byte[] lockingScriptHash = SHA256.HashData(lockingScript);
        return network.GetBech32Encoder(Bech32Type.WITNESS_SCRIPT_ADDRESS, throws: true)!
            .Encode(witnessVerion: 0, witnessProgramm: lockingScriptHash);
        // witnessVersion: 0 means that it is segwit (version: 0) and not taproot (version: 1) because in case of taproot
        // there is difference encoding used. WITNESS_SCRIPT_ADDRESS tells that address should encode ( OP_0 [script hash - 32 bytes] )
        // address because it is p2wsh. If we wanted p2wpkh, we would use WITNESS_PUBKEY_ADDRESS
    }

    public byte[] GetPrivateKey(string[] mnemonic)
    {
        Mnemonic m = new Mnemonic(string.Join(' ', mnemonic), Wordlist.English);
        return m.DeriveExtKey().PrivateKey.ToBytes();
    }
}