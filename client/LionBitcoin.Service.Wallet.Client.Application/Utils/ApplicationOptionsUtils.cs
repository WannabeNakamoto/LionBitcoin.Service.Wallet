using LionBitcoin.Service.Wallet.Client.Application.Options;
using NBitcoin;

namespace LionBitcoin.Service.Wallet.Client.Application.Utils;

public static class ApplicationOptionsUtils
{
    extension(ApplicationOptions options)
    {
        public Network Network => options.IsMainNet ? Network.Main : Network.RegTest;
    }
}