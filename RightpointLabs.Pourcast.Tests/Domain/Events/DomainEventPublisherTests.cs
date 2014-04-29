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

            DomainEvents.Register<PourStopped>(b => volumeResult = b.Volume);
            DomainEvents.Raise(new PourStopped("asdf", "qwer", volume, 10));

            Assert.AreEqual(volume, volumeResult);
        }

        [TestMethod]
        public void FiresOffMultipleHandler()
        {
            double volume = 1;
            double volumeResult = 0;
            string tapId = "asdf";
            string tapIdResult = "";

            DomainEvents.Register<PourStopped>(b => volumeResult = b.Volume);
            DomainEvents.Register<PourStopped>(b => tapIdResult = b.TapId);
            DomainEvents.Raise(new PourStopped(tapId, "qwer", volume, 10));

            Assert.AreEqual(volume, volumeResult);
            Assert.AreEqual(tapId, tapIdResult);
        }

        [TestMethod]
        public void SpecificEventsReachSpecificHandlers()
        {
            bool beerWasPoured = false;
            bool kegWasEmptied = false;

            DomainEvents.Register<PourStopped>(b => beerWasPoured = true);
            DomainEvents.Register<KegEmptied>(k => kegWasEmptied = true);
            DomainEvents.Raise(new PourStopped("asdf", "qwer", 1, 0));
            DomainEvents.Raise(new KegEmptied("qwer"));

            Assert.IsTrue(beerWasPoured);
            Assert.IsTrue(kegWasEmptied);
        }
    }
}
