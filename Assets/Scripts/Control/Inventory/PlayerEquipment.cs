using RPG.Attributes;
using RPG.Combat;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour, ISaveable
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

        GameObject[] _PantsGO;

        GameObject[] _BootsGO;

        ItemInventory itemHat;
        ItemInventory itemFront;
        ItemInventory itemPants;
        ItemInventory itemBoots;

        ItemInventory itemWeapon;

        HealthPlayer healthPlayer;
        FighterPlayer fighterPlayer;
        PlayerUI playerUI;

        void Awake (){
            healthPlayer = GetComponent<HealthPlayer>();
            fighterPlayer = GetComponent<FighterPlayer>();
            playerUI = GetComponent<PlayerUI>();


            _chestGO = new GameObject[_chestPosition.Length];
            _GlovesGO = new GameObject[_GlovesPosition.Length];
            _PantsGO = new GameObject[_PantsPosition.Length];
            _BootsGO = new GameObject[_BootsPosition.Length];

        }

        void Start()
        {


            UpdateTextStatsInventory();

        }

        public void AddNewEquipmentToPlayer(ItemInventory itemInventory) {
            print("Add equip");
            if (itemInventory == null) return;
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
                    ChangeArmorGameObject(itemInventory, _PantsPosition, _PantsGO, value);
                    break;
                case TypeArmor.Boots:
                    ChangeNewArmor(ref itemBoots, itemInventory, value);
                    ChangeArmorGameObject(itemInventory, _BootsPosition, _BootsGO, value);
                    break;
            }
        }

        private void ChangeArmorGameObject(ItemInventory itemInventory, Transform[] GOposition, GameObject[] GOSaveArmor, bool value)
        {
            for (int i = 0; i < GOposition.Length; i++)
            {
                if (!value)
                    GOSaveArmor[i] = Instantiate(itemInventory.GetGameObjectsArmor()[i], GOposition[i].transform);
                else
                    Destroy(GOSaveArmor[i]);
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

        #region Save function
        public object CaptureState()
        {

            int[] g = { 0,0,0,0,0 };

            if(itemHat!= null) g[0] = itemHat.GetActualIndexItem();
            if (itemFront != null) g[1] = itemFront.GetActualIndexItem();
            if (itemPants != null) g[2] = itemPants.GetActualIndexItem();
            if (itemBoots != null) g[3] = itemBoots.GetActualIndexItem();
            if (itemWeapon != null) g[4] = itemWeapon.GetActualIndexItem();


            return g;
        }

        public void RestoreState(object state)
        {
            ControlTypeItem controlTypeItem = FindObjectOfType<ControlTypeItem>();
            int[] index = (int[])state;

            AddNewEquipmentToPlayer(controlTypeItem.GetTypeItemByIndex(index[0]));
            AddNewEquipmentToPlayer(controlTypeItem.GetTypeItemByIndex(index[1]));
            AddNewEquipmentToPlayer(controlTypeItem.GetTypeItemByIndex(index[2]));
            AddNewEquipmentToPlayer(controlTypeItem.GetTypeItemByIndex(index[3]));
            AddNewEquipmentToPlayer(controlTypeItem.GetTypeItemByIndex(index[4]));


            print(index.Length);
        }
        #endregion
    }
}
