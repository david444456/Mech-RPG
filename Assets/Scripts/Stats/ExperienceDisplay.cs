using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] Slider SliderEXP;
        [SerializeField] Text textLevel;

        Experience experience;
        Health health;
        BaseStats baseStats;

        private void Start()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            baseStats = experience.GetComponent<BaseStats>();

            SliderEXP = GetComponent<Slider>();
            UpdateLevel();
            experience.onExperienceGained += UpdateLevel;
        }

        void UpdateLevel()
        {
            SliderEXP.maxValue = baseStats.GetExpToLevelUp();
            SliderEXP.value = experience.GetPoints();
            textLevel.text = baseStats.GetLevel().ToString();
        }
    }
}
