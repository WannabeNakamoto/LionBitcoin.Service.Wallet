using System.Text.Json.Serialization;
using LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Enums;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;

public class BitcoinRpcRequest
{
    [JsonPropertyName("jsonrpc")]
    public string RpcVersion { get; } = "1.0";

    [JsonPropertyName("method")]
    public string Method { get; private set; }

    [JsonPropertyName("params")]
    public List<object> Params { get; set; }

    [JsonIgnore]
    public required MethodType MethodType
    {
        get => field;
        set
        {
            Method = value switch
            {
                MethodType.GetUtxos => "scantxoutset",
                _ => throw new ArgumentOutOfRangeException(nameof(MethodType), value, "undefined method type.")
            };
            field = value;
        }
    }
}