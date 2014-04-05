namespace RightpointLabs.Pourcast.Tests.Domain.Models
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ploeh.AutoFixture;

    using RightpointLabs.Pourcast.Domain.Models;

    [TestClass]
    public class KegTests
    {
        protected Beer someBeer;

        [TestInitialize]
        public void Initialize()
        {
            var fixture = new Fixture();

            someBeer = fixture.Create<Beer>();
        }

        [TestClass]
        public class Constructors : KegTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void NullBeerThrowsException()
            {
                var keg = new Keg(null, 1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ZeroCapactiyThrowsException()
            {
                var keg = new Keg(someBeer, 0);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NegativeCapactiyThrowsException()
            {
                var keg = new Keg(someBeer, -1);
            }
        }
    }
}
