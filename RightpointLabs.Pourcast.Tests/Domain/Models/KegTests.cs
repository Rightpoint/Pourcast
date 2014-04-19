namespace RightpointLabs.Pourcast.Tests.Domain.Models
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ploeh.AutoFixture;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Models;

    [TestClass]
    public class KegTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var fixture = new Fixture();
        }

        [TestClass]
        public class Constructors : KegTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ZeroCapactiyThrowsException()
            {
                new Keg("asdf", "qwer", 0);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NegativeCapactiyThrowsException()
            {
                new Keg("asdf", "qwer", -1);
            }
        }

        [TestClass]
        public class PourBeerMethod
        {
            [TestMethod]
            public void UpdatesAmountOfBeerPouredAndRemaining()
            {
                var fixture = new Fixture();
                var tapId = "asdf";
                var sut = fixture.Create<Keg>();
                var volume = 10;

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, volume);

                Assert.AreEqual(sut.AmountOfBeerPoured, volume);
                Assert.AreEqual(sut.AmountOfBeerRemaining + volume, sut.Capacity);
            }

            [TestMethod]
            public void SetsIsEmptyWhenEmpty()
            {
                var fixture = new Fixture();
                var tapId = "asdf";
                var sut = fixture.Create<Keg>();
                var volume = sut.Capacity;

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, volume);

                Assert.IsTrue(sut.IsEmpty);
            }

            [TestMethod]
            public void DoesNotSetIsEmptyWhenNotEmpty()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var tapId = "asdf";
                var volume = sut.Capacity - 1;

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, volume);

                Assert.IsFalse(sut.IsEmpty);
            }

            [TestMethod]
            public void RaisesBeerPouredEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var tapId = "asdf";
                var beerPouredEventWasRaised = false;
                var kegEmptiedEventWasRaised = false;
                
                DomainEvents.Register<BeerPourStopped>(b => beerPouredEventWasRaised = true);
                DomainEvents.Register<KegEmptied>(k => kegEmptiedEventWasRaised = true);

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, sut.Capacity - 1);

                Assert.IsTrue(beerPouredEventWasRaised);
                Assert.IsFalse(kegEmptiedEventWasRaised);
            }

            [TestMethod]
            public void RaisesKegEmptiedEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var tapId = "asdf";
                var beerPouredEventWasRaised = false;
                var kegEmptiedEventWasRaised = false;

                DomainEvents.Register<BeerPourStopped>(b => beerPouredEventWasRaised = true);
                DomainEvents.Register<KegEmptied>(k => kegEmptiedEventWasRaised = true);

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, sut.Capacity);

                Assert.IsTrue(beerPouredEventWasRaised);
                Assert.IsTrue(kegEmptiedEventWasRaised);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void VolumeLessThanZeroThrowsException()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var tapId = "asdf";

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, -1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void VolumeEqualToZeroThrowsException()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var tapId = "asdf";

                sut.StartPourFromTap(tapId);
                sut.StopPourFromTap(tapId, 0);
            }
        }
    }
}
