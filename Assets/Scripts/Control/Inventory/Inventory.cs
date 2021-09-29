using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class Inventory
    {
        List<ItemInventory> itemInventories = new List<ItemInventory>();
        private int _countItems = 0;

        public Inventory(int countInv) {
            for (int i = 0; i < countInv; i++) {
                itemInventories.Add(null);
            }

            _countItems = 0;
        }

        public bool CanAddOtherItem() => _countItems < itemInventories.Count;

        public int GetEmptySlotIndex()
        {
            int index = -1;

            for (int i = itemInventories.Count-1; i >= 0 ; i--)
            {
                if (itemInventories[i] == null) index = i;
            }

            return index;
        }

        public void AddNewItem(ItemInventory itemInventory, int index) {
            itemInventories[index] = itemInventory;
        }
    }
}
