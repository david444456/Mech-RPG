using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] Text textHealth;

        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            textHealth = GetComponent<Text>();
        }

        void Update()
        {
            if (fighter.GetTarget() == null) {
                textHealth.text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();
            textHealth.text = string.Format(textHealth.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints().ToString(), health.GetMaxHealthPoints().ToString()));
        }
    }
}
