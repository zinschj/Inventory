using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Api;

namespace Inventory.Demo
{
    //This is a basic console application to demonstrate usage of the inventory API.
    class Demo
    {
        private static readonly Random random = new Random();
        private static readonly InventoryManager manager = new InventoryManager(new InventoryRepository());

        static void Main(string[] args)
        {
            
            IInventoryItem item; 
            
            //add listeners to Removed and Expired events.
            manager.Removed += ManagerOnRemoved;
            manager.Expired += ManagerOnExpired;

            //Add some items to the inventory
            for (var i = 0; i < 10; i++)
            {
                item = manager.Add(new InventoryItem()
                                                 {
                                                     Label = Guid.NewGuid().ToString(),
                                                     Expires = DateTime.Now.AddSeconds(random.Next(1, 10))
                                                 });
                Console.WriteLine("Added item {0}", item.Label);
                manager.CheckExpiration();
                System.Threading.Thread.Sleep(1000);
            }

            //Additional Ten Seconds for Completion of Expirations.
            for (var i = 0; i < 10; i++)
            {
                manager.CheckExpiration();
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Press any key to exit....");
            Console.ReadKey();

        }

        private static void ManagerOnExpired(object sender, InventoryEventArgs inventoryEventArgs)
        {
            //When an item expires, display the label of the expired item and remove it from inventory.
            Console.WriteLine("Item {0} expired.  Requesting removal....", inventoryEventArgs.InventoryItem.Label);
            manager.Remove(inventoryEventArgs.InventoryItem.Label);
        }

        private static void ManagerOnRemoved(object sender, InventoryEventArgs inventoryEventArgs)
        {
            //Once the item is removed, our event listener will display that it was removed.
            Console.WriteLine("Removed item {0}.", inventoryEventArgs.InventoryItem.Label);
        }
    }
}
