namespace LionBitcoin.Service.Wallet.Client.Domain.Utils;

public static class BytesUtils
{
    extension(byte[] arr)
    {
        public bool IsEquivalent(byte[] arr2)
        {
            if (arr.Length != arr2.Length)
            {
                return false;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}