using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api
{
    public class InventoryManager
    {
        private IInventoryRepository _repository;

        public InventoryManager(IInventoryRepository repository)
        {
            _repository = repository;
        }
        
        public event InventoryEventHandler Removed;
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
        
        public IInventoryItem Add(IInventoryItem item)
        {
            return _repository.Add(item);
        }

        public IInventoryItem Remove(string label)
        {
            var item = _repository.Remove(label);
            if (item != null)
            {
                OnRemoved(new InventoryEventArgs(item));
            }
            return item;
        }

        public void CheckExpiration()
        {
            var currentDate = DateTime.Now;
            var expiredItems = _repository.GetAll().Where(i => i.Expires < currentDate).ToList();

            while (expiredItems.Any())
            {
                OnExpired(new InventoryEventArgs(expiredItems.First()));
                expiredItems.RemoveAt(0);
            }
        }

    }
}
