namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using System.Transactions;

    using RightpointLabs.Pourcast.Domain.Events;

    public abstract class TransactionDependantEventHandler<T> : IEventHandler<T> where T : IDomainEvent
    {
        private T _domainEvent;

        public void Handle(T domainEvent)
        {
            if (Transaction.Current != null)
            {
                _domainEvent = domainEvent;

                Transaction.Current.TransactionCompleted += HandleAfterTransaction;
            }
            else
            {
                HandleAfterTransaction(domainEvent);
            }
        }

        private void HandleAfterTransaction(object sender, TransactionEventArgs e)
        {
            HandleAfterTransaction(_domainEvent);
        }

        protected abstract void HandleAfterTransaction(T domainEvent);
    }
}
