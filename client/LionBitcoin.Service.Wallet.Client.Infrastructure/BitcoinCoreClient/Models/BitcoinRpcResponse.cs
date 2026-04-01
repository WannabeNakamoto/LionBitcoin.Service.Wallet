using System.Text.Json;
using System.Text.Json.Serialization;

namespace LionBitcoin.Service.Wallet.Client.Infrastructure.BitcoinCoreClient.Models;

public class BitcoinRpcResponse<T>
{
    [JsonPropertyName("result")]
    public T? Resut { get; set; }

    [JsonPropertyName("error")]
    public Error? ErrorData { get; set; }

    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public required string Message { get; set; }
    }
}

public static class BitcoinRpcResponse
{
    public static BitcoinRpcResponse<T> Create<T>(string response)
    {
        return JsonSerializer.Deserialize<BitcoinRpcResponse<T>>(response)!;
    }
}