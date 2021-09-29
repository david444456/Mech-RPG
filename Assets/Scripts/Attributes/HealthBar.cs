using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healtComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        [SerializeField] Text LevelText;
        [SerializeField] Slider sliderHealth;

        private void Start()
        {
            LevelText.text = healtComponent.GetLevel().ToString();
            //sliderHealth.maxValue = healtComponent.GetInitialHealth();
            foreground.localScale = new Vector3(10, 1, 1);
            rootCanvas.enabled = false;
        }

        void Update()
        {
            if(Mathf.Approximately(healtComponent.GetFraction(), 0)
                || Mathf.Approximately(healtComponent.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healtComponent.GetFraction(), 1, 1);


        }
    }
}
