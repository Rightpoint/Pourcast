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
                var sut = fixture.Create<Keg>();
                var volume = 10;

                sut.EndPourFromTap("asdf", volume);

                Assert.AreEqual(sut.AmountOfBeerPoured, volume);
                Assert.AreEqual(sut.AmountOfBeerRemaining + volume, sut.Capacity);
            }

            [TestMethod]
            public void SetsIsEmptyWhenEmpty()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var volume = sut.Capacity;

                sut.EndPourFromTap("asdf", volume);

                Assert.IsTrue(sut.IsEmpty);
            }

            [TestMethod]
            public void DoesNotSetIsEmptyWhenNotEmpty()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var volume = sut.Capacity - 1;

                sut.EndPourFromTap("asdf", volume);

                Assert.IsFalse(sut.IsEmpty);
            }

            [TestMethod]
            public void RaisesBeerPouredEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var beerPouredEventWasRaised = false;
                var kegEmptiedEventWasRaised = false;
                
                DomainEvents.Register<BeerPourEnded>(b => beerPouredEventWasRaised = true);
                DomainEvents.Register<KegEmptied>(k => kegEmptiedEventWasRaised = true);

                sut.EndPourFromTap("asdf", sut.Capacity - 1);

                Assert.IsTrue(beerPouredEventWasRaised);
                Assert.IsFalse(kegEmptiedEventWasRaised);
            }

            [TestMethod]
            public void RaisesKegEmptiedEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();
                var beerPouredEventWasRaised = false;
                var kegEmptiedEventWasRaised = false;

                DomainEvents.Register<BeerPourEnded>(b => beerPouredEventWasRaised = true);
                DomainEvents.Register<KegEmptied>(k => kegEmptiedEventWasRaised = true);

                sut.EndPourFromTap("asdf", sut.Capacity);

                Assert.IsTrue(beerPouredEventWasRaised);
                Assert.IsTrue(kegEmptiedEventWasRaised);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void VolumeLessThanZeroThrowsException()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();

                sut.EndPourFromTap("asdf", -1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void VolumeEqualToZeroThrowsException()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Keg>();

                sut.EndPourFromTap("asdf", 0);
            }
        }
    }
}
