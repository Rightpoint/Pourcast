namespace RightpointLabs.Pourcast.Tests.Domain.Models
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ploeh.AutoFixture;

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
    }
}
