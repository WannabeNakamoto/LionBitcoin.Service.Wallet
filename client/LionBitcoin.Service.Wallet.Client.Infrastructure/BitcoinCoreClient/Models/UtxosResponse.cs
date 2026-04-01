using System.Text.Json.Serialization;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;

public class UtxosResponse
{
    [JsonPropertyName("height")]
    public int ChainHeight { get; set; }

    [JsonPropertyName("unspents")]
    public List<Utxo> Utxos { get; set; }

    [JsonPropertyName("total_amount")]
    public decimal Balance { get; set; }
    public class Utxo
    {
        [JsonPropertyName("txid")]
        public required string TransactionId { get; set; }

        [JsonPropertyName("vout")]
        public int OutputIndex { get; set; }

        [JsonPropertyName("scriptPubKey")]
        public required string ScriptPubKey { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }
    }
}