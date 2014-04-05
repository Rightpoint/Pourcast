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
                var pour = new Pour(DateTime.Now, 1);

                Assert.IsNotNull(pour);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ZeroVolumeThrowsException()
            {
                var pour = new Pour(DateTime.Now, 0);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NegativeVolumeThrowsException()
            {
                var pour = new Pour(DateTime.Now, -1);
            }
        }
    }
}
