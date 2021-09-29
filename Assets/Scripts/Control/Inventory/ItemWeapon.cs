using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/ Make new Item Weapon", order = 0)]
    public class ItemWeapon : ItemInventory
    {
        [Header("Weapon")]
        [SerializeField] WeaponConfig _weaponConfig;

        public override string GetInfoItemString()
        {
            string damage = _weaponConfig.AttackArea ? _weaponConfig.weaponDamage.ToString() : _weaponConfig.damageArea.ToString();
            string textInfo = "Name: " + _nameItem + "\n" +
                              "Damage: " + damage + "\n" +
                              "Description: " + description;
            return textInfo;
        }

        public override WeaponConfig GetWeaponConfig() => _weaponConfig;
    }
}
