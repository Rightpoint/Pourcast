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
        public class TapKegMethod : TapTests
        {
            [TestMethod]
            public void AddsKegToTap()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();
                var kegId = "asdf";

                sut.TapKeg(kegId);
                
                Assert.IsNotNull(sut.KegId);
                Assert.IsTrue(sut.HasKeg);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void ThrowsExceptionIfTapHasKeg()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();
                var kegId = "asdf";

                sut.TapKeg(kegId);
                sut.TapKeg("qwer");
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsExceptionIfKegIdNull()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();

                sut.TapKeg("");
            }

            [TestMethod]
            public void RaisesKegTappedEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();
                var eventRaised = false;

                DomainEvents.Register<KegTapped>(kt => eventRaised = true);
                sut.TapKeg("asdf");

                Assert.IsTrue(eventRaised);
            }
        }

        [TestClass]
        public class RemoveKegMethod
        {
            [TestMethod]
            public void RemovesKeg()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();
                sut.TapKeg("asdf");

                sut.RemoveKeg();

                Assert.IsFalse(sut.HasKeg);
                Assert.IsTrue(string.IsNullOrWhiteSpace(sut.KegId));
            }

            [TestMethod]
            public void RaisesKegRemovedFromTapEvent()
            {
                var fixture = new Fixture();
                var sut = fixture.Create<Tap>();
                var eventRaised = false;
                sut.TapKeg("asdf");

                DomainEvents.Register<KegRemovedFromTap>(kt => eventRaised = true);
                sut.RemoveKeg();

                Assert.IsTrue(eventRaised);
            }
        }
    }
}
