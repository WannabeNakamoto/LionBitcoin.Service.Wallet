using LionBitcoin.Wallet.Cli.Application.Options;
using NBitcoin;

namespace LionBitcoin.Wallet.Cli.Application.Utils;

public static class ApplicationOptionsUtils
{
    extension(ApplicationOptions options)
    {
        public Network Network => options.IsMainNet ? Network.Main : Network.RegTest;
    }
}