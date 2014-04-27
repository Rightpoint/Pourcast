using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RightpointLabs.Pourcast.Tests
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Infrastructure.Persistence;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories;

    [TestClass]
    public class KegRepositoryTests
    {
        [TestMethod]
        public void CanGetKegsFromLocalDatabase()
        {
            // setup
            var repo = new KegRepository(new KegCollectionDefinition(new MongoConnectionHandler("mongodb://localhost", "test")));

            // Act
            var kegs = repo.GetAll().ToList();

            // Assert
            Assert.AreNotEqual(0, kegs);
        }
    }
}
