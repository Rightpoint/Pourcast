using System;

namespace RightpointLabs.Pourcast.Application.Transactions
{
    using System.Transactions;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class TransactionalAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new TransactionCallHandler();
        }
    }

    public class TransactionCallHandler : ICallHandler
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
