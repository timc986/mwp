using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mwp.WebApi.Models;

namespace mwp.WebApi.Services
{
    public class InventoryService: IInventoryService
    {

        private readonly Dictionary<string, InventoryItems> inventoryItems;

        public InventoryService()
        {
            this.inventoryItems = new Dictionary<string, InventoryItems>();
        }

        public InventoryItems AddInventoryItems(InventoryItems items)
        {
            inventoryItems.Add(items.ItemName, items);

            return items;
        }

        public Dictionary<string, InventoryItems> GetInventoryItems()
        {
            return inventoryItems;
        }
    }
}
