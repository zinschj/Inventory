using System;

namespace Inventory.Api
{
    public class InventoryEventArgs : EventArgs
    {
        public IInventoryItem InventoryItem { get; set; }

        public InventoryEventArgs(IInventoryItem item)
        {
            InventoryItem = item;
        }
    }
}