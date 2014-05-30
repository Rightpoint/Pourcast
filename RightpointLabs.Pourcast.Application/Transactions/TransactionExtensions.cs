using System;

namespace RightpointLabs.Pourcast.Application.Transactions
{
    using System.Transactions;

    public static class TransactionExtensions
    {
        public static void WaitForTransactionCompleted(Action action)
        {
            if (Transaction.Current != null)
            {
                Transaction.Current.TransactionCompleted += (sender, args) => action();
            }
            else
            {
                action();
            }
        }
    }
}
