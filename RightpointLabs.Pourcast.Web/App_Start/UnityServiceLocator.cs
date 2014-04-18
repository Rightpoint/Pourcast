using System;
using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Web.App_Start
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    public class UnityServiceLocator : ServiceLocatorImplBase
    {
        private IUnityContainer container;

        public UnityServiceLocator(IUnityContainer container)
        {
            this.container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return container.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }
    }
}