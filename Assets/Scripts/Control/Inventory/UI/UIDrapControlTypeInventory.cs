using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class UIDrapControlTypeInventory : MonoBehaviour
    {
        [SerializeField] TypeItemInventory itemInventory;
        [SerializeField] TypeArmor typeArmor;

        public bool ItIsTheSameType(UIControlTypeInventoryItem uIControlType) {
            bool value = false;

            if (uIControlType.GetTypeItemInventory() == TypeItemInventory.Weapon
                && itemInventory == TypeItemInventory.Weapon) {
                value = true;
            }else if (uIControlType.GetTypeItemInventory() == TypeItemInventory.Armor
                && itemInventory == TypeItemInventory.Armor)
            {
                if (uIControlType.GetTypeItemArmor() == typeArmor)
                    value = true;
            }

            return value;
        }

    }
}
