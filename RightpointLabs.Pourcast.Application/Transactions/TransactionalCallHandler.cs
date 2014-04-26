namespace RightpointLabs.Pourcast.Application.Transactions
{
    using System.Transactions;

    using Microsoft.Practices.Unity.InterceptionExtension;

    public class TransactionalCallHandler : ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result;

            using (var scope = new TransactionScope())
            {
                result = getNext()(input, getNext);

                scope.Complete();
            }

            return result;
        }

        public int Order { get; set; }
    }
}