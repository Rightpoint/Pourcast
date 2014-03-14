using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Tests
{
    [TestClass]
    public class MongoConnectionHandlerTest
    {
        [TestMethod]
        public void AbleToRetrieveItemsFromTestDatabase()
        {
            // Setup
            var mch = new MongoConnectionHandler<Keg>("mongodb://localhost", "test");

            // Act
            var kegs = mch.MongoCollection.FindAllAs<Keg>();
            

            // Assert
            Assert.AreNotEqual(0, kegs.Count());
        }
    }
}
