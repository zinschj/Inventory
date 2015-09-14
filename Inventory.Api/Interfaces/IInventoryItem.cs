using System;

namespace Inventory.Api
{
    public interface IInventoryItem
    {
        string Label { get; set; }
        DateTime Expires { get; set; }
    }
}