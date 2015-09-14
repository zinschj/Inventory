using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Api;

namespace Inventory.Tests
{
    [TestClass]
    public class InventoryTest
    {
        private string label;
        private InventoryItem item;
        private InventoryManager manager;

        [TestInitialize()]
        public void Startup()
        {
            label = "An Item";
            item = new InventoryItem() { Label = label, Expires = DateTime.Now.Date.AddDays(1) };
            manager = new InventoryManager(new InventoryRepository());
        }

        [TestCleanup()]
        public void Cleanup()
        {
            label = null;
            item = null;
            manager = null;
        }


        [TestMethod]
        public void ShouldAddItemToInventory()
        {
            // arrange
            // act
            var actual = manager.Add(item);

            // assert
            Assert.AreEqual(item.Label, actual.Label, "Should add item to inventory");
        }

        [TestMethod]
        [ExpectedException(typeof(InventoryException))]
        public void ShouldThrowIfAddingItemThatExists()
        {
            // arrange
            // act 
            var sameItem = manager.Add(item);
            var actual = manager.Add(sameItem);

            //(implied assert)
        }

        [TestMethod]
        public void ShouldRemoveItemFromInventory()
        {
            // arrange
            manager.Add(item);

            // act
            var actual = manager.Remove(label);

            // assert
            Assert.AreEqual(item.Label, actual.Label, "Should remove item from inventory");
        }

        [TestMethod]
        public void ShouldReturnNullItemForRemoveItemThatDoesNotExist()
        {
            // arrange
            manager.Add(item);

            // act
            var actual = manager.Remove("Does Not Exist");

            // assert
            Assert.IsNull(actual, "Should return null item when attempting to remove non-existing item.");
        }

        [TestMethod]
        public void ShouldRaiseItemRemovedEvent()
        {
            // arrange
            bool eventRaised = false;
            manager.Removed += delegate { eventRaised = true; };
            manager.Add(item);

            // act
            var actual = manager.Remove(label);

            // assert
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void ShouldRaiseItemExpiredEvent()
        {
            // arrange
            bool eventRaised = false;
            manager.Expired += delegate { eventRaised = true; };
            item.Expires = DateTime.Now.Date.AddDays(-1);
            manager.Add(item);

            // act
            manager.CheckExpiration();

            // assert
            Assert.IsTrue(eventRaised);
        }

    }
}
