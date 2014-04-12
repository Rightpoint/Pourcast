namespace RightpointLabs.Pourcast.Tests.Domain.Models
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ploeh.AutoFixture;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Models;

    [TestClass]
    public class TapTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestClass]
        public class PourBeerMethod
        {
            [TestMethod]
            public void FiresEvent()
            {
                var fixture = new Fixture();
                var tap = fixture.Create<Tap>();
                double volume = 1;

                double volumeResult = 0;
                DomainEvents.Register<BeerPoured>(b => volumeResult = b.Volume);

                tap.PoorBeer(volume, DateTime.Now);

                Assert.AreEqual(volume, volumeResult);
            } 
        }
    }
}
