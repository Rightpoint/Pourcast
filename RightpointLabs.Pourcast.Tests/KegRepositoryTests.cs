using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.DataModel.Entities;
using RightpointLabs.Pourcast.Repository.Concrete;

namespace RightpointLabs.Pourcast.Tests
{
    [TestClass]
    public class KegRepositoryTests
    {
        [TestMethod]
        public void CanGetKegsFromLocalDatabase()
        {
            // setup
            var repo = new KegRepository(new MongoConnectionHandler<Keg>("mongodb://localhost", "test"));

            // Act
            var kegs = repo.GetAll();

            // Assert
            Assert.AreNotEqual(0, kegs.Count);
        }
    }
}
