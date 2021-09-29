using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Text textHealth;
        [SerializeField] Slider slider;

        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            textHealth = GetComponent<Text>();
            slider = GetComponent<Slider>();
        }

        void Update()
        {
            slider.maxValue = health.GetMaxHealthPoints();
            slider.value = health.GetHealthPoints();
            //textHealth.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
