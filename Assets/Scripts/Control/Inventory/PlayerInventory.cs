using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] GameObject uiButton = null;
        [SerializeField] int distance = 10;

        [Header("var inventory")]
        [SerializeField] int largeInventory = 8;

        [Header("UI")]
        [SerializeField] GameObject GOInventory;
        [SerializeField] Image[] _slotsImages;
        [SerializeField] Image[] _cardsImages;

        ItemPickup itemPickup;
        Inventory inventory;

        private void Start()
        {
            inventory = new Inventory(largeInventory);
        }

        void Update()
        {
            if (uiButton.activeSelf && itemPickup != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (itemPickup != null && CanAddOtherItemToInventory()) {
                        uiButton.SetActive(false);
                        itemPickup.PickupItemForPlayer();
                        //add to the inventory
                        SaveNewItemInventory(itemPickup.GetItemInventory());
                    }
                }
                else if (Vector3.Distance(transform.position, itemPickup.gameObject.transform.position) > distance)
                {
                    itemPickup = null;
                    uiButton.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Item")
            {
                itemPickup = collision.GetComponent<ItemPickup>();
                uiButton.transform.position = collision.gameObject.transform.position;
                uiButton.SetActive(true);
            }
        }

        public bool InteractWithInventory() {
            bool interact = GOInventory.activeSelf;

            if (Input.GetKey(KeyCode.B) && !interact)
            {
                GOInventory.SetActive(true);
                interact = true;
            }
            else if(interact && Input.GetKey(KeyCode.Escape)) {
                GOInventory.SetActive(false);
                interact = false;
            }

            return interact;
        }

        private void SaveNewItemInventory(ItemInventory _itemInventory) {
            int indexEmpty = inventory.GetEmptySlotIndex();
            if (indexEmpty < 0) return;

            //change ui

            _cardsImages[indexEmpty].gameObject.SetActive(true);
            _cardsImages[indexEmpty].gameObject.GetComponent<UIControlTypeInventoryItem>().SetItemInventory(_itemInventory);
            _cardsImages[indexEmpty].sprite = _itemInventory.GetSpriteInventory();
            _cardsImages[indexEmpty].transform.SetParent(_slotsImages[indexEmpty].transform);
            _slotsImages[indexEmpty].GetComponent<CardSlotDrapDetection>().
                AddNewCardItemFromPlayerInventory(_cardsImages[indexEmpty].gameObject);

            //notify the slot that i put one card inside him

            //add item to inventory
            inventory.AddNewItem(_itemInventory, indexEmpty);
        }

        private bool CanAddOtherItemToInventory() => inventory.CanAddOtherItem();
    }
}
