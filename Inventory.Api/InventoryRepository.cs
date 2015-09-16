using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly Dictionary<string, IInventoryItem> _data;

        public InventoryRepository()
        {
            _data = new Dictionary<string, IInventoryItem>();
        }

        public IInventoryItem Add(IInventoryItem item)
        {
            if (_data.ContainsKey(item.Label))
            {
                throw new InventoryException();
            }

            _data.Add(item.Label, item);
            return _data[item.Label];
        }

        public IInventoryItem Remove(string label)
        {
            try
            {
                var item = _data[label];
                _data.Remove(label);
                return item;
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new InventoryException();
            }
        }


        public IEnumerable<IInventoryItem> List()
        {
            return _data.Values;
        }
    }
}
