using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class UIControlTypeInventoryItem : MonoBehaviour
    {
        [SerializeField] ItemInventory itemInventory;

        DrapSystem drapSystem;

        private void Start()
        {
            drapSystem = GetComponent<DrapSystem>();
            drapSystem.EventStartDrag += SetActionToSlotInfoInventory;

            drapSystem.EventPointerEnterThisCard += ShowInformationAboutItem;
            drapSystem.EventPointerExitThisCard += DesactiveItemHover;
        }

        private void ShowInformationAboutItem() {
            HoverUIItem.hoverUI.ShowInfoWeapon(itemInventory.GetInfoItemString(),
                GetComponentInParent<CardSlotDrapDetection>().GetComponent<RectTransform>());
        }

        private void DesactiveItemHover()
        {
            HoverUIItem.hoverUI.DesactiveObject();
        }

        private void SetActionToSlotInfoInventory() {
            try
            {
                GetComponentInParent<SlotInfoInventory>().SetEventObserverToChanges(ref drapSystem.EventEndDrag);
            }
            catch (Exception e) { 
                
            }
        }

        public void SetItemInventory(ItemInventory newItem) => itemInventory = newItem;

        public ItemInventory GetItemInventory() => itemInventory;

        public TypeItemInventory GetTypeItemInventory() => itemInventory.GetTypeItemInventory();

        public TypeArmor GetTypeItemArmor() => itemInventory.GetTypeItemArmor();
    }
}
