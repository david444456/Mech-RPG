using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class DrapSystem : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IBeginDragHandler, IDragHandler
        , IPointerEnterHandler, IPointerExitHandler
    {
        public Action EventStartDrag = delegate { };
        public Action EventEndDrag = delegate { };
        public Action EventPointerRightClickCard = delegate { };
        public Action EventPointerEnterThisCard = delegate { };
        public Action EventPointerExitThisCard = delegate { };

        [SerializeField] Canvas PrincipalCanvas;

        Vector2 lastAnchoredPosition = new Vector2(0, 0);

        RectTransform rectTransform;
        CanvasGroup canvasGroup;
        HorizontalLayoutGroup HorizontalGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            HorizontalGroup = GetComponentInParent<HorizontalLayoutGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            EventStartDrag.Invoke();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta/ PrincipalCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            //ray slot detection
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {
                print(hit.transform.position);
                hit.transform.gameObject.SetActive(false);
            }*/

            rectTransform.anchoredPosition = lastAnchoredPosition;

            StartCoroutine(ActiveBlockRayCast());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventPointerEnterThisCard.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventPointerExitThisCard.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                EventPointerRightClickCard.Invoke();
        }

        IEnumerator ActiveBlockRayCast() {
            yield return new WaitForSeconds(0.1f);
            EventEndDrag.Invoke();
            canvasGroup.blocksRaycasts = true;
        }

        private void UpdateHorizontalGroup() {
            RectOffset tempPadding = new RectOffset(
                HorizontalGroup.padding.left,
                HorizontalGroup.padding.right,
                HorizontalGroup.padding.top,
                HorizontalGroup.padding.bottom);

            HorizontalGroup.padding = tempPadding;
        }
    }
}
