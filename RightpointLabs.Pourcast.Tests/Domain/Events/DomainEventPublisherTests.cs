namespace RightpointLabs.Pourcast.Tests.Domain.Events
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RightpointLabs.Pourcast.Domain.Events;

    [TestClass]
    public class DomainEventPublisherTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void FiresOffSingleHandler()
        {
            double volume = 1;
            double volumeResult = 0;

            DomainEvents.Register<BeerPourStopped>(b => volumeResult = b.Volume);
            DomainEvents.Raise(new BeerPourStopped("asdf", "qwer", volume, 10));

            Assert.AreEqual(volume, volumeResult);
        }

        [TestMethod]
        public void FiresOffMultipleHandler()
        {
            double volume = 1;
            double volumeResult = 0;
            string tapId = "asdf";
            string tapIdResult = "";

            DomainEvents.Register<BeerPourStopped>(b => volumeResult = b.Volume);
            DomainEvents.Register<BeerPourStopped>(b => tapIdResult = b.TapId);
            DomainEvents.Raise(new BeerPourStopped(tapId, "qwer", volume, 10));

            Assert.AreEqual(volume, volumeResult);
            Assert.AreEqual(tapId, tapIdResult);
        }

        [TestMethod]
        public void SpecificEventsReachSpecificHandlers()
        {
            bool beerWasPoured = false;
            bool kegWasEmptied = false;

            DomainEvents.Register<BeerPourStopped>(b => beerWasPoured = true);
            DomainEvents.Register<KegEmptied>(k => kegWasEmptied = true);
            DomainEvents.Raise(new BeerPourStopped("asdf", "qwer", 1, 0));
            DomainEvents.Raise(new KegEmptied("qwer"));

            Assert.IsTrue(beerWasPoured);
            Assert.IsTrue(kegWasEmptied);
        }
    }
}
