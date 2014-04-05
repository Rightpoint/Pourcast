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

        protected Keg SomeKeg;

        protected DateTime SomeTime;

        [TestInitialize]
        public void Initialize()
        {
            var fixture = new Fixture();

            SomeKeg = fixture.Create<Keg>();
            SomeTime = DateTime.Now;

            KegRepository = new Mock<IKegRepository>();
            KegRepository.Setup(k => k.OnTap(It.IsAny<String>()))
                .Returns(SomeKeg);
            KegRepository.Setup(k => k.Update(It.IsAny<Keg>()));

            DateTimeProvider = new Mock<IDateTimeProvider>();
            DateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(SomeTime);
        }

        [TestClass]
        public class PourBeerFromTapMethod : KegOrchestratorTests
        {
            [TestMethod]
            public void CreatesPourAndAddsToKeg()
            {
                var orch = new KegOrchestrator(KegRepository.Object, DateTimeProvider.Object);
                
                orch.PourBeerFromTap("0", 10);

                Assert.IsTrue(SomeKeg.Pours.Any());
                Assert.IsTrue(SomeKeg.Pours.Count() == 1);
                Assert.AreEqual(SomeKeg.Pours.Single().Volume, 10);
                Assert.AreEqual(SomeKeg.Pours.Single().PouredDateTime, SomeTime);
                KegRepository.Verify(x => x.Update(SomeKeg));
            }
        }
    }
}
