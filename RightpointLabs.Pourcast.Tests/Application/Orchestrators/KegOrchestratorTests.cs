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

        }

        [TestClass]
        public class PourBeerFromTapMethod : KegOrchestratorTests
        {
            
        }
    }
}
