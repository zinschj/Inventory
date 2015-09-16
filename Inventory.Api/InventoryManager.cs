using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api
{
    //Since the instructions did not specify if the API should be a web service or 
    //something that could support a thick client, I wrote a DLL.
    //The DLL could be used as the core for either.
    public class InventoryManager
    {
        //add a repository member so that we can easily switch to a database without affecting the API
        private readonly IInventoryRepository _repository;

        //repository is passed in to allow use of Inversion of Control
        public InventoryManager(IInventoryRepository repository)
        {
            _repository = repository;
        }
        

        /// <summary>
        /// Removed event notifies listeners an item was removed from inventory.
        /// </summary>
        public event InventoryEventHandler Removed;


        /// <summary>
        /// Expired event notifies listeners an item in inventory has expired.
        /// </summary>
        public event InventoryEventHandler Expired;


        protected virtual void OnRemoved(InventoryEventArgs e)
        {
            if (Removed != null)
            {
                Removed(this, e);
            }
        }

        protected virtual void OnExpired(InventoryEventArgs e)
        {
            if (Expired != null)
            {
                Expired(this, e);
            }
        }
        
        /// <summary>
        /// Adds item to inventory.
        /// </summary>
        /// <param name="item">IInventoryItem to be added.</param>
        /// <returns>IInventoryItem that was added. To support fluent code style.</returns>
        public IInventoryItem Add(IInventoryItem item)
        {
            return _repository.Add(item);
        }

        /// <summary>
        /// Removes item from inventory by label.  
        /// It is assumed that label is the unique key for the item.
        /// </summary>
        /// <param name="label"></param>
        /// <returns>IInventoryItem removed from inventory.</returns>
        public IInventoryItem Remove(string label)
        {
            var item = _repository.Remove(label);
            if (item != null)
            {
                OnRemoved(new InventoryEventArgs(item));
            }
            return item;
        }

        /// <summary>
        /// CheckExpiration will inspect each item in inventory for expiration.
        /// For each item that has expired an "Expired" event is triggered.
        /// Normally, calls to CheckExpiration would be scheduled using System.Timers.Timer.
        /// Re-curring timer was not implemented to save time.
        /// </summary>
        public void CheckExpiration()
        {
            var currentDate = DateTime.Now;
            var expiredItems = _repository.List().Where(i => i.Expires < currentDate).ToList();

            while (expiredItems.Any())
            {
                OnExpired(new InventoryEventArgs(expiredItems.First()));
                expiredItems.RemoveAt(0);
            }
        }

    }
}
