using System.Text.Json.Serialization;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.MempoolClient.Models;

public class Utxo
{
    [JsonPropertyName("txid")]
    public required string TransactionId { get; set; }

    /// <summary>
    /// Index of output in the transaction with id: 'TransactionId'
    /// </summary>
    [JsonPropertyName("vout")]
    public int OutputIndex { get; set; }

    /// <summary>
    /// Amount in satoshis. 1 bitcoin = 100,000,000 satoshis.
    /// </summary>
    [JsonPropertyName("value")]
    public ulong Amount { get; set; }

    [JsonPropertyName("status")]
    public required Status UtxoStatus { get; set; }

    public class Status
    {
        [JsonPropertyName("confirmed")]
        public bool IsConfirmed { get; set; }

        [JsonPropertyName("block_height")]
        public uint BlockHeight { get; set; }

        [JsonPropertyName("block_hash")]
        public required string BlockHash { get; set; }

        /// <summary>
        /// Epoch timestamp in seconds.
        /// </summary>
        [JsonPropertyName("block_time")]
        public ulong BlockTimestamp { get; set; }
    }
}