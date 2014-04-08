using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RightpointLabs.Pourcast.Tests
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Infrastructure.Data;
    using RightpointLabs.Pourcast.Infrastructure.Data.Repositories;

    [TestClass]
    public class KegRepositoryTests
    {
        [TestMethod]
        public void CanGetKegsFromLocalDatabase()
        {
            // setup
            var repo = new KegRepository(new MongoConnectionHandler<Keg>("mongodb://localhost", "test"));

            // Act
            var kegs = repo.GetAll().ToList();

            // Assert
            Assert.AreNotEqual(0, kegs);
        }
    }
}
