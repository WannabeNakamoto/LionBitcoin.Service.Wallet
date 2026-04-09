using LionBitcoin.Wallet.Cli.Application.Services.Abstractions;
using NBitcoin;

namespace LionBitcoin.Wallet.Cli.Application.Services;

public class ScriptService : IScriptService
{
    public byte[] GenerateDepositAddressLockingScript(byte[] privateKey)
    {
        Key key = new Key(privateKey);
        PubKey pubKey = key.PubKey;
        Script lockingScript = new Script(
            Op.GetPushOp(pubKey.ToBytes()), // Pushes pub key as 33 bytes compressed form.
            OpcodeType.OP_CHECKSIG
        );
        return lockingScript.ToBytes();
    }
}