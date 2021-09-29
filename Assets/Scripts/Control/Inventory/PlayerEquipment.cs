using RPG.Attributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour
    {
        ItemInventory itemHat;
        ItemInventory itemFront;
        ItemInventory itemPants;
        ItemInventory itemBoots;

        ItemInventory itemWeapon;

        HealthPlayer healthPlayer;
        FighterPlayer fighterPlayer;
        PlayerUI playerUI;

        void Start()
        {
            healthPlayer = GetComponent<HealthPlayer>();
            fighterPlayer = GetComponent<FighterPlayer>();
            playerUI = GetComponent<PlayerUI>();

            UpdateTextStatsInventory();
        }

        public void AddNewEquipmentToPlayer(ItemInventory itemInventory) {
            print("Add equip");
            if (itemInventory.GetTypeItemInventory() == TypeItemInventory.Armor) FindItemAndChangeArmor(itemInventory, false);
            else AddNewWeapon(itemInventory);

            UpdateTextStatsInventory();
        }

        public void RemoveEquipmentToPlayer(ItemInventory itemInventory) {
            print("Remove equip");
            if (itemInventory.GetTypeItemInventory() == TypeItemInventory.Armor) FindItemAndChangeArmor(itemInventory, true);
            else RemoveLastWeapon(itemInventory);

            UpdateTextStatsInventory();
        }

        #region Item equipment

        private void FindItemAndChangeArmor(ItemInventory itemInventory, bool value) {
            switch (itemInventory.GetTypeItemArmor()) {
                case TypeArmor.Hat:
                    ChangeNewArmor(ref itemHat, itemInventory, value);
                    break;
                case TypeArmor.Front:
                    ChangeNewArmor(ref itemFront, itemInventory, value);
                    break;
                case TypeArmor.Pants:
                    ChangeNewArmor(ref itemPants, itemInventory, value);
                    break;
                case TypeArmor.Boots:
                    ChangeNewArmor(ref itemBoots, itemInventory, value);
                    break;
            }
        }

        private void ChangeNewArmor(ref ItemInventory item, ItemInventory itemInventory, bool OnlyForNotDrop)
        {
            if (item != null) {
                if(!OnlyForNotDrop) print("Spawn new armor in the zone, or add to the inventory " + OnlyForNotDrop);
                RemoveArmorFromPlayer(item);
            }
            if (OnlyForNotDrop) return; //only remove if item it is wear

            item = itemInventory;
            EquipNewArmorToPlayer(item);
        }

        private void EquipNewArmorToPlayer(ItemInventory itemInventory)
        {
            //add stats from the player
            print("Add armor " + itemInventory.GetActualArmorItem());
            healthPlayer.ReceivedNewArmor(itemInventory.GetActualArmorItem());

        }

        private void RemoveArmorFromPlayer(ItemInventory itemInventory) {
            //remove stats from the player
            print("Remove armor " + itemInventory.GetActualArmorItem());
            SpawnLastItem(itemInventory);
            healthPlayer.EliminatedArmor(itemInventory.GetActualArmorItem());
        }

        #endregion

        #region Item Weapon

        private void AddNewWeapon(ItemInventory itemInventory) {
            if (itemWeapon != null)
            {
                SpawnLastItem(itemWeapon);
                RemoveLastWeapon(itemWeapon);
            }

            itemWeapon = itemInventory;
            fighterPlayer.EquipWeapon(itemInventory.GetWeaponConfig() ,0);
        }

        private void RemoveLastWeapon(ItemInventory itemInventory)
        {
            fighterPlayer.ChangeToUnarmedWeapon();
            itemWeapon = null;
        }

        #endregion

        private void SpawnLastItem(ItemInventory itemInventory) {
            Instantiate(itemInventory.GetGameObjectToSpawn(), transform.position, Quaternion.identity);
        }

        private void UpdateTextStatsInventory()
        {
            playerUI.UpdateTextInventoryStatsPlayer(
                fighterPlayer.GetDamage(),
                healthPlayer.GetInitialHealth(),
                healthPlayer.GetArmor());
        }
    }
}
