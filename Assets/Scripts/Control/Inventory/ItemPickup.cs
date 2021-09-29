using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] ItemInventory _itemInventory;

        public ItemInventory GetItemInventory() => _itemInventory;

        public void PickupItemForPlayer()
        {
            //add to player
            Destroy(gameObject);
        }

    }
}
