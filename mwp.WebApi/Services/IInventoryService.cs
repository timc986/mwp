using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mwp.WebApi.Models;

namespace mwp.WebApi.Services
{
    public interface IInventoryService
    {
        InventoryItems AddInventoryItems(InventoryItems items);
        Dictionary<string, InventoryItems> GetInventoryItems();
    }
}
