using System;

namespace Inventory.Api
{
    public class InventoryItem : IInventoryItem
    {
        public string Label { get; set; }
        public DateTime Expires { get; set; }
    }
}