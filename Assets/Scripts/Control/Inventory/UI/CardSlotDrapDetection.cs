using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RPG.Inventory
{
    public class CardSlotDrapDetection : MonoBehaviour, IDropHandler
    {
        public Action<ItemInventory> EventShareNewItemInventory = delegate { };

        [SerializeField] Vector2 newAnchorerPositionCard = new Vector2(-15, -20);
        [SerializeField] Vector2 anchorsPivotCard = new Vector2(0.5f, 0.5f);
        [SerializeField] bool itIsSlot = false;
        [SerializeField] bool thisSlotWorksWithEliminatedItems = false;
        [SerializeField] bool worksForEliminatedThings = false;

        UIDrapControlTypeInventory uIDrapControlType;
        GameObject _lastItemCardInThisSlot;

        void Start()
        {
            uIDrapControlType = GetComponent<UIDrapControlTypeInventory>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null) {
                if (itIsSlot ||
                    uIDrapControlType.ItIsTheSameType(eventData.pointerDrag.GetComponent<UIControlTypeInventoryItem>()))
                {
                    //logic to error two objects in the same slot
                    if (_lastItemCardInThisSlot == null) _lastItemCardInThisSlot = eventData.pointerDrag.gameObject;
                    else if (thisSlotWorksWithEliminatedItems) EliminatedTheLastChildrenItem(eventData.pointerDrag.gameObject);
                    else return;

                    //logic to player
                    if (!itIsSlot) EventShareNewItemInventory.Invoke(
                        eventData.pointerDrag.GetComponent<UIControlTypeInventoryItem>().GetItemInventory());
                    else if (worksForEliminatedThings) EliminatedObjectThatDropInThisObject();

                    //set item in parent position
                    eventData.pointerDrag.transform.SetParent(this.transform);

                    RectTransform rectTransformItem = eventData.pointerDrag.GetComponent<RectTransform>();

                    rectTransformItem.pivot = anchorsPivotCard;
                    rectTransformItem.anchorMin = anchorsPivotCard;
                    rectTransformItem.anchorMax = anchorsPivotCard;
                    rectTransformItem.anchoredPosition = newAnchorerPositionCard;
                    rectTransformItem.sizeDelta = GetComponent<RectTransform>().sizeDelta;
                }
            }
        }

        public void AddNewCardItemFromPlayerInventory(GameObject lastGO) {
            _lastItemCardInThisSlot = lastGO;
        }

        public void ChangeToNullTheLastItemInSlot() => _lastItemCardInThisSlot = null;

        private void EliminatedObjectThatDropInThisObject() {

            _lastItemCardInThisSlot.SetActive(false);
            _lastItemCardInThisSlot = null;
        }

        private void EliminatedTheLastChildrenItem(GameObject newGameObject) {
            if (_lastItemCardInThisSlot == newGameObject) return;

            _lastItemCardInThisSlot.SetActive(false);
            _lastItemCardInThisSlot = newGameObject;
        }
    }
}
