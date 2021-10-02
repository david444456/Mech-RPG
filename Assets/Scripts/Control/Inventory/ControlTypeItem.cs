using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory {
    public class ControlTypeItem : MonoBehaviour
    {
        [SerializeField] ItemInventory[] allItems;

        public ItemInventory GetTypeItemByIndex(int index) {
            if (index == 0) return null;
            for (int i = 0; i < allItems.Length; i++) {
                if (index == allItems[i].GetActualIndexItem()) return allItems[i];
            }
            Debug.LogError("Type index not found in the list of all items");
            return null;
        }
    }
}
