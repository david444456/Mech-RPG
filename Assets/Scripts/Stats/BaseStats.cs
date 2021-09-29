using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using System;
using RPG.Core;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public string nameCharacter; 
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpPartycleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLevel = 0;
        Experience experience;

        private void Awake()
        {
            //to get levels difficult
            if(PlayerInformationBetweenScenes.gameManager != null)
                 progression = PlayerInformationBetweenScenes.gameManager.progressionPrincipal;
            

            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        public float GetStat( Stat stat)
        {

            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }


        public int GetLevel() {
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
                
            }

            return currentLevel;
        }

        public float GetExpToLevelUp() {
            return progression.GetStat(Stat.ExperecienceToLevelUp, characterClass, CalculateLevel());
        }

        public string stringTextDead() {
            return progression.GetStringDead(characterClass);
        }

        public CharacterClass GetTypeCharacter() {
            return characterClass;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifiers in provider.GetAdditiveModifier(stat)) {
                    total += modifiers;
                }
            }
            return total;
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpPartycleEffect, transform);
        }


        int CalculateLevel() {

            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();


            int PenultimateLevel = progression.GetLevels(Stat.ExperecienceToLevelUp, characterClass);
            for (int levels = 1; levels <= PenultimateLevel; levels++)
            {
                float XPToLevelpUP = progression.GetStat(Stat.ExperecienceToLevelUp, characterClass, levels);
                if (XPToLevelpUP > currentXP) {
                    return levels;
                }
            }

            return PenultimateLevel + 1;

        }

    }
}
