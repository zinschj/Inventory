using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Api;

namespace Inventory.Demo
{
    class Demo
    {
        private static readonly Random random = new Random();
        private static readonly InventoryManager manager = new InventoryManager(new InventoryRepository());

        static void Main(string[] args)
        {
            IInventoryItem item; 
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
            Console.WriteLine("Item {0} expired.  Requesting removal....", inventoryEventArgs.InventoryItem.Label);
            manager.Remove(inventoryEventArgs.InventoryItem.Label);
        }

        private static void ManagerOnRemoved(object sender, InventoryEventArgs inventoryEventArgs)
        {
            Console.WriteLine("Removed item {0}.", inventoryEventArgs.InventoryItem.Label);
        }
    }
}
