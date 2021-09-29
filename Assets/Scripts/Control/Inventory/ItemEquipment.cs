using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/ Make new Item equipment", order = 0)]
    public class ItemEquipment : ItemInventory
    {
        [Header("Equipment")]
        [SerializeField] TypeArmor _typeArmor;
        [SerializeField] int _valueArmor = 0;

        public override string GetInfoItemString()
        {
            string textInfo = "Name: " + _nameItem + "\n" +
                              "Armor: " + _valueArmor + "\n" +
                              "Description: " + description;
            return textInfo;
        }

        public override TypeArmor GetTypeItemArmor() => _typeArmor;

        public override int GetActualArmorItem() => _valueArmor;

    }
    public enum TypeArmor
    {
        Hat,
        Front,
        Pants,
        Boots
    }
}
