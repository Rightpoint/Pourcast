namespace RightpointLabs.Pourcast.Tests.Domain.Models
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RightpointLabs.Pourcast.Domain.Models;

    [TestClass]
    public class PourTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestClass]
        public class Constructor : PourTests
        {
            [TestMethod]
            public void PositiveVolumeWorks()
            {
                var pour = new Pour("0", 1, DateTime.Now);

                Assert.IsNotNull(pour);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ZeroVolumeThrowsException()
            {
                var pour = new Pour("0", 0, DateTime.Now);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NegativeVolumeThrowsException()
            {
                var pour = new Pour("0", -1, DateTime.Now);
            }
        }
    }
}
