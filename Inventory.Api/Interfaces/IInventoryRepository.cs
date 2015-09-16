using System.Collections.Generic;

namespace Inventory.Api
{
    public interface IInventoryRepository
    {
        IInventoryItem Add(IInventoryItem item);
        IInventoryItem Remove(string label);
        IEnumerable<IInventoryItem> List();
    }
}