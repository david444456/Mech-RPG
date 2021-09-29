using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory {
    public class HoverUIItem : MonoBehaviour
    {
        public static HoverUIItem hoverUI;
        [SerializeField] GameObject _goItemHoverDescription;
        [SerializeField] Text _textDescriptionHover;
        [SerializeField] Vector2 _moveToRigthPositionUI = new Vector2(60, 0);

        void Start()
        {
            hoverUI = this;
        }

        public void ShowInfoWeapon(string stringInfo, RectTransform rectTransformCard) {
            _goItemHoverDescription.SetActive(true);
            _textDescriptionHover.text = stringInfo;

            //change position
            _goItemHoverDescription.transform.SetParent(rectTransformCard.parent);
            _goItemHoverDescription.GetComponent<RectTransform>().anchoredPosition =
                _moveToRigthPositionUI + rectTransformCard.anchoredPosition;
        }

        public void DesactiveObject() {
            _goItemHoverDescription.SetActive(false);
        }
    }
}
