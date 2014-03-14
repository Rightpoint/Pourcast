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
            var mch = new MongoConnectionHandler("mongodb://localhost", "test");

            // Act
            mch.SetCollection<Keg>();
            var kegs = mch.Collection.FindAllAs<Keg>();

            // Assert
            Assert.AreNotEqual(0, kegs.Count());
        }
    }
}
