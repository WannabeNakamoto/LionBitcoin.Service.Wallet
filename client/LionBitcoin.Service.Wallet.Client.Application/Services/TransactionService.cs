using LionBitcoin.Service.Wallet.Client.Application.Options;
using LionBitcoin.Service.Wallet.Client.Application.Services.Abstractions;
using LionBitcoin.Service.Wallet.Client.Application.Services.Models;
using LionBitcoin.Service.Wallet.Client.Application.Utils;
using Microsoft.Extensions.Options;
using NBitcoin;
using NBitcoin.Crypto;
using Utxo = LionBitcoin.Service.Wallet.Client.Domain.Entities.Utxo;

namespace LionBitcoin.Service.Wallet.Client.Application.Services;

public class TransactionService(
    IOptions<ApplicationOptions> applicationOptions,
    IWalletService walletService) : ITransactionService
{
    public string BuildSignedTransaction(BuildSignedTransactionParams parameters)
    {
        Network network = applicationOptions.Value.Network;
        Key key = new Key(parameters.PrivateKey);
        BitcoinAddress depositAddress = BitcoinAddress.Create(walletService.GenerateDepositAddress(parameters.PrivateKey), network);

        Script witnessScript = new Script(
            Op.GetPushOp(key.PubKey.ToBytes()),
            OpcodeType.OP_CHECKSIG);

        Transaction tx = network.CreateTransaction();
        tx.Version = 2; // Just random version number. it does not metter in our case.

        ulong totalInputsAmount = 0;
        foreach (Utxo utxo in parameters.Utxos)
        {
            AddInput(utxo, tx, ref totalInputsAmount);
        }

        AddDestinationOutput(parameters, network, tx);

        AddChangeOutput(parameters, totalInputsAmount, depositAddress, tx);

        for (int i = 0; i < tx.Inputs.Count; i++)
        {
            SignOutput(parameters, i, tx, witnessScript, depositAddress, key);
        }

        return tx.ToHex();
    }

    private static void SignOutput(BuildSignedTransactionParams parameters, int i, Transaction tx, Script witnessScript,
        BitcoinAddress depositAddress, Key key)
    {
        Money inputAmount = new Money((long)parameters.Utxos[i].Amount);

        uint256 sighash = tx.GetSignatureHash(
            witnessScript,
            i,
            SigHash.All,
            new TxOut(inputAmount, depositAddress.ScriptPubKey),
            HashVersion.WitnessV0);

        TransactionSignature sig = key.Sign(sighash, new SigningOptions(SigHash.All, true));

        tx.Inputs[i].ScriptSig = Script.Empty;
        tx.Inputs[i].WitScript = new WitScript(
            Op.GetPushOp(sig.ToBytes()), // Signature bytes
            Op.GetPushOp(witnessScript.ToBytes())); // Actual witness script
    }

    private static void AddInput(Utxo utxo, Transaction tx, ref ulong totalInputsAmount)
    {
        // Reversing because in transaction, it should be little endian and our library's uint256 needs little endian value
        byte[] txId = utxo.TransactionId.Reverse().ToArray();
        OutPoint outPoint = new OutPoint(
            new uint256(txId),
            (uint)utxo.OutputIndex);

        tx.Inputs.Add(new TxIn(outPoint)
        {
            Sequence = Sequence.Final
        });

        totalInputsAmount += utxo.Amount;
    }

    private static void AddDestinationOutput(BuildSignedTransactionParams parameters, Network network, Transaction tx)
    {
        BitcoinAddress destination = BitcoinAddress.Create(parameters.DestinationAddress, network);
        tx.Outputs.Add(new TxOut(new Money((long)parameters.Amount), destination.ScriptPubKey));
    }

    private void AddChangeOutput(
        BuildSignedTransactionParams parameters,
        ulong totalInputsAmount,
        BitcoinAddress depositAddress,
        Transaction tx)
    {
        ulong change = totalInputsAmount - parameters.Amount - parameters.Fees;
        // Change goes to same deposit address
        tx.Outputs.Add(new TxOut(new Money((long)change), depositAddress.ScriptPubKey));
    }
}