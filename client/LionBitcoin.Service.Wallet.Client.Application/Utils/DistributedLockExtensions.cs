using Medallion.Threading;

namespace LionBitcoin.Service.Wallet.Client.Application.Utils;

public static class DistributedLockExtensions
{
    extension(IDistributedLockProvider distributedLockProvider)
    {
        public async Task<IDistributedSynchronizationHandle> TryLock(string name, CancellationToken cancellationToken)
        {
            IDistributedSynchronizationHandle? handle = await distributedLockProvider.TryAcquireLockAsync(
                name,
                cancellationToken: cancellationToken);
            if (handle is null)
            {
                throw new Exception($"{name} already in processing. distributed lock taking failed.");
            }

            return handle;
        }
    }
}