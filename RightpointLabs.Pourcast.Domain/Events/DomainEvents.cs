namespace RightpointLabs.Pourcast.Domain.Events
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    public static class DomainEvents
    {
        [ThreadStatic]
        private static List<Delegate> _actions;
        private static List<Delegate> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = new List<Delegate>();
                }
                return _actions;
            }
        }

        public static IDisposable Register<T>(Action<T> callback)
        {
            Actions.Add(callback);
            return new DomainEventRegistrationRemover(() => Actions.Remove(callback));
        }

        public static void Raise<T>(T eventArgs) where T : IDomainEvent
        {
            try
            {
                IEnumerable<IEventHandler<T>> registeredHandlers =
                    ServiceLocator.Current.GetAllInstances<IEventHandler<T>>();
                foreach (IEventHandler<T> handler in registeredHandlers)
                {
                    handler.Handle(eventArgs);
                }
            }
            catch (InvalidOperationException)
            {
                //When service locator is not set, ignore it.
            }
            foreach (var action in Actions)
            {
                Action<T> typedAction = action as Action<T>;
                if (typedAction != null)
                {
                    typedAction(eventArgs);
                }
            }
        }

        private sealed class DomainEventRegistrationRemover : IDisposable
        {
            private readonly Action _callOnDispose;

            public DomainEventRegistrationRemover(Action toCall)
            {
                _callOnDispose = toCall;
            }

            public void Dispose()
            {
                _callOnDispose();
            }
        }
    }
}
