using RPG.Attributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour
    {
        [Header("Chest")]
        [SerializeField] Transform[] _chestPosition;

        [Header("Gloves")]
        [SerializeField] Transform[] _GlovesPosition;

        [Header("Pants")]
        [SerializeField] Transform[] _PantsPosition;

        [Header("Boots")]
        [SerializeField] Transform[] _BootsPosition;

        GameObject[] _chestGO;

        GameObject[] _GlovesGO;

        GameObject[] _PantsGORight;

        GameObject[] _BootsGORight;

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

            _chestGO = new GameObject[_chestPosition.Length];
            _GlovesGO = new GameObject[_GlovesPosition.Length];
            _PantsGORight = new GameObject[_PantsPosition.Length];
            _BootsGORight = new GameObject[_BootsPosition.Length];
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
                case TypeArmor.Gloves:
                    ChangeNewArmor(ref itemHat, itemInventory, value);
                    ChangeArmorGameObject(itemInventory, _GlovesPosition, _GlovesGO, value);
                    break;
                case TypeArmor.Chest:
                    ChangeNewArmor(ref itemFront, itemInventory, value);
                    ChangeArmorGameObject(itemInventory, _chestPosition, _chestGO, value);
                    break;
                case TypeArmor.Pants:
                    ChangeNewArmor(ref itemPants, itemInventory, value);
                    break;
                case TypeArmor.Boots:
                    ChangeNewArmor(ref itemBoots, itemInventory, value);
                    break;
            }
        }

        private void ChangeArmorGameObject(ItemInventory itemInventory, Transform[] GOposition, GameObject[] GOSaveArmor, bool value)
        {
            for (int i = 0; i < GOposition.Length; i++)
            {
                if (!value)
                    GOSaveArmor[0] = Instantiate(itemInventory.GetGameObjectsArmor()[i], GOposition[i].transform);
                else
                    Destroy(GOSaveArmor[0]);
            }
        }

        private void ChangeNewArmor(ref ItemInventory item, ItemInventory itemInventory, bool OnlyForNotDrop)
        {
            if (item != null) {

                if(!OnlyForNotDrop) SpawnLastItem(item);
                RemoveArmorFromPlayer(ref item);
            }
            if (OnlyForNotDrop) return; //only remove if item it is wear

            item = itemInventory;
            EquipNewArmorToPlayer(item);
        }

        private void EquipNewArmorToPlayer(ItemInventory item)
        {
            //add stats from the player
            print("Add armor " + item.GetActualArmorItem());
            healthPlayer.ReceivedNewArmor(item.GetActualArmorItem());
        }

        private void RemoveArmorFromPlayer(ref ItemInventory itemInventory) {
            //remove stats from the player
            print("Remove armor " + itemInventory.GetActualArmorItem());

            healthPlayer.EliminatedArmor(itemInventory.GetActualArmorItem());
            itemInventory = null;
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
