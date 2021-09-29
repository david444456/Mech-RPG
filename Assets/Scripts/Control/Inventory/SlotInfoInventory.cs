using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class SlotInfoInventory : MonoBehaviour
    {
        [SerializeField] TypeItemInventory typeItemInventory;
        [SerializeField] bool worksWithSlotForPlayer = false;

        CardSlotDrapDetection drapDetection;
        PlayerEquipment playerEquipment;

        ItemInventory lastItemInventory;

        void Start()
        {

                playerEquipment = FindObjectOfType<PlayerEquipment>();
                drapDetection = GetComponent<CardSlotDrapDetection>();

            if (worksWithSlotForPlayer)
            {
                drapDetection.EventShareNewItemInventory += SetStatsToPlayer;
            }
        }

        public void SetEventObserverToChanges(ref Action eventEnd)
        {
            eventEnd += VerificatedIfRemoveTheObject;
        }

        private void SetStatsToPlayer(ItemInventory itemInventory)
        {
            if (CheckIfTheSameItemInventory(itemInventory)) return;
            lastItemInventory = itemInventory;
            playerEquipment.AddNewEquipmentToPlayer(itemInventory);
        }

        private bool CheckIfTheSameItemInventory(ItemInventory itemInventory)
        {
            return lastItemInventory != null &&
                            lastItemInventory.GetActualIndexItem() == itemInventory.GetActualIndexItem();
        }

        private void RemoveStatsToPlayer() {
            if (lastItemInventory != null)
            {
                playerEquipment.RemoveEquipmentToPlayer(lastItemInventory);
                lastItemInventory = null;
            }
        }

        private void VerificatedIfRemoveTheObject() {
            try
            {
                GameObject childrenCard = GetComponentInChildren<DrapSystem>().gameObject;
                if (childrenCard == null)
                {
                    print("Nothing here " + childrenCard.name);
                }
            }
            catch (Exception e) {
                RemoveStatsToPlayer();
                drapDetection.ChangeToNullTheLastItemInSlot();
            }
        }

    }
}