namespace RightpointLabs.Pourcast.Tests.Application.Orchestrators
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ploeh.AutoFixture;

    using RightpointLabs.Pourcast.Application.Orchestrators.Concrete;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    [TestClass]
    public class KegOrchestratorTests
    {
        protected Mock<IKegRepository> KegRepository;

        protected Mock<IDateTimeProvider> DateTimeProvider;

        protected Keg Keg;

        protected DateTime Now;

        [TestInitialize]
        public void Initialize()
        {
            var fixture = new Fixture();

            Keg = fixture.Create<Keg>();
            Now = DateTime.Now;

            KegRepository = new Mock<IKegRepository>();
            KegRepository.Setup(k => k.OnTap(It.IsAny<String>()))
                .Returns(Keg);
            KegRepository.Setup(k => k.Update(It.IsAny<Keg>()));

            DateTimeProvider = new Mock<IDateTimeProvider>();
            DateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(Now);
        }

        [TestClass]
        public class PourBeerFromTapMethod : KegOrchestratorTests
        {
            [TestMethod]
            public void AddsPourToKeg()
            {
                var orch = new KegOrchestrator(KegRepository.Object, DateTimeProvider.Object);
                
                orch.PourBeerFromTap("0", 10);

                Assert.IsTrue(Keg.Pours.Any());
                Assert.IsTrue(Keg.Pours.Count() == 1);
                Assert.AreEqual(Keg.Pours.Single().Volume, 10);
                Assert.AreEqual(Keg.Pours.Single().PouredDateTime, Now);
                KegRepository.Verify(x => x.Update(Keg));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NegativeVolumeThrowsException()
            {
                var orch = new KegOrchestrator(KegRepository.Object, DateTimeProvider.Object);

                orch.PourBeerFromTap("0", -1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ZeroVolumeThrowsException()
            {
                var orch = new KegOrchestrator(KegRepository.Object, DateTimeProvider.Object);

                orch.PourBeerFromTap("0", 0);
            }
        }
    }
}
